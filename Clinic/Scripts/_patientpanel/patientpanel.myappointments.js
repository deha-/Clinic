$(document).ready(function () {
    $("#myappointments").jsGrid({
        height: "400px",
        width: "100%",
        editing: false,
        autoload: false,
        pageLoading: true,
        paging: true,
        rowClick: function (args) {
            if (!args.item.IsConfirmed) {
                showConfirmDialog(args.item);
            }
        },
        onDataLoaded: function() {
            $(".jsgrid-table").css("width", "100%");
        },

        fields: [
            { name: "ClinicName", title: "Clinic", type: "text", width: "30%" },
			{ name: "DoctorName", title: "Doctor", type: "text", width: "30%" },
			{ name: "DateName", title: "Date", type: "text", width: "15%" },
            { name: "IsConfirmed", type: "checkbox", title: "IsConfirmed", sorting: false, width: "10%" },
            { name: "AddedDateName", title: "Date Added", type: "text", width: "15%" }
        ],

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: "/Appointment/GetAppointmentsByPatient/",
                    data: "",
                    dataType: "json"
                });
            }
        }

    });
});

$("#confirmAppointmentDialog").dialog({
    autoOpen: false,
    width: 400,
    buttons: {
        "Ok": function () {
            var appointmentId = $("#MyAppointment_AppointmentId").val();
            ConfirmAppointment(appointmentId);
            $(this).dialog("close");
            $("#myappointments").jsGrid("loadData");
        },
        "Cancel": function () {
            $(this).dialog("close");
        }
    }
});

var showConfirmDialog = function (element) {
    $("#MyAppointment_AppointmentId").val(element.Id);

    $("#confirmAppointmentDialog").dialog("option", "title", "Confirm Appointment").dialog("open");
};

function ConfirmAppointment(appointmentId) {
    var myUrl = "/Appointment/ConfirmAppointment";

    $.ajax({
        type: "POST",
        url: myUrl,
        data: '{ appointmentId: "' + appointmentId + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false
    });
}