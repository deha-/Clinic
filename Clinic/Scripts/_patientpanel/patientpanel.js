$(document).ready(function () {
    $('#appointmentsLink').click(function (e) {
        e.preventDefault();
        $("#appointments").jsGrid("loadData");
    });

    $('#myappointmentsLink').click(function (e) {
        e.preventDefault();
        $("#myappointments").jsGrid("loadData");
    });
});