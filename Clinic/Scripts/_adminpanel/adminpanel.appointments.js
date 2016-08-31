$(document).ready(function () {
	$("#appointments").jsGrid({
		height: "400px",
		width: "100%",
		editing: false,
		autoload: false,
		pageLoading: true,
		paging: true,
		deleteConfirm: function (item) {
			return "The appointment will be cancelled. Are you sure?";
		},
		onDataLoaded: function () {
		    $(".jsgrid-table").css("width", "100%");
		},

		fields: [
            { name: "ClinicName", title: "Clinic", type: "text" },
			{ name: "DoctorName", title: "Doctor", type: "text" },
			{ name: "PatientName", title: "Patient", type: "text" },
			{ name: "DateName", title: "Date", type: "text" },
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
					url: "/Appointment/GetAppointments/",
					data: "",
					dataType: "json"
				});
			},
			deleteItem: function (item) {
			    return $.ajax({
			        type: "POST",
			        url: "/Appointment/RemoveAppointment/",
			        data: item,
			        dataType: "json",
			        success: function (response) {
			            alert(response);
			        }
			    });
			},
		}
			
	});
});