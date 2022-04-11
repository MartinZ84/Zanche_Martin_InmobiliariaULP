using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Index()
        {
             var lista= repositorio.ObtenerTodos();    
            return View(lista);
        }

        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
          var inmueble = repositorio.ObtenerPorId(id);
          return View(inmueble);
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
          ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}