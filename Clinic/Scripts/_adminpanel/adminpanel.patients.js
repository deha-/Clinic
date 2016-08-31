$(document).ready(function () {
    $("#patients").jsGrid({
        height: "400px",
        width: "100%",
        editing: false,
        autoload: false,
        pageLoading: true,
        paging: true,
        rowClick: function (args) {
            if (!args.item.IsApproved) {
                showConfirmDialog(args.item);
            }
        },
        deleteConfirm: function (item) {
            return "The patient will be removed. Are you sure?";
        },
        onDataLoaded: function () {
            $(".jsgrid-table").css("width", "100%");
        },

        fields: [
            { name: "FirstName", title: "First Name", type: "text" },
            { name: "LastName", title: "Last Name", type: "text" },
            { name: "PESEL", type: "text" },
            { name: "Address", type: "text" },
            { name: "IsApproved", type: "checkbox", title: "Is Approved" },
            {
                type: "control",
                modeSwitchButton: false,
                editButton: false
            }
        ],

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: "/Patient/GetPatients/",
                    data: "",
                    dataType: "json"
                });
            },
            deleteItem: function (item) {
                return $.ajax({
                    type: "POST",
                    url: "/Patient/RemovePatient/",
                    data: item,
                    dataType: "json"
                });
            },
        }
    });
});

$("#activateUserDialog").dialog({
    autoOpen: false,
    width: 400,
    buttons: {
        "Ok": function () {
            var userId = $("#ActivateUser_UserId").val();
            ActivateUser(userId);
            $(this).dialog("close");
            $("#patients").jsGrid("loadData");
        },
        "Cancel": function () {
            $(this).dialog("close");
        }
    }
});

var showConfirmDialog = function (element) {
    $("#ActivateUser_UserId").val(element.UserId);

    $("#activateUserDialog").dialog("option", "title", "Activate User").dialog("open");
};

function ActivateUser(userId) {
    var myUrl = "/Patient/ActivateUser";

    $.ajax({
        type: "POST",
        url: myUrl,
        data: '{ userId: "' + userId + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false
    });
}