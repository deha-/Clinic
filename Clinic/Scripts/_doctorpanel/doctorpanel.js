$(document).ready(function () {
    $('#appointmentsLink').click(function (e) {
        e.preventDefault();
        $("#appointments").jsGrid("loadData");
    });

    $('#schedulesLink').click(function (e) {
        e.preventDefault();
        $("#schedules").jsGrid("loadData");
    });
});