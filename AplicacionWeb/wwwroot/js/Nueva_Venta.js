//usa Select2 para hacer la barra de busqueda
$("#cboBuscarProducto").select2({
    ajax: {
        url: "/Venta/ObtenerProductos",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        delay: 250,
        data: function (params) {
            return {
                busqueda: params.term
            };
        },
        processResults: function (data) {

            return {
                results: data.map((item) => (
                    {
                        id: item.idProducto,
                        text: item.nombre,
                        descripcion: item.descripcion,
                        marca: item.marca,
                        categoria: item.nombreCategoria, //NO FUNCIONA
                        urlImagen: item.urlImagen,
                        precio: parseFloat(item.precio)
                    }
                ))
            };
        }
    },
    language: "es",
    placeholder: 'Buscar Producto...',
    minimumInputLength: 1,
    templateResult: formatoResultado
});

//Esto es lo que devuelve como resultado el SELECT
function formatoResultado(data) {
    if (data.loading)
        return data.text;

    var contenedor = $(
        `<table width="100%">
        <tr class="rowSelect">
            <td style="width:60px">
                <img style="height:60px;width:60px;margin-right:10px" src="${data.urlImagen}"/>
            </td>
            <td>
                <p style="font-weight: bolder;margin:2px">${data.marca}</p>
                <p style="margin:2px">${data.text}</p>
                <p style="margin:2px">${data.descripcion}</p>
            </td>
        </tr>
        </table>`
    );
    return contenedor;
}


//MODAL CANTIDAD
$("#cboBuscarProducto").on("select2:select", function (e) {
    cargarModalCantidad();
});

function cargarModalCantidad() { //el input por defecto viene con 1
    var htmlModal = `
      <div class="modal" id="modalCantidad">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-body">
              <label for="inputCantidad">Introduce la cantidad:</label>
              <input type="number" id="inputCantidad" class="form-control" value="1" min="1"/>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-primary" id="btnGuardarCantidad">Guardar</button>
              <button type="button" class="btn btn-secondary" data-dismiss="modal" id="btnCancelar">Cancelar</button>
            </div>
          </div>
        </div>
      </div>
    `;

    // Pegar el HTML del modal en el contenedor
    $('#modalContainer').html(htmlModal);

    // Mostrar el modal
    $('#modalCantidad').modal('show');
}


let ProductosVenta = [];

//BOTON GUARDAR CANTIDAD
$(document).on('click', '#btnGuardarCantidad', function () {
    var data = $("#cboBuscarProducto").select2('data')[0];

    var cantidadIngresada = $('#inputCantidad').val();
    if (isNaN(parseInt(cantidadIngresada))) {
        alert("Debe ingresar un valor numerico");
        return false;
    }

    //crea el producto
    let producto = {
        idProducto: data.id,
        marca: data.marca,
        nombreProducto: data.text,
        descripcion: data.descripcion,
        categoriaProducto: data.categoria,
        cantidad: parseInt(cantidadIngresada),
        precio: data.precio.toString(),
        total: (parseFloat(cantidadIngresada) * data.precio).toString()
    }
    ProductosVenta.push(producto) //agrega el producto al vector ProductosVenta
    mostrarProductoYPrecios();

    // Cierra el modal
    $('#modalCantidad').modal('hide');
});

$(document).on('click', '#btnCancelar', function () {
    $('#modalCantidad').modal('hide');
});



