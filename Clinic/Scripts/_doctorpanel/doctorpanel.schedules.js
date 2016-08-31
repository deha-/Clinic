var clinics = [];

$(document).ready(function () {
    $("#schedules").jsGrid({
        height: "400px",
        width: "100%",
        editing: false,
        autoload: false,
        pageLoading: true,
        paging: true,
        deleteConfirm: function (item) {
            return "The schedule will be removed. Are you sure?";
        },
        rowClick: function (args) {
            showDetailsDialog("Edit", args.item);
        },
        onDataLoaded: function () {
            $(".jsgrid-table").css("width", "100%");
        },

        fields: [
            { name: "ClinicSchedule", title: "Clinics", type: "text", width: "100%" }
        ],

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: "/Schedule/GetScheduleByDoctor/",
                    data: "",
                    dataType: "json"
                });
            },
            insertItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: "/Schedule/AddSchedule/",
                    data: item,
                    dataType: "json"
                });
            },
            updateItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: "/Schedule/UpdateSchedule/",
                    data: item,
                    dataType: "json"
                });
            },
            deleteItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: "/Schedule/RemoveSchedule/",
                    data: item,
                    dataType: "json"
                });
            },
        },

        rowRenderer: function (item) {
            var clinic = { Id: item.ClinicId, Name: item.ClinicName };
            clinics.push(clinic);

            var $clinic = $("<div>").append($("<p>").append($("<strong>").text(item.ClinicName)));

            var $schedules = $("<div>");
            for (var i = 0; i < item.schedules.length; i++) {
                $schedules = $schedules.append($("<p>").text(item.schedules[i].DayName + ": " + item.schedules[i].FromStr + " - " + item.schedules[i].ToStr));
            }

            return $("<tr>").append($("<td>").append($clinic).append($schedules));
        },

    });

    GetDays();

    $("#Schedule_Day").change(function () {
        var day = $("#Schedule_Day").val();
        var clinicId = $("#Schedule_Clinic").val();
        GetDayScheduleByDoctor(clinicId, day);
    });

    $("#Remove_Schedule").click(function () {
        var scheduleId = $("#Remove_Schedule_Time").val();

        RemoveSchedule(scheduleId);
    });

    $("#Add_Schedule").click(function () {
        var clinicId = $("#Add_Schedule_Clinic").val();
        var day = $("#Add_Schedule_Day").val();
        var from = $("#Add_Schedule_From").val();
        var to = $("#Add_Schedule_To").val();

        AddSchedule(clinicId, day, from, to);
    });
    //GetDoctorClinics();
    $('#Add_Schedule_From').mask('00:00');
    $('#Add_Schedule_To').mask('00:00');
});

/*$("#detailsDialogSchedule").dialog({
    autoOpen: false,
    width: 400,
    close: function () {
        $("#detailsFormSchedule").validate().resetForm();
        $("#detailsFormSchedule").find(".error").removeClass("error");
    }
});*/

$('#schedules-dialog').dialog({
    autoOpen: false,
    width: 400,

    create: function () {
        $('#schedules-tab').tabs({
            create: function (e, ui) {
                $(this).parent().find('.tabdialog-close').click(function () {
                    $('#schedules-dialog').dialog('close');
                });
            }
        });

        $(this).parent().children('.ui-dialog-titlebar').remove();
    },

    close: function () {
        //$("#schedules-dialog").validate().resetForm();
        //$("#schedules-dialog").find(".error").removeClass("error");
    },

    buttons: {
        Close: function () { $(this).dialog("close"); }
    }
});

$("#detailsFormSchedule").validate({
    /*rules: {
        Schedule_Clinic: "required",
        Schedule_Day: "required",
        Schedule_From: "required",
        Schedule_To: "required"
    },
    messages: {
        Schedule_Clinic: "Please choose clinic",
        Schedule_Day: "Please choose day",
        Schedule_From: "Please enter time",
        Schedule_To: "Please enter time"
    },*/
    submitHandler: function () {
        formSubmitHandler();
    }
});

var formSubmitHandler = $.noop;

