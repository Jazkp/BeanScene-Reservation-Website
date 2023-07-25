$(() => {
    //make DataTable
    $('#myTable').DataTable({
        ajax: {
            url: `/api/reservations/get`,
            dataSrc: ''
        },
        columns: [
            { data: 'name' },
            { data: 'guests' },
            { data: 'start' },
            {
                data: 'duration',
                render: function (data, type, row) {
                    return data + "min";
                }
            },
            { data: 'status'},
            { data: 'notes' },
            { data: 'source' },
            { data: 'phone' },
            { data: 'email' },
        ]
    });

    $('#BackBtn').click(() => {
        window.location.href = "/Staff/Calendar/Index";
    });
});