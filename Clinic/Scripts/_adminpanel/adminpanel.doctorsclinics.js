$(document).ready(function () {
    $("#doctorsclinics").jsGrid({
        height: "400px",
        width: "100%",
        editing: false,
        autoload: false,
        pageLoading: true,
        paging: true,
        deleteConfirm: function (item) {
            return "Doctor \"" + item.DoctorName + "\" will be removed from " + item.ClinicName + ". Are you sure?";
        },
        onDataLoaded: function () {
            $(".jsgrid-table").css("width", "100%");
        },

        fields: [
            { name: "DoctorName", title: "Doctor", type: "text" },
            { name: "ClinicName", title: "Clinic", type: "text" },
            {
                type: "control",
                modeSwitchButton: false,
                editButton: false,
                headerTemplate: function () {
                    return $("<button>").attr("type", "button").text("Add")
                            .on("click", function () {
                                showDoctorsClinicsDialog("Add", {});
                            });
                }
            }
        ],

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: "/Doctor/GetDoctorsClinics/",
                    data: "",
                    dataType: "json"
                });
            },
            insertItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: "/Doctor/AddDoctorClinic/",
                    data: item,
                    dataType: "json",
                    success: function (response) {
                        alert(response);
                    }
                });
            },
            deleteItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: "/Doctor/RemoveDoctorClinic/",
                    data: item,
                    dataType: "json"
                });
            },
        }
    });

    $("#detailsDialogDoctorClinic").dialog({
        autoOpen: false,
        width: 400,
        close: function () {
            $("#detailsFormDoctorClinic").validate().resetForm();
            $("#detailsFormDoctorClinic").find(".error").removeClass("error");
        }
    });

    $("#detailsFormDoctorClinic").validate({
        submitHandler: function () {
            formSubmitHandler();
        }
    });

    GetDoctors();
    GetClinics();

    //var formSubmitHandler = $.noop;

    var showDoctorsClinicsDialog = function (dialogType, element) {
        formSubmitHandler = function () {
            saveDoctorClinic(element, dialogType === "Add");
        };

        $("#detailsDialogDoctorClinic").dialog("option", "title", dialogType + " Doctor-Clinic").dialog("open");
    };

    var saveDoctorClinic = function (element, isNew) {
        $.extend(element, {
            ClinicId: $("#DoctorClinic_Clinic").val(),
            DoctorId: $("#DoctorClinic_Doctor").val()
        });

        $("#doctorsclinics").jsGrid("insertItem", element);

        $("#detailsDialogDoctorClinic").dialog("close");
    };
});

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
            $("#DoctorClinic_Doctor option").remove();
            for (var i = 0; i < response.itemsCount; i++)
                $("#DoctorClinic_Doctor").append('<option value="' + response.data[i].UserId + '">' + response.data[i].FirstName + ' ' + response.data[i].LastName + '</option>');
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
            $("#DoctorClinic_Clinic option").remove();
            for (var i = 0; i < response.itemsCount; i++)
                $("#DoctorClinic_Clinic").append('<option value="' + response.data[i].Id + '">' + response.data[i].Name + '</option>');
        }
    });
}