var showDetailsDialog = function (dialogType, element) {
    $("#Add_Schedule_Clinic option").remove();

    for (var i = 0; i < clinics.length; i++)
        $("#Add_Schedule_Clinic").append('<option value="' + clinics[i].Id + '">' + clinics[i].Name + '</option>');

    $("#Remove_Schedule_Clinic").text(element.ClinicName);

    $("#Remove_Schedule_Time option").remove();

    for (var i = 0; i < element.schedules.length; i++) {
        var time = element.schedules[i].DayName + ", " + element.schedules[i].FromStr + " - " + element.schedules[i].ToStr;
        $("#Remove_Schedule_Time").append('<option value="' + element.schedules[i].Id + '">' + time + '</option>');
    }

    $('#Add_Schedule_Clinic option:first-child').attr("selected", "selected");
    $('#Add_Schedule_Day option:first-child').attr("selected", "selected");
    $("#Add_Schedule_From").val("");
    $("#Add_Schedule_To").val("");

    /*if (dialogType === "Edit") {
        $("#Schedule_Clinic").val(element.Clinic.Id);
        $("#Schedule_Day").val(element.Day);
        $("#Schedule_From").val(element.FromStr);
        $("#Schedule_To").val(element.ToStr);
    }
    else {
        $("#Schedule_Clinic").val("");
        $("#Schedule_Day").val("");
        $("#Schedule_From").val("");
        $("#Schedule_To").val("");
    }*/

    formSubmitHandler = function () {
        saveElement(element, dialogType === "Add");
    };

    $('#schedules-dialog').dialog("option", "title", dialogType + " Schedule").dialog("open");
};

var saveElement = function (element, isNew) {
    $.extend(element, {
        /*ClinicId: $("#Schedule_Clinic").val(),
        Day: $("#Schedule_Day").val(),
        From: $("#Schedule_From").val(),
        To: $("#Schedule_To").val()*/
    });

    $("#schedules").jsGrid(isNew ? "insertItem" : "updateItem", element);

    $("#detailsDialogSchedule").dialog("close");
};

function GetDays() {
    var myUrl = "/Schedule/GetDays";

    $.ajax({
        type: "POST",
        url: myUrl,
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (response) {
            $("#Add_Schedule_Day option").remove();
            for(var i = 0; i < response.itemsCount; i++)
                $("#Add_Schedule_Day").append('<option value="' + response.data[i].Key + '">' + response.data[i].Value + '</option>');
        }
    });
}

/*function GetDoctorClinics() {
    var myUrl = "/Doctor/GetDoctorClinics";

    $.ajax({
        type: "POST",
        url: myUrl,
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (response) {
            for (var i = 0; i < response.itemsCount; i++)
                $("#Schedule_Clinic").append('<option value="' + response.data[i].Id + '">' + response.data[i].Name + '</option>');
        }
    });
}*/

function GetDayScheduleByDoctor(clinicId, day) {
    var myUrl = "/Schedule/GetDayScheduleByDoctor";

    $.ajax({
        type: "POST",
        url: myUrl,
        data: '{ clinicId: "' + clinicId + '", day: "' + day + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (response) {
            $("#Schedule_From option").remove();
            for (var i = 0; i < response.itemsCount; i++)
                $("#Schedule_From").append('<option value="' + response.data[i].Id + '">' + response.data[i].FromStr + '</option>');
        }
    });
}

function RemoveSchedule(scheduleId) {
    var myUrl = "/Schedule/RemoveSchedule";

    $.ajax({
        type: "POST",
        url: myUrl,
        data: '{ scheduleId: "' + scheduleId + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (response) {
            $("#schedules").jsGrid("loadData");
            $('#schedules-dialog').dialog('close');

            if(alert != null)
                alert(response);
        }
    });
}

function AddSchedule(clinicId, day, from, to) {
    var myUrl = "/Schedule/AddSchedule";

    $.ajax({
        type: "POST",
        url: myUrl,
        data: '{ clinicId: "' + clinicId + '", day: "' + day + '", from: "' + from + '", to: "' + to + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (response) {
            $("#schedules").jsGrid("loadData");
            $('#schedules-dialog').dialog('close');

            alert(response);
        }
    });
}