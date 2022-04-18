using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zanche_Martin_InmobiliariaULP.Models;

namespace Zanche_Martin_InmobiliariaULP.Controllers
{
    public class PagosController : Controller
    {
      RepositorioPago repositorio;
      RepositorioContrato repoContrato;
       RepositorioInmueble repoInmueble;
      RepositorioInquilino repoInquilino;
      public PagosController (IConfiguration config)
      {
        
        repositorio =new RepositorioPago(config);
        repoContrato= new RepositorioContrato(config);
        repoInmueble= new RepositorioInmueble(config);
        repoInquilino= new RepositorioInquilino(config);
      }
        // GET: Pagos
        [Authorize(Policy = "Empleado")]
        public ActionResult Index(int id)
        {
           var pagos= repositorio.ObtenerPagosPorContrato(id);
           ViewBag.ContratoId = id;
            return View(pagos);
        }

        // GET: Pagos/Details/5
        [Authorize(Policy = "Empleado")]
        public ActionResult Details(int id)
        {
            var pago = repositorio.ObtenerPorId(id);
            ViewBag.Contrato=repoContrato.ObtenerPorId(pago.ContratoId);
             ViewBag.ContratoId = pago.ContratoId;
            // ViewBag.Inquilinos = repoInquilino.ObtenerTodos();ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(pago);
        }

        // GET: Pagos/Create
         [Authorize(Policy = "Empleado")]
        public ActionResult Create(int id)
        {
          String fechaActual=DateTime.Now.ToString("dd/MM/yyyy");
          ViewBag.ContratoId = id;
          ViewBag.nroPago= repositorio.ObtenerCantidadPagos(id);
          var contrato=repoContrato.ObtenerPorId(id);
          ViewBag.importe= contrato.Precio;
           return View();
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize(Policy = "Empleado")]
        public ActionResult Create(Pago pago)
        {

          pago.ContratoId=pago.Id;
          pago.Id=0;
            try
            {
                 if (ModelState.IsValid)
                  {
                      repositorio.Alta(pago);
                      TempData["Id"] = pago.Id;
                // int contratoId= pago.ContratoId;
                     
                      return RedirectToAction("Index", new { id = pago.ContratoId });
                  }
                  else
                  {
                      ViewBag.Contrato = repoContrato.ObtenerPorId(pago.ContratoId);
                   
                      return View(pago);
                  }
            }
              catch (Exception ex)
              {
                  ViewBag.Error = ex.Message;
                  ViewBag.StackTrate = ex.StackTrace;
                  return View(pago);
              }
        }

        // GET: Pagos/Edit/5
         [Authorize(Policy = "Empleado")]
        public ActionResult Edit(int id)
        {
          var pago = repositorio.ObtenerPorId(id);
          ViewBag.ContratoId = pago.ContratoId;
            return View(pago);
        }

        // POST: Pagos/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize(Policy = "Empleado")]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {
               repositorio.Modificacion(pago);
               TempData["Mensaje"] = "Datos guardados correctamente";
               return RedirectToAction
                ("Index", new { id = pago.ContratoId });
            }
               catch(Exception ex)
            {
                var pay = repositorio.ObtenerPorId(id);
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pay);
            }
        }

        // GET: Pagos/Delete/5
         [Authorize(Policy = "Empleado")]
        public ActionResult Delete(int id)
        {
              var pago = repositorio.ObtenerPorId(id);
          if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
          return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            {
              pago= repositorio.ObtenerPorId(id);
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction
                ("Index", new { id = pago.ContratoId });
            }
              catch (Exception ex)
            {
              var pay = repositorio.ObtenerPorId(id);
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pay);
            }
        }
    }
}