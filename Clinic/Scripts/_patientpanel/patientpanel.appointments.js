$(document).ready(function () {
    var dateToday = new Date();
    $("#appointmentdate").datepicker({
        dateFormat: 'dd-mm-yy',
        minDate: dateToday,
        onSelect: function (dateText) {
            $('#doctor option:eq(0)').prop('selected', true);
            $('#clinic option:eq(0)').prop('selected', true);

            $("#appointments").jsGrid("loadData");
        }
    });
    $("#appointmentdate").datepicker("setDate", dateToday);
    GetDoctors();
    GetClinics();

    $("#doctor").change(function () {
        $('#clinic option:eq(0)').prop('selected', true);
        $("#appointmentdate").val("");

        if($("#doctor").val() != -1)
            $("#appointments").jsGrid("loadData");
    });

    $("#clinic").change(function () {
        $('#doctor option:eq(0)').prop('selected', true);
        $("#appointmentdate").val("");

        if ($("#clinic").val() != -1)
            $("#appointments").jsGrid("loadData");
    });

    $("#appointments").jsGrid({
        height: "400px",
        width: "100%",
        editing: false,
        autoload: true,
        pageLoading: true,
        paging: true,
        rowClick: function (args) {
            showDetailsDialog(args.item);
        },
        onDataLoaded: function () {
            $(".jsgrid-table").css("width", "100%");
        },

        onItemInserted: function () {
            //$("#appointments").jsGrid("loadData");
        },

        fields: [
            { name: "ClinicName", title: "Clinic", type: "text" },
			{ name: "DoctorName", title: "Doctor", type: "text" },
			{ name: "DateName", title: "Date", type: "text" }
        ],

        controller: {
            loadData: function (filter) {
                var date = $("#appointmentdate").datepicker("getDate");
                var clinicId = $("#clinic").val();
                var doctorId = $("#doctor").val();

                if (date != null && date != '') {
                    var date = $("#appointmentdate").datepicker("getDate");
                    var day = date.getDate();
                    var month = date.getMonth() + 1;
                    var year = date.getFullYear();

                    return $.ajax({
                        type: "POST",
                        url: "/Appointment/GetAvailableAppointmentsByDate/",
                        data: '{ day: "' + day + '", month: "' + month + '", year: "' + year + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        cache: false
                    });
                }
                else if (clinicId != -1) {
                    return $.ajax({
                        type: "POST",
                        url: "/Appointment/GetFirstAppointmentByClinic/",
                        data: '{ clinicId: "' + clinicId + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        cache: false
                    });
                }
                else {
                    return $.ajax({
                        type: "POST",
                        url: "/Appointment/GetFirstAppointmentByDoctor/",
                        data: '{ doctorId: "' + doctorId + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        cache: false
                    });
                }
            },
            insertItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: "/Appointment/AddAppointment/",
                    data: '{ ClinicId: "' + item.ClinicId + '", DoctorId: "' + item.DoctorId + '", Date: "' + item.Date + '", Time: "' + item.Time + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (response) {
                        alert(response);
                    }
                });
            },
        }

    });
});

$("#detailsDialogAppointment").dialog({
    autoOpen: false,
    width: 400,
    close: function () {
        $("#detailsFormAppointment").validate().resetForm();
        $("#detailsFormAppointment").find(".error").removeClass("error");
    }
});

$("#detailsFormAppointment").validate({
    rules: {
        Appointment_Clinic: "required",
        Appointment_Doctor: "required",
        Appointment_Date: "required",
        Appointment_Time: "required"
    },
    messages: {
        Appointment_Clinic: "Please enter clinic",
        Appointment_Doctor: "Please enter doctor",
        Appointment_Date: "Please enter date",
        Appointment_Time: "Please enter time"
    },
    submitHandler: function () {
        formSubmitHandler();
    }
});

var formSubmitHandler = $.noop;

var showDetailsDialog = function (element) {
    $("#Appointment_Clinic").val(element.ClinicName).prop("readonly", true);
    $("#Appointment_ClinicId").val(element.ClinicId);
    $("#Appointment_Doctor").val(element.DoctorName).prop("readonly", true);
    $("#Appointment_DoctorId").val(element.DoctorId);
    $("#Appointment_Date").val(element.DateName.split(" ")[0]).prop("readonly", true);
    $("#Appointment_Time").val(element.DateName.split(" ")[1]).prop("readonly", true);

    formSubmitHandler = function () {
        saveElement(element);
    };

    $("#detailsDialogAppointment").dialog("option", "title", "Add Appointment").dialog("open");
};

var saveElement = function (element) {
    $.extend(element, {
        ClinicId: $("#Appointment_ClinicId").val(),
        DoctorId: $("#Appointment_DoctorId").val(),
        Date: $("#Appointment_Date").val(),
        Time: $("#Appointment_Time").val()
    });

    $("#appointments").jsGrid("insertItem", element);

    $("#detailsDialogAppointment").dialog("close");
};

function GetDoctors() {
    var myUrl = "/Doctor/GetDoctors";

    $.ajax({
        type: "POST",
        url: myUrl,
        data: '{ }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (response) {
            $("#doctor option").remove();
            $("#doctor").append('<option value="-1"></option>');
            for (var i = 0; i < response.itemsCount; i++)
                $("#doctor").append('<option value="' + response.data[i].UserId + '">' + response.data[i].FirstName + ' ' + response.data[i].LastName + '</option>');
        }
    });
}

function GetClinics() {
    var myUrl = "/Clinic/GetClinics";

    $.ajax({
        type: "POST",
        url: myUrl,
        data: '{ }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (response) {
            $("#clinic option").remove();
            $("#clinic").append('<option value="-1"></option>');
            for (var i = 0; i < response.itemsCount; i++)
                $("#clinic").append('<option value="' + response.data[i].Id + '">' + response.data[i].Name + '</option>');
        }
    });
}