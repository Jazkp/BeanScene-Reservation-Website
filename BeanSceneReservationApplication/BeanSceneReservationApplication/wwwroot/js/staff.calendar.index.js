$(() => {
    var calendarEl = $('#calendar');

    var clickInfo;

    var calendar = new FullCalendar.Calendar(calendarEl[0], {
        timeZone: 'UTC',
        eventTimeFormat: {
            date: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            meridiem: false
        },
        initialView: 'timeGridWeek',
        events: {
            url: '/api/sittings',
        },
        editable: false,
        headerToolbar: {
            left: 'prev,next',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        eventClick: function (info) {
            $('#title').html(info.event.title);
            $('#date').html(new Date(info.event.start).toDateString());

            let time = new Date(info.event.start).toUTCString().slice(17, 22) + " ~ " + new Date(info.event.end).toUTCString().slice(17, 22);
            $('#time').html(time);
            $('#capacity').html(info.event.extendedProps.capacity);
            $("#active").prop("checked", info.event.extendedProps.active);
            $('#reservations').html(info.event.extendedProps.reservations.length)

            $('#SittingModal').modal('toggle');

            clickInfo = info

        },
        eventResize: function (info) {
            info.revert();
        },
        eventDrop: function (info) {
            info.revert();
        }
    });

    calendar.render();

    $('#ViewAllBtn').click(function () {
        window.location = `/Staff/list/indexes`;
    });

    $('#ViewBtn').click(function () {
        window.location = `/Staff/list/index?id=${clickInfo.event.extendedProps.id}`;
    });
});