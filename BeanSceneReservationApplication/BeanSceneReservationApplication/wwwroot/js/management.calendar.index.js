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
        editable: true,
        eventResizableFromStart: true,
        events: {
            url: '/api/sittings',
        },
        headerToolbar: {
            left: 'prev,next',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        selectable: true,
        select: function (info) {
            GetType();
            $("#sStart").val(info.start.toISOString().slice(0,22));
            $("#sEnd").val(info.end.toISOString().slice(0, 22));
            $("#CreateSittingModal").modal("show");
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
        eventDrop: function (info) {
            if (info.event.extendedProps.reservations.length == 0) {
                const sittingId = info.event.extendedProps.id

                const Sitting = {
                    Name: info.event.title,
                    StartTime: info.event.start,
                    EndTime: info.event.end,
                    Capacity: info.event.extendedProps.capacity,
                    Active: info.event.extendedProps.active
                }
                if (confirm(`Are you sure about this change ${sittingId}?`)) {
                    fetch(`/api/sittings/${sittingId}`, {
                        method: 'POST',
                        headers: {
                            'content-type': 'application/json'
                        },
                        body: JSON.stringify(Sitting)
                    })
                }
            } else {
                alert("You can't move a sitting with reservations.")
                info.revert();
            }
        },
        eventResize: function (info) {
            if (info.event.extendedProps.reservations.length != 0) {
                if (info.event.start <= info.oldEvent.start && info.event.end >= info.oldEvent.end) {
                    const sittingId = info.event.extendedProps.id

                    const Sitting = {
                        Name: info.event.title,
                        StartTime: info.event.start,
                        EndTime: info.event.end,
                        Capacity: info.event.extendedProps.capacity,
                        Active: info.event.extendedProps.active
                    }
                    if (confirm(`Are you sure you want to modify this sitting??`)) {
                        fetch(`/api/sittings/${sittingId}`, {
                            method: 'POST',
                            headers: {
                                'content-type': 'application/json'
                            },
                            body: JSON.stringify(Sitting)
                        })
                    }
                } else {
                    alert("You can't resize this sitting because conflicts with reservations may occur./r/nWhen reservations exist for a sitting, the duration can only be extended.");
                    info.revert;
                }
            } else {
                const sittingId = info.event.extendedProps.id

                const Sitting = {
                    Name: info.event.title,
                    StartTime: info.event.start,
                    EndTime: info.event.end,
                    Capacity: info.event.extendedProps.capacity,
                    Active: info.event.extendedProps.active
                }
                if (confirm(`Are you sure you want to modify this sitting?`)) {
                    fetch(`/api/sittings/${sittingId}`, {
                        method: 'POST',
                        headers: {
                            'content-type': 'application/json'
                        },
                        body: JSON.stringify(Sitting)
                    })
                }
            }
        },
    });

    calendar.render();

    $('#active').change(function () {
        var Active = $('#active').is(':checked')
        msg = "";
        let color = "";
        let editable;
        let active;
        if (Active) {
            msg = "Do you want to make this sitting active?";
            color = "CornflowerBlue";
            editable = false;
            active = true;
        } else {
            msg = "Do you want to make this sitting inactive?";
            color = "red";
            editable = true;
            active = false;
        }
        if (confirm(msg)) {
            fetch(`/api/sittings/active/${clickInfo.event.extendedProps.id}`, {
                method: 'POST',
                headers: {
                    'content-type': 'application/json'
                },
                body: JSON.stringify(Active)
            })
                .then(response => {
                    if (response.ok) {
                        calendar.refetchEvents();
                    } else {
                        e.preventDefault();
                    }
                });
        } else {
            $("#active").prop("checked", clickInfo.event.extendedProps.active);
        }
    });

    $('#DeleteBtn').click(function () {
        if (!clickInfo.event.extendedProps.active) {
            if (clickInfo.event.extendedProps.reservations.length == 0) {
                if (confirm(`Do you want to delete this sitting?`)) {
                    fetch(`/api/sittings/delete/${clickInfo.event.extendedProps.id}`, {
                        method: 'POST',
                    })
                        .then(response => {
                            if (response.ok) {
                                calendar.refetchEvents();
                            } else {
                                e.preventDefault();
                            }
                        });
                }
            } else {
                alert("This sitting has reservations!\nIf you want to delete the sitting, please first delete all existing reservations.");
            }

        } else {
            alert("Please make this sitting inactive first.");
        }
    });

    $('#ViewAllBtn').click(function () {
        window.location = `/Management/list/indexes`;
    });

    $('#ViewBtn').click(function () {
        window.location = `/Management/list/index?id=${clickInfo.event.extendedProps.id}`;
    });

    $('#CreateForm').on("submit", function (e) {
        e.preventDefault();

        var selectedDays = [];
        $('#sDaysOfWeek input[type="checkbox"]:checked').each(function () {
            selectedDays.push($(this).val());
        });

        const CreateSittingsVM = {
            Name: $("#sName").val(),
            TypeName: $('#sType').val(),
            StartTime: $("#sStart").val(),
            EndTime: $("#sEnd").val(),
            Capacity: $("#sCapacity").val(),
            Active: $("#sActive").is(':checked'),
            BatchStartDate: $("#sBatchStartDate").val(),
            BatchEndDate: $("#sBatchEndDate").val(),
            DaysOfWeek: selectedDays,
        };
        console.log(CreateSittingsVM)
        debugger
        fetch(`/api/sittings/create`, {
            method: 'POST',
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify(CreateSittingsVM)
        })
            .then(response => {
                if (response.ok) {
                    calendar.refetchEvents();
                    $('#CreateSittingModal').modal('hide');
                } else {
                    response.json().then(data => {
                        let errorMessage = "";

                        // Iterate over the error categories and extract the error messages
                        for (const category in data) {
                            if (data.hasOwnProperty(category)) {
                                const errorMessages = data[category];

                                // Join the error messages into a single string
                                const formattedErrorMessages = errorMessages.join("\n");

                                // Add the error category and messages to the main error message string
                                errorMessage += `${category}:\n${formattedErrorMessages}\n\n`;
                            }
                        }

                        // Display the error messages
                        if (errorMessage) {
                            alert(errorMessage);
                        } else {
                            // If no specific error messages found, display a generic error message
                            alert("There is a problem with the sitting details. Please try again.");
                        }
                    });
                }
            })
    });

    $('#CreateTypeBtn').on('click', function () {
        if ($('#typeName').val() != "") {
            const CreateType = {
                Name: $("#typeName").val()
            };
            fetch(`/api/sittings/createType`, {
                method: 'POST',
                headers: {
                    'content-type': 'application/json'
                },
                body: JSON.stringify(CreateType)
            })
                .then(response => {
                    if (response.ok) {
                        GetType();
                    } else {
                        alert("There are issues with the data. Please try again.")
                    }
                });
        } else {
            alert("Please decide on a name.")
        }

    });

    async function GetType() {
        const response = await fetch(`/api/sittings/getType`);
        const jsonData = await response.json();
        $('#sType').empty();
        for (let i = 0; i < jsonData.length; i++) {
            $('#sType').append(`<option>${jsonData[i].name}</option>`)
        }
    }

    $('#CreateSittingBtn').click('shown.bs.modal', () => {
        GetType();
    })
});