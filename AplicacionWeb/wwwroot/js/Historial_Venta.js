
function busquedaFecha() { //cuando selecciona fecha muestra los datetimepickers y oculta el num venta
    $(".busqueda-fecha").show();
    $(".busqueda-venta").hide();
}

function busquedaVenta() { //lo mismo pero al reves
    $(".busqueda-fecha").hide();
    $(".busqueda-venta").show();
}

$(document).ready(function (){
    busquedaFecha();
})

$("#cboBuscarPor").change(function () {
    $("#txtFechaInicio").val("")
    $("#txtFechaFin").val("")
    $("#txtNumeroVenta").val("")

    if ($("#cboBuscarPor").val() == "fecha") {
        busquedaFecha();
    } else {
        busquedaVenta();
    }
})

$("#btnBuscar").click(function () { 
    if ($("#cboBuscarPor").val() == "fecha") { //si selecciono fecha en el combobox
        if ($("#txtFechaInicio").val().trim() == "" || $("#txtFechaFin").val().trim() == "") { //si no ingreso las fechas en los campos
            alert("Seleccione las fechas para realizar la busqueda")
            return;
        }
    }
    else { //si selecciono num venta en el combobox
        if ($("#txtNumeroVenta").val().trim() == "") { //si no ingreso el num venta en el campo
            alert("Ingrese el numero de venta para buscar") 
            return;
        }
    }

    let numVenta = $("#txtNumeroVenta").val()
    let fechaInicio = $("#txtFechaInicio").val()
    let fechaFin = $("#txtFechaFin").val()

    //ejecuta el metodo Historial del controlador, pasandole los parametros. Para pasarle los parametros hay que usar `` y ${}
    fetch(`/Venta/Historial?numeroVenta=${numVenta}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`) 
        .then(response => { return response.ok ? response.json() : Promise.reject(response); })
        .then(responseJson => {
            $("#tbventa tbody").html(""); //vacia la tabla

            if (responseJson.length > 0) { //si la lista historial que devuelve el metodo del controller no esta vacia

                responseJson.forEach(venta => {
        
                $("#tbventa tbody").append(
                    $("<tr>").append(
                        $("<td>").text(formatearFecha(venta.fechaRegistro)),
                        $("<td>").text(venta.numeroVenta),
                        $("<td>").text(venta.idTipoDocumentoVentaNavigation.descripcion),
                        $("<td>").text(venta.documentoCliente),
                        $("<td>").text(venta.nombreCliente),
                        $("<td>").text(venta.total),
                        $("<td>").append(
                            $("<button>").addClass("btn btn-info btn-sm").data("venta", venta).append($("<img>").attr("src", "https://res.cloudinary.com/dmoinc30z/image/upload/v1707166154/patck3k2a3qsvemb1ewd.png").css({ width: "20px", height: "15px" })
                        )
                    )
                    )
                )
                });
            }
    });

})


//FORMATEAR LA FECHA Y MOSTRARLA MEJOR EN LA TABLA.
//ORIGINALMENTE VIENE ASI LA FECHA "2024-02-04T01:18:00" y con esto lo pasaria a  "2024-02-04 01:18". Es decir  yyyy-MM-dd HH:mm
function formatearFecha(fecha) {
    let fechaFormateada = fecha.replace("T", " ").slice(0, -3); //remplaza la T por un espacio " " y le quita los ultimos tres caracteres que son los de los segundos
    return fechaFormateada;
}


//click boton ver detalle venta
$("#tbventa tbody").on("click",".btn-info", function () {

    cargarModalDetalleVenta();

    let venta = $(this).data("venta");
    console.log(venta);

    $("#txtTotal").val(venta.total)
    $("#txtSubTotal").val(venta.subTotal)
    $("#txtIGV").val(venta.impuestoTotal)

    $("#tbProductos tbody").html("");

    venta.detalleVenta.forEach((item) => {
        $("#tbProductos tbody").append(
            $("<tr>").append(
                $("<td>").text(item.nombreProducto),
                $("<td>").text(item.marcaProducto),
                $("<td>").text(item.cantidad),
                $("<td>").text(item.precio),
                $("<td>").text(item.total)
            )
        )
    })

    

});

//MODAL DETALLE VENTA
function cargarModalDetalleVenta() {

    var htmlModal = `
    <div class="modal" id="modalInfo" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
    <div class="modal-content">
    <div class="modal-header bg-light">
        <h6>Detalle venta</h6>
        <button type="button" class="btn btn-outline-secondary btn-sm" data-dismiss="modal" id="btnCerrar">Cerrar</button>
    </div>
    <div class="modal-body">
    <div class="form-group">
          <label for="txtTotal">Total:</label>
          <input type="text" class="form-control" id="txtTotal" disabled>
        </div>
        <div class="form-group">
          <label for="txtSubTotal">Subtotal:</label>
          <input type="text" class="form-control" id="txtSubTotal" disabled>
        </div>
        <div class="form-group">
          <label for="txtIGV">Impuesto:</label>
          <input type="text" class="form-control" id="txtIGV" disabled>
        </div>
    <br/>
    <table class="table" style="width: 100%" id="tbProductos">
        <thead>
            <tr>
                <th>Producto</th>
                <th>Marca</th>
                <th>Cantidad</th>
                <th>Precio</th>
                <th>Total</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
    </div>
    </div>
    </div>
</div>
    `;

    // Pegar el HTML del modal en el contenedor
    $('#modalContainer').html(htmlModal);

    // Mostrar el modal
    $('#modalInfo').modal('show');
}



$(document).on('click', '#btnCerrar', function () {
    $('#modalInfo').modal('hide');
});

