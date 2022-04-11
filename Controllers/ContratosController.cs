using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zanche_Martin_InmobiliariaULP.Models;

namespace Zanche_Martin_InmobiliariaULP.Controllers
{
    public class ContratosController : Controller
    {
      RepositorioContrato repositorio;
      RepositorioInmueble repoInmueble;
      RepositorioInquilino repoInquilino;

 public ContratosController (IConfiguration config)
      {
        
        repositorio =new RepositorioContrato(config);
        repoInmueble= new RepositorioInmueble(config);
        repoInquilino= new RepositorioInquilino(config);
      }
        // GET: Contratos
        public ActionResult Index()
        {
          var lista=repositorio.ObtenerTodos();
          ViewBag.Contratos=repositorio.ObtenerTodos();
           ViewBag.Inquilinos=repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles=repoInmueble.ObtenerTodos();
            return View();
        }

        // GET: Contratos/Details/5
        public ActionResult Details(int id)
        {
          
              var contrato = repositorio.ObtenerPorId(id);
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(contrato);
      
         
        }

        // GET: Contratos/Create
        public ActionResult Create()
        {
            ViewBag.Inquilino=repoInquilino.ObtenerTodos();
            ViewBag.Inmueble=repoInmueble.ObtenerTodos();
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                 if (ModelState.IsValid)
                  {
                      repositorio.Alta(contrato);
                      TempData["Id"] = contrato.Id;
                      return RedirectToAction(nameof(Index));
                  }
                  else
                  {
                      ViewBag.Inmueble = repoInmueble.ObtenerTodos();
                      ViewBag.Inquilino = repoInquilino.ObtenerTodos();
                      return View(contrato);
                  }
            }
              catch (Exception ex)
              {
                  ViewBag.Error = ex.Message;
                  ViewBag.StackTrate = ex.StackTrace;
                  return View(contrato);
              }
        }

        // GET: Contratos/Edit/5
        public ActionResult Edit(int id)
        {
              var contrato = repositorio.ObtenerPorId(id);
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                contrato.Id = id;
                repositorio.Modificacion(contrato);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
               
            }
            catch(Exception ex)
            {
                ViewBag.Inmueble = repoInmueble.ObtenerTodos();
                ViewBag.Inquilino = repoInquilino.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(contrato);
            }
        }

        // GET: Contratos/Delete/5
        public ActionResult Delete(int id)
        {
            var contrato = repositorio.ObtenerPorId(id);
          if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
          return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
               
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
              var contrat = repositorio.ObtenerPorId(id);
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(contrat);
            }
        }
        }
    }
