$(document).ready(function () {
	$("#clinics").jsGrid({
		height: "400px",
		width: "100%",
		editing: true,
		autoload: true,
		pageLoading: true,
		paging: true,
		deleteConfirm: function (item) {
			return "The clinic \"" + item.Name + "\" will be removed. Are you sure?";
		},
		rowClick: function (args) {
			showDetailsDialog("Edit", args.item);
		},
		onDataLoaded: function () {
		    $(".jsgrid-table").css("width", "100%");
		},

		fields: [
            { name: "Name", type: "text" },
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
					url: "/Clinic/GetClinics/",
					data: "",
					dataType: "json"
				});
			},
			insertItem: function (item) {
				return $.ajax({
					type: "POST",
					url: "/Clinic/AddClinic/",
					data: item,
					dataType: "json",
					success: function (response) {
					    alert(response);
					}
				});
			},
			updateItem: function (item) {
				return $.ajax({
					type: "POST",
					url: "/Clinic/UpdateClinic/",
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
					url: "/Clinic/RemoveClinic/",
					data: item,
					dataType: "json",
					success: function (response) {
					    alert(response);
					}
				});
			},
		}
	});

	$("#detailsDialogClinic").dialog({
		autoOpen: false,
		width: 400,
		close: function () {
			$("#detailsFormClinic").validate().resetForm();
			$("#detailsFormClinic").find(".error").removeClass("error");
		}
	});

	$("#detailsFormClinic").validate({
		rules: {
		    Clinic_Name: "required"
		},
		messages: {
			Clinic_Name: "Please enter name"
		},
		submitHandler: function () {
		    formSubmitHandler();
		}
	});

	//var formSubmitHandler = $.noop;

	var showDetailsDialog = function (dialogType, element) {
		$("#Clinic_Name").val(element.Name);

		formSubmitHandler = function () {
			saveElement(element, dialogType === "Add");
		};

		$("#detailsDialogClinic").dialog("option", "title", dialogType + " Clinic").dialog("open");
	};

	var saveElement = function (element, isNew) {
		$.extend(element, {
			Name: $("#Clinic_Name").val()
		});

		$("#clinics").jsGrid(isNew ? "insertItem" : "updateItem", element);

		$("#detailsDialogClinic").dialog("close");
	};
});