//MOSTRAR PRODUCTOS Y DETALLE PRECIOS
let ValorImpuesto = 21;
function mostrarProductoYPrecios() {
    let total = 0;
    let igv = 0;
    let subtotal = 0;
    let procentaje = ValorImpuesto / 100;
    $("#tbProducto tbody").html("")


    ProductosVenta.forEach((item) => {
        total = total + parseFloat(item.total)
        $("#tbProducto tbody").append(
            $("<tr>").append(
                //boton de eliminar tiene una imagen y un data que pasa el ID
                $("<td>").append($("<button>").addClass("btn btn-danger btn-eliminar btn-sm").data("idProducto", item.idProducto).append($("<img>").attr("src", "https://res.cloudinary.com/dmoinc30z/image/upload/v1706333652/i6i9vbjgn25v4nwnxmmg.png").css({ width: "15px", height: "20px" }))),
                $("<td>").text(item.nombreProducto),
                $("<td>").text(item.cantidad),
                $("<td>").text(item.precio),
                $("<td>").text(item.total)
            )
        )
    })
    subtotal = total / (1 + procentaje);
    igv = total - subtotal;

    $("#txtSubTotal").val(subtotal.toFixed(2)) //esto es para fijarlo a 2 decimales
    $("#txtIGV").val(igv.toFixed(2))
    $("#txtTotal").val(total.toFixed(2))
}


$(document).on('click', 'button.btn-eliminar', function () {
    const _idproducto = $(this).data("idProducto");
    //Igualamos el vector a todos los productos que NO coincidan con el id del producto a eliminar. Para asi hacer un vector sin ese producto a eliminar
    ProductosVenta = ProductosVenta.filter(p => p.idProducto != _idproducto); 

    mostrarProductoYPrecios();
});



$("#btnTerminarVenta").click(function () {

    if (ProductosVenta.length > 0) {

        const vmDetalleVenta = ProductosVenta;
        const venta = {
            IdTipoDocumentoVenta: $("#cboTipoDocumentoVenta").val(),
            DocumentoCliente: $("#txtDocumentoCliente").val(),
            NombreCliente: $("#txtNombreCliente").val(),
            SubTotal: $("#txtSubTotal").val(),
            ImpuestoTotal: $("#txtIGV").val(),
            Total: $("#txtTotal").val(),
            DetalleVenta: vmDetalleVenta
        }
        

        $.ajax({
            url: '/Venta/RegistrarVenta', 
            type: 'POST',
            contentType: 'application/json', 
            data: JSON.stringify(venta), 
            success: function (response) {
                
                //muestra modal con la imagen Exito
                cargarModalDetalleVenta("https://res.cloudinary.com/dmoinc30z/image/upload/v1706496703/xxbf4cgxf4tllckoapmy.png", response.numeroVenta);
                ProductosVenta = []; //vacia el vector
                mostrarProductoYPrecios(); //vacia la lista en la vista
                
            },
            error: function (xhr, status, error) {
                //muestra modal con la imagen Error
                cargarModalDetalleVenta("https://res.cloudinary.com/dmoinc30z/image/upload/v1706498394/qojexiybiiym6kyjop4n.png");
            }
        });


    } else {
        alert("Agregue productos para la venta");
        return false;
    }
})


/* CON FETCH

fetch("/Venta/RegistrarVenta", {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify(venta)
        })
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.estado) {
                ProductosVenta = []; //vacia el vector
                mostrarProductoYPrecios(); //vacia la lista en la vista
            }
        })


    } else {
        alert("Agregue productos para la venta");
        return false;
    }


*/




//MODAL REGISTRAR VENTA. MUESTRA EXITO O ERROR
function cargarModalDetalleVenta(link, numVenta) {

    if (numVenta == undefined) numVenta = "-";

    var htmlModal = `
      <div class="modal fade" id="modalRegistrarVenta" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog  modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-body text-center">
            <img src="${link}" class="img-fluid mx-auto d-block" style="max-width: 50%; alt="Imagen">
             <br/>
            <h1>Numero venta: ${numVenta}</h1>
            <br/>
            <button type="button" class="btn btn-primary mt-3 mx-auto" data-dismiss="modal" id="btnAceptar">Aceptar</button>
          </div>
        </div>
      </div>
    </div>
    `;

    // Pegar el HTML del modal en el contenedor
    $('#modalContainer').html(htmlModal);

    // Mostrar el modal
    $('#modalRegistrarVenta').modal('show');
}

//boton Aceptar modal
$(document).on('click', '#btnAceptar', function () {
    $('#modalRegistrarVenta').modal('hide');
});