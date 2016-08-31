$(document).ready(function () {
    $("#doctors").jsGrid({
        height: "400px",
        width: "100%",
        editing: false,
        autoload: false,
        pageLoading: true,
        paging: true,
        deleteConfirm: function (item) {
            return "Doctor \"" + item.FirstName + " " + item.LastName + "\" will be removed. Are you sure?";
        },
        onDataLoaded: function () {
            $(".jsgrid-table").css("width", "100%");
        },

        fields: [
            { name: "FirstName", title: "First Name", type: "text" },
            { name: "LastName", title: "Last Name", type: "text" },
            { name: "PWZ", type: "text" },
            {
                type: "control",
                modeSwitchButton: false,
                editButton: false,
                headerTemplate: function () {
                    return $("<button>").attr("type", "button").text("Add")
                            .on("click", function () {
                                showDetailsDialog("Add", {});
                            });
                }
            }
        ],

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: "/Doctor/GetDoctors/",
                    data: "",
                    dataType: "json"
                });
            },
            insertItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: "/Doctor/AddDoctor/",
                    data: item,
                    dataType: "json"
                });
            },
            deleteItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: "/Doctor/RemoveDoctor/",
                    data: item,
                    dataType: "json"
                });
            },
        }
    });
    
    $("#detailsDialogDoctor").dialog({
        autoOpen: false,
        width: 400,
        close: function () {
            $("#detailsFormDoctor").validate().resetForm();
            $("#detailsFormDoctor").find(".error").removeClass("error");
        }
    });

    jQuery.validator.addMethod("samePasswords", function (value, element) {
        return $("#Doctor_Password").val() == $("#Doctor_ConfirmPassword").val() && $("#Doctor_Password").val() != null && $("#Doctor_Password").val() != "";
    }, "Invalid password confirmation");

    $("#detailsFormDoctor").validate({
        rules: {
            Doctor_FirstName: "required",
            Doctor_LastName: "required",
            Doctor_PWZ: "required",
            Doctor_Login: "required",
            Doctor_Password: "required",
            Doctor_ConfirmPassword: { samePasswords: true },
        },
        messages: {
            Doctor_FirstName: "Please enter first name",
            Doctor_LastName: "Please enter last name",
            Doctor_PWZ: "Please enter PWZ number",
            Doctor_Login: "Please enter login",
            Doctor_Password: "Please enter password"
        },
        submitHandler: function () {
            formSubmitHandler();
        }
    });

    var formSubmitHandler = $.noop;

    var showDetailsDialog = function (dialogType, element) {
        formSubmitHandler = function () {
            saveElement(element, dialogType === "Add");
        };

        $("#detailsDialogDoctor").dialog("option", "title", dialogType + " Doctor").dialog("open");
    };

    var saveElement = function (element, isNew) {
        $.extend(element, {
            FirstName: $("#Doctor_FirstName").val(),
            LastName: $("#Doctor_LastName").val(),
            PWZ: $("#Doctor_PWZ").val(),
            Login: $("#Doctor_Login").val(),
            Password: $("#Doctor_Password").val(),
            ConfirmPassword: $("#Doctor_ConfirmPassword").val()
        });

        $("#doctors").jsGrid("insertItem", element);

        $("#detailsDialogDoctor").dialog("close");
    };
});