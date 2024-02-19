create database DBVENTA

go

use DBVENTA

go

create table Menu(
idMenu int primary key identity(1,1),
descripcion varchar(30),
idMenuPadre int references Menu(idMenu),
icono varchar(30),
controlador varchar(30),
paginaAccion varchar(30),
esActivo bit,
fechaRegistro datetime default getdate()
)

go

create table Rol(
idRol int primary key identity(1,1),
descripcion varchar(30),
esActivo bit,
fechaRegistro datetime default getdate()
)

go
 
 create table RolMenu(
 idRolMenu int primary key identity(1,1),
 idRol int references Rol(idRol),
 idMenu int references Menu(idMenu),
 esActivo bit,
 fechaRegistro datetime default getdate()
 )

go

create table Usuario(
idUsuario int primary key identity(1,1),
nombre varchar(50),
correo varchar(50),
telefono varchar(50),
idRol int references Rol(idRol),
urlFoto varchar(500),
nombreFoto varchar(100),
clave varchar(100),
esActivo bit,
fechaRegistro datetime default getdate()
)

go

create table Categoria(
idCategoria int primary key identity(1,1),
descripcion varchar(50),
esActivo bit,
fechaRegistro datetime default getdate()
)

go

create table Producto(
idProducto int primary key identity(1,1),
codigoBarra varchar(50),
marca varchar(50),
nombre varchar(100),
descripcion varchar(100),
idCategoria int references Categoria(idCategoria),
stock int,
urlImagen varchar(500),
nombreImagen varchar(100),
precio decimal(10,2),
esActivo bit,
fechaRegistro datetime default getdate()
)

go


go

create table TipoDocumentoVenta(
idTipoDocumentoVenta int primary key identity(1,1),
descripcion varchar(50),
esActivo bit,
fechaRegistro datetime default getdate()
)

go

create table Venta(
idVenta int primary key identity(1,1),
numeroVenta varchar(6),
idTipoDocumentoVenta int references TipoDocumentoVenta(idTipoDocumentoVenta),
idUsuario int references Usuario(idUsuario),
documentoCliente varchar(10),
nombreCliente varchar(20),
subTotal decimal(10,2),
impuestoTotal decimal(10,2),
Total decimal(10,2),
fechaRegistro datetime default getdate()
)

go

create table DetalleVenta(
idDetalleVenta int primary key identity(1,1),
idVenta int references Venta(idVenta),
idProducto int,
marcaProducto varchar(100),
nombreProducto varchar(100),
descripcionProducto varchar(100),
categoriaProducto varchar(100),
cantidad int,
precio decimal(10,2),
total decimal(10,2)
)



go

create table Negocio(
idNegocio int primary key,
urlLogo varchar(500),
nombreLogo varchar(100),
numeroDocumento varchar(50),
nombre varchar(50),
correo varchar(50),
direccion varchar(50),
telefono varchar(50),
porcentajeImpuesto decimal(10,2),
simboloMoneda varchar(5)
)

go

create table Configuracion(
recurso varchar(50),
propiedad varchar(50),
valor varchar(60)
)

go
 

Insert into Rol values ('Administrador',1,GETDATE())
Insert into Rol values ('Empleado',1,GETDATE())
Insert into Rol values ('Usuario',1,GETDATE())

insert into Usuario(nombre,correo,telefono,idRol,urlFoto,nombreFoto,clave,esActivo) values
('Admin','admin@gmail.com','909090',1,'','','a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3',1)

insert into TipoDocumentoVenta(descripcion,esActivo) values
('Boleta',1),
('Factura',1)