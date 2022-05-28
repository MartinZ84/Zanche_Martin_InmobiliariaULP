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
    public class InmueblesController : Controller
    {
      RepositorioInmueble repositorio;
      RepositorioPropietario repoPropietario;


      public InmueblesController(IConfiguration config)
      {
        
        repositorio =new RepositorioInmueble(config);
        repoPropietario= new RepositorioPropietario(config);
      }
        // GET: Inmuebles
         [Authorize(Policy = "Empleado")]
        public ActionResult Index()
        {
             var lista= repositorio.ObtenerTodos();    
            return View(lista);
        }
         [Authorize(Policy = "Empleado")]
          public ActionResult InmDisp()
        {
             var lista= repositorio.ObtenerTodosDisponibles();    
            return View(lista);
        }
         [Authorize(Policy = "Empleado")]
         public ActionResult InmNoDisp()
        {
             var lista= repositorio.ObtenerTodosNoDisponibles();    
            return View(lista);
        }

        // GET: Inmuebles/Details/5
         [Authorize(Policy = "Empleado")]
        public ActionResult Details(int id)
        {
          var inmueble = repositorio.ObtenerPorId(id);
          return View(inmueble);
        }

        // GET: Inmuebles/Create
         [Authorize(Policy = "Empleado")]
        public ActionResult Create()
        {
          ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize(Policy = "Empleado")]
        public ActionResult Create(Inmueble inmueble)
        {
         try
         {
          if (ModelState.IsValid)
                  {
                      repositorio.Alta(inmueble);
                      TempData["Id"] = inmueble.Id;
                      return RedirectToAction(nameof(Index));
                  }
                  else
                  {
                      ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                      return View(inmueble);
                  }
              }
              catch (Exception ex)
              {
                  ViewBag.Error = ex.Message;
                  ViewBag.StackTrate = ex.StackTrace;
                  return View(inmueble);
              }
        }

        // GET: Inmuebles/Edit/5
         [Authorize(Policy = "Empleado")]
        public ActionResult Edit(int id)
        {
            var inmueble = repositorio.ObtenerPorId(id);
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(inmueble);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize(Policy = "Empleado")]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
          try
            {
                inmueble.Id = id;
                repositorio.Modificacion(inmueble);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(inmueble);
            }
        }

        // GET: Inmuebles/Delete/5
         [Authorize(Policy = "Empleado")]
        public ActionResult Delete(int id)
        {
          var inmueble = repositorio.ObtenerPorId(id);
          if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
          return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize(Policy = "Empleado")]
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
               
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
               var inm = repositorio.ObtenerPorId(id);
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(inm);
            }
        }

         [Authorize(Policy = "Empleado")]
        public ActionResult InmProp()
        {
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
           var res= new List<Inmueble>();
            return View(res);
        }
        
        // [HttpPost]
        // [ValidateAntiForgeryToken]
         public ActionResult InmPropGet(int id)
        {
            // var lista = repositorio.ObtenerInmPorPropietario(id);
            // return Json(lista);
           try
            {
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                ViewBag.Id=id;
                var res = repositorio.ObtenerInmPorPropietario(id);
                return View("InmProp",res);
               
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }

       [Authorize(Policy = "Empleado")]
        public ActionResult BuscarInmuebles()
        {
          try
          {
            var res= new List<Inmueble>();
            return View(res);
          }
           catch (Exception ex)
          {
               return Json(new { Error = ex.Message });
            }
          }
          
      [HttpPost]
       [Authorize(Policy = "Empleado")]
        public ActionResult BuscarInmuebles(string Uso, string Tipo,int Ambientes, int Precio, int Superficie, DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
             
              var res = repositorio.BuscarInmuebles(Uso, Tipo, Ambientes, Precio, Superficie, FechaInicio, FechaFin);
              ViewBag.FechaInicio = FechaInicio;
              ViewBag.FechaFin = FechaFin;
              return View("BuscarInmuebles", res);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
          }
         
        }


      // [Route("[controller]/GetInmueblesByPropietario/{q}", Name = "GetInmueblesByPropietario")]
      //   public ActionResult GetInmueblesByPropietario(int id)
      //   {
      //       // var lista = repositorio.ObtenerInmPorPropietario(id);
      //       // return Json(lista);
      //      try
      //       {
      //           var res = repositorio.ObtenerInmPorPropietario(id);
      //           return Json(new { Datos = res });
      //       }
      //       catch (Exception ex)
      //       {
      //           return Json(new { Error = ex.Message });
      //       }
      //   }
    }
