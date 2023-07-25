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
            url: '/api/reservation/getsittings',
        },
        editable: false,
        headerToolbar: {
            left: 'prev,next',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        eventClick: function (info) {
            //Generating details for modal
            $('#title').html(info.event.title);
            $('#date').html(new Date(info.event.start).toDateString());
            let time = new Date(info.event.start).toUTCString().slice(17, 22) + " ~ " + new Date(info.event.end).toUTCString().slice(17, 22);
            $('#time').html(time);
            $('#seatsRemaining').html(info.event.extendedProps.seatsRemaining);
            $('#sittingId').val(info.event.extendedProps.sittingId);
            $('#sittingModal').modal('toggle');

            clickInfo = info

            /******************/
            /**Preparing form**/
            /******************/

            //Setting min/max time for reservation start time input
            var startTimeInput = $("#startTimeInput");

            var endTime = new Date(info.event.end);
            var modifiedEndTime = new Date(endTime);
            modifiedEndTime.setHours(endTime.getHours() - 1);//Max reservation start time is 1 hour before end of sitting
            var minTime = new Date(info.event.start).toUTCString().slice(17, 22);
            var maxTime = new Date(modifiedEndTime).toUTCString().slice(17, 22);
            startTimeInput.attr("min", minTime);
            startTimeInput.attr("max", maxTime);

            //Displays selected sitting details above reservation form
            $('input[name="Sitting.Id"]').val($('#sittingId').val());
            $('#sittingNameDisplay').text($('#title').text());
            $('#sittingTimeDisplay').text($('#date').text() + " " + $('#time').text());
            $('#sittingSeatsDisplay').text($('#seatsRemaining').text() + " seats remain");
        },
    });
    
    calendar.render();

    $('#selectButton').click(function () {
        $('#calendarContainer').hide(200, "swing", function () { $('#formContainer').show(200, "swing"); });        
    });

    $('#changeSittingButton').click(function () {
        $('#formContainer').hide(200, "swing", function () { $('#calendarContainer').show(200, "swing"); });
    });

    if (window.modelData !== null) {
        $('#calendarContainer').hide();
        $('#formContainer').show();
    }
});
