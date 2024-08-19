const MODELO_BASE = {
    iD: 0,
    numero: 0,
    estado: 0,
}
const MODELO_BASE1 = {
    id: 0,
    nombre: "",
    identificacion: "",
    numero: 0,
    ingreso: null,
    salida: null,
    estado:0,
}

let tablaData;

function habitaciones_dis() {
    fetch("/Hotel/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            console.log(responseJson)
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((item) => {
                    $("#cboProceso").append(
                        $("<option>").val(item.id).text(item.numero)
                    )
                })
            }
        })
    $(".js-example-basic-single").select2({
        dropdownParent: $('#modalData')
    });
}
$(document).ready(function () {
    debugger;
    fetch("/Hotel/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            console.log(responseJson)
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((item) => {
                    $("#cboProceso").append(
                        $("<option>").val(item.id).text(item.numero)
                    )
                })
            }
        })
    $(".js-example-basic-single").select2({
        dropdownParent: $('#modalData')
    });
    debugger;
    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Hotel/ListaDisponibles',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "searchable": false },
            { "data": "nombre" },
            { "data": "identificacion" },
            { "data": "numero" },
            { "data": "ingreso" },
            { "data": "salida" },
            {
                "data": "estado", render: function (data) {
                    if (data == 0)
                        return '<span class="badge badge-info">Activo</span>';
                    if (data == 2)
                        return '<span class="badge badge-info">Completado</span>';
                    else
                        return '<span class="badge badge-danger">Cancelada</span>';
                }
            },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
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




function mostrarModal(modelo = MODELO_BASE1) {
    
    $("#txtId").val(modelo.id)
    
    if (modelo.id == 0) {
        $("#cboProceso").val(modelo.numero == 0 ? $("#cboProceso option:first").val() : modelo.numero).trigger('change.select2')
        $("#txtFechafin").prop('disabled', true);
        $("#txtFechaini").prop('disabled', false);
        $("#txtnombre").prop('disabled', false);
        $("#txtcedula").prop('disabled', false);
        $("#cboProceso").removeAttr('disabled')
    } else {
        $("#cboProceso").attr('disabled', 'disabled')
        $("#cboProceso").val(modelo.numero == 0 ? $("#cboProceso option:first").val() : modelo.numero).trigger('change.select2')
        $("#txtFechaini").prop('disabled', true);
        $("#txtnombre").prop('disabled', true);
        $("#txtcedula").prop('disabled', true);
        $("#txtFechafin").prop('disabled', false);
        $("#txtFechaini").val(modelo.ingreso);
        $("#txtnombre").val(modelo.nombre);
        $("#txtcedula").val(modelo.identificacion);
    }
  
    $("#modalData").modal("show")
}


$("#btnNuevo").click(function () {
        
    //esta linea es para poer escribir dentro del modal con el cuadro de busqueda
    $(".js-example-basic-single").select2({
        dropdownParent: $('#modalData')
    });
    //filtrarDoc(resu);
 
    mostrarModal()
})

//al momento de cerrar un modal ejecutar la carga de cargar documentos



document.addEventListener('DOMContentLoaded', (event) => {
    var today = new Date().toISOString().split('T')[0];
    document.getElementById("txtFechaini").setAttribute('min', today);
    document.getElementById("txtFechafin").setAttribute('min', today);
    // Obtener los valores de las fechas
    var fechaIni = document.getElementById("txtFechaini").value;
    var fechaFin = document.getElementById("txtFechafin").value;

    // Convertir las fechas a objetos Date para la comparación
    var inicio = new Date(fechaIni);
    var fin = new Date(fechaFin);

  
});



$("#btnGuardar").click(function () {
    
    if ($("#txtcedula").val().trim() == "") {
        toastr.warning("", "Debe completa el campo : cedula")
        $("#txtcedula").focus()
        return;
    }
    if ($("#txtnombre").val().trim() == "") {
        toastr.warning("", "Debe completa el campo : nombre")
        $("#txtnombre").focus()
        return;
    }
    
    const modelo = structuredClone(MODELO_BASE1);
    modelo["id"] = parseInt($("#txtId").val())
    var selectedText = $("#cboProceso option:selected").val();
    modelo["nombre"] = $("#txtnombre").val()
    modelo["identificacion"] = $("#txtcedula").val()
    modelo["numero"] = $("#cboProceso option:selected").val();
    modelo["ingreso"] = $("#txtFechaini").val()
    //modelo["salida"] = $("#txtFechaini").val()
    if (modelo.id == 0) {
        
        $("#modalData").find("div.modal-content").LoadingOverlay("show");
        fetch("/Hotel/Crear", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {

                    tablaData.row.add(responseJson.objeto).draw(false)
                    $("#modalData").modal("hide")
                    swal("Listo!", "El documento fue creado", "success")
                } else {
                    swal("Los sentimos", responseJson.mensaje, "error")
                }
            })
    } else {
        
        var selectedText = $("#cboProceso option:selected").val();
        modelo["id"] = habitacion.id;
        modelo["nombre"] = habitacion.nombre;
        modelo["identificacion"] = habitacion.identificacion;
        modelo["numero"] = habitacion.numero;
        modelo["ingreso"] = habitacion.ingreso;
        modelo["salida"] = $("#txtFechafin").val();
        debugger;
        fetch("/Hotel/Editar", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {

                    tablaData.row(filaSeleccionada).data(responseJson.objeto).draw(false);
                    filaSeleccionada = null;
                    $("#modalData").modal("hide")
                    swal("Listo!", "El documento fue modificada", "success")
                } else {
                    swal("Los sentimos", responseJson.mensaje, "error")
                }
            })
    }
    habitaciones_dis()
})


let filaSeleccionada;
var habitacion;
$("#tbdata tbody").on("click", ".btn-editar", function () {
    debugger
    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }
    const data = tablaData.row(filaSeleccionada).data();
    habitacion = data;
    mostrarModal(data);

})


$("#tbdata tbody").on("click", ".btn-eliminar", function () {

    let fila;
    if ($(this).closest("tr").hasClass("child")) {
        fila = $(this).closest("tr").prev();
    } else {
        fila = $(this).closest("tr");
    }

    const data = tablaData.row(fila).data();

    swal({
        title: "¿Está seguro?",
        text: `Cancelar reserva "${data.nombre}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, cancelar",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (respuesta) {
            debugger;
            if (respuesta) {

                $(".showSweetAlert").LoadingOverlay("show");

                fetch(`/Hotel/Eliminar?idreserva=${data.id}&idnumero=${data.numero}`, {
                    method: "DELETE"
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {

                        if (responseJson.estado) {

                            tablaData.row(fila).remove().draw()

                            swal("Listo!", "El documento fue eliminada", "success")
                            habitaciones_dis()
                        } else {
                            swal("Los sentimos", responseJson.mensaje, "error")
                        }
                    })


            }
        }
    )


})