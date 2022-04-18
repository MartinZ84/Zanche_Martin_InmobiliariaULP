// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//funcion que obtenga la fecha actual y la muestre en un input type date que se ejecute cuando cargue la pagina

function getFechaActual() {
  var fecha = new Date();
  var dia = fecha.getDate();
  var mes = fecha.getMonth() + 1;
  var anio = fecha.getFullYear();
  if (dia < 10) {
    dia = "0" + dia;
  }
  if (mes < 10) {
    mes = "0" + mes;
  }
  var fechaActual = anio + "-" + mes + "-" + dia;
  document.getElementById("fechaInicio").value = fechaActual;
}

// window.onload = function () {
//   var fecha = new Date(); //Fecha actual
//   var mes = fecha.getMonth() + 1; //obteniendo mes
//   var dia = fecha.getDate(); //obteniendo dia
//   var ano = fecha.getFullYear(); //obteniendo año
//   if (dia < 10) dia = "0" + dia; //agrega cero si el menor de 10
//   if (mes < 10) mes = "0" + mes; //agrega cero si el menor de 10
//   document.getElementById("fechaInicio").value = ano + "-" + mes + "-" + dia;

//   //sumar 2 años a la fecha actual
//   var fecha2 = new Date(); //Fecha actual
//   var mes2 = fecha2.getMonth() + 1; //obteniendo mes
//   var dia2 = fecha2.getDate(); //obteniendo dia
//   var ano2 = fecha2.getFullYear() + 2; //obteniendo año
//   if (dia2 < 10) dia2 = "0" + dia2; //agrega cero si el menor de 10
//   if (mes2 < 10) mes2 = "0" + mes2; //agrega cero si el menor de 10
//   document.getElementById("fechaFin").value = ano2 + "-" + mes2 + "-" + dia2;
// };

//Funcion para armar el link para peticion get de inmuebles por propietario
$("#PropietarioId").change(function () {
  var id = $(this).val();
  $("#link").attr("href", "/Inmuebles/InmPropGet/" + id);
});
