$(document).ready(function () {
    $('#clinicsLink').click(function (e) {
        e.preventDefault();
        $("#clinics").jsGrid("loadData");
    });

    $('#doctorsLink').click(function (e) {
        e.preventDefault();
        $("#doctors").jsGrid("loadData");
    });

    $('#doctorsclinicsLink').click(function (e) {
        e.preventDefault();
        $("#doctorsclinics").jsGrid("loadData");
    });

    $('#patientsLink').click(function (e) {
        e.preventDefault();
        $("#patients").jsGrid("loadData");
    });

    $('#appointmentsLink').click(function (e) {
        e.preventDefault();
        $("#appointments").jsGrid("loadData");
    });
});