
let tablaData;

$(document).ready(function () {
    $(".js-example-basic-single").select2({
        dropdownParent: $('#modalData')
    });
    debugger
    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Hotel/ListaDisponibles2',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            /*{ "data": "AuditID", "searchable": false },*/
            { "data": "tabla" },
            { "data": "operacion" },
            { "data": "id" },
            { "data": "valorant" },
            { "data": "valornew" },
            { "data": "usuario" },
            { "data": "fecha" }
        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte',
                exportOptions: {
                    columns: [1]
                }
            }, 'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
})

