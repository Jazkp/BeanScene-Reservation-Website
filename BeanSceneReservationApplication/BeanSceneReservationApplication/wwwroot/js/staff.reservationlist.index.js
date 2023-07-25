$(async () => {
    //get sittingId from URL param
    const url = await new URL(window.location.href);
    const params = url.searchParams;
    const sittingId = params.get("id")

    const statusRes = await fetch(`/api/reservations/get/status/${sittingId}`);
    const statusJson = await statusRes.json();
    let Selecter = "";
    for (let i = 0; i < statusJson.length; i++) {
        Selecter += `<option value="${statusJson[i].name}">${statusJson[i].name}</option>`;
    };    

    //make DataTable
    let table = $('#myTable').DataTable({
        ajax: {
            url: `/api/reservations/get/${sittingId}`,
            dataSrc: ''
        },
        columns: [
            {
                className: 'dt-control',
                orderable: false,
                data: null,
                defaultContent: '',
            },
            { data: 'name' },
            { data: 'guests' },
            { data: 'start' },
            {
                data: 'duration',
                render: function (data, type, row) {
                    return data + "min";
                }
            },
            {
                data: 'status',
                render: function (data, type, row) {
                    return `<p id="status-${row.id}">${data}</p>` +
                        `<select id="select-${row.id}" class="form-select">` +
                        Selecter+
                        '</select>' +
                        `<button onclick="updateStatus(${row.id})" type="button" class="btn btn-primary my-2">Change</button>`
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return '<button onclick="setTableModal(' + row.id + ')" type="button" class="btn btn-primary my-2" data-bs-toggle="modal" data-bs-target="#TableModal">Change Tables</button>'
                        + '<button onclick="setEditModal(' + row.id + ')"  type="button" class="btn btn-primary my-2" data-bs-toggle="modal" data-bs-target="#EditModal">Edit</button>'
                        + '<button onclick="setId(' + row.id + ')" type="button" class="btn btn-danger my-2" data-bs-toggle="modal" data-bs-target="#DeleteModal">Delete</button>';
                }
            }
        ]
    });

    //click Create Modal btn and get sitting info
    $('#CreateModalBtn').click('shown.bs.modal', async () => {
        const response1 = await fetch(`/api/reservations/get/sitting/${sittingId}`);
        const jsonData1 = await response1.json();
        $('#leftCapacity').html(`${jsonData1.capacity} seats left`)

        $("#rGuests").change(function () {
            var max = jsonData1.capacity;
            var min = 1;
            if ($(this).val() > max) {
                $(this).val(max);
            }
            else if ($(this).val() < min) {
                $(this).val(min);
            }
        });

        $("#rGuests").attr({
            "max": jsonData1.capacity,
            "min": 1
        })

        $('#rStartTime').attr({
            "max": jsonData1.end,
            "min": jsonData1.start
        });

        $('#rSittingTime').html(`${jsonData1.start.slice(0,10)} ${jsonData1.start.slice(11, 16)} ` +
            `~ ${jsonData1.end.slice(0, 10)} ${jsonData1.end.slice(11, 16) }`)
        const response2 = await fetch(`/api/reservations/get/source`);
        const jsonData2 = await response2.json();
        $('#rSourceName').empty();
        for (let i = 0; i < jsonData2.length; i++) {
            $('#rSourceName').append(`<option value="${jsonData2[i].name}" >${jsonData2[i].name}</option>`)
        }
    })

    //When submit create form
    $('#CreateForm').on("submit", function (e) {
        e.preventDefault();

        const CreateReservationVM = {
            Name: $("#rName").val(),
            Phone: $('#rPhone').val(),
            Email: $("#rEmail").val(),
            Duration: $("#rDuration").val(),
            Guests: $("#rGuests").val(),
            StartTime: $("#rStartTime").val(),
            SourceName: $("#rSourceName").val(),
            Notes: $("#rNotes").val(),
        };
        fetch(`/api/reservations/${sittingId}/create`, {
            method: 'POST',
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify(CreateReservationVM)
        })
            .then(response => {
            if (response.ok) {
                location.reload();
            }
        })
    });

    //Click DeleteAll btn
    $('#DeleteAllBtn').click((e) => {
        fetch(`/api/reservations/delete/all/${sittingId}`)
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                e.preventDefault();
            }
        })
    });

    //Click row
    function format(d){
        return (
            '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
            '<tr>' +
            '<td>Table:</td>' +
            '<td id="tables">' +
            d.tables +
            '</td >' +
            '</tr>' +
            '<tr>' +
            '<td>Notes:</td>' +
            '<td>' +
            d.notes +
            '</td >' +
            '</tr>' +
            '<tr>' +
            '<td>Phone:</td>' +
            '<td>' +
            d.phone +
            '</td >' +
            '</tr>' +
            '<tr>' +
            '<td>Email:</td>' +
            '<td>' +
            d.email +
            '</td >' +
            '</tr>' +
            '<tr>' +
            '<td>Source:</td>' +
            '<td>' +
            d.source +
            '</td >' +
            '<tr>'+
            '</table>'
        );
    }

    $('#myTable tbody').on('click', 'td.dt-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });

    $('#BackBtn').click(()=>{
        window.location.href = "/Staff/Calendar/Index";
    });

});