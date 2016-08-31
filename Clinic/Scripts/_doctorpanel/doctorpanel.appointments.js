$(document).ready(function () {
    $("#appointments").jsGrid({
        height: "400px",
        width: "100%",
        editing: false,
        autoload: true,
        pageLoading: true,
        paging: true,
        deleteConfirm: function (item) {
            return "The appointment will be removed. Are you sure?";
        },
        onDataLoaded: function () {
            $(".jsgrid-table").css("width", "100%");
        },

        fields: [
            { name: "ClinicName", title: "Clinic", type: "text" },
			{ name: "PatientName", title: "Patient", type: "text" },
			{ name: "DateName", title: "Date", type: "text" }
        ],

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: "/Appointment/GetAppointmentsByDoctor/",
                    data: "",
                    dataType: "json"
                });
            }
        }

    });
});