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
    public class ContratosController : Controller
    {
      RepositorioContrato repositorio;
      RepositorioInmueble repoInmueble;
      RepositorioInquilino repoInquilino;
      RepositorioPropietario repoPropietario;

 public ContratosController (IConfiguration config)
      {
        
        repositorio =new RepositorioContrato(config);
        repoInmueble= new RepositorioInmueble(config);
        repoInquilino= new RepositorioInquilino(config);
        repoPropietario= new RepositorioPropietario(config);
      }
        // GET: Contratos
         [Authorize(Policy = "Empleado")]
        public ActionResult Index()
        {
          var lista=repositorio.ObtenerTodos();
          ViewBag.Cantidad= lista.Count();
          ViewBag.Contratos=repositorio.ObtenerTodos();
           ViewBag.Inquilinos=repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles=repoInmueble.ObtenerTodos();
            return View();
        }

        // GET: Contratos/Details/5
         [Authorize(Policy = "Empleado")]
        public ActionResult Details(int id)
        {
          
            var contrato = repositorio.ObtenerPorId(id);
            ViewBag.Inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);
            ViewBag.Inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);
            ViewBag.Propietario=repoPropietario.ObtenerPorId(ViewBag.Inmueble.PropietarioId);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(contrato);
      
         
        }

        // GET: Contratos/Create
         [Authorize(Policy = "Empleado")]
        public ActionResult Create()
        {
          TempData.Remove("returnUrl");
          var returnUrl = "/Contratos";
            ViewBag.Inquilino=repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles=repoInmueble.ObtenerTodosDisponibles();
            TempData["returnUrl"] = returnUrl;
            return View();
        }

           [Authorize(Policy = "Empleado")]
        public ActionResult CreateByInmId(int id)
        {
          TempData.Remove("returnUrl");
           var returnUrl = "/Contratos/ContratosInmueble/"+id ; 
      
            ViewBag.Inquilino=repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles=repoInmueble.ObtenerPorId(id);
           TempData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Contratos/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize(Policy = "Empleado")]
        public ActionResult Create(Contrato contrato)
        {
           
           var urlOrigen="";
          if(TempData.ContainsKey("returnUrl")){
                 urlOrigen=TempData["returnUrl"].ToString();}
          else
          {
              urlOrigen = "/Contratos/ContratosInmueble/"+contrato.InmuebleId ; 
              TempData["returnUrl"] = urlOrigen;      
          }
            try
            {               
                 if (ModelState.IsValid)
                  {
                      var res=repoInmueble.BuscarDisponibilidad(contrato.InmuebleId,contrato.FechaInicio,contrato.FechaFin);
                      if(res >0){
                        TempData["Error"] = "No hay disponibilidad para el periodo seleccionado";
                        ViewBag.Inquilino=repoInquilino.ObtenerTodos();
                        ViewBag.Inmuebles=repoInmueble.ObtenerTodos();
                        ViewBag.Inmueble=repoInmueble.ObtenerPorId(contrato.InmuebleId);
                        ViewBag.Contrato=contrato;
                        TempData["returnUrl"] = urlOrigen;        
                        return View(contrato);
                      } else
                      {
                       repositorio.Alta(contrato);
                      TempData["Id"] = contrato.Id;

                      }
                                          
                      // return RedirectToAction(nameof(Index));
                      if(TempData.ContainsKey("returnUrl")){
                        urlOrigen = "/Contratos/ContratosInmueble/"+contrato.InmuebleId ; 
                        return Redirect(urlOrigen);
                      }
                      else{
                        return RedirectToAction(nameof(Index));
                      }
                       
                      //return View();
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
         [Authorize(Policy = "Empleado")]
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
         [Authorize(Policy = "Empleado")]
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
         [Authorize(Policy = "Empleado")]
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
         [Authorize(Policy = "Empleado")]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
               
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminaci??n realizada correctamente";
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

       // GET: Contratos/Renovar/5
         [Authorize(Policy = "Empleado")]
        public ActionResult Renovar(int id)
        {
           TempData.Remove("returnUrl");
          
            var contrato = repositorio.ObtenerPorId(id);
            contrato.FechaInicio=contrato.FechaInicio.Date.AddDays(2);
            contrato.FechaFin=contrato.FechaInicio.AddYears(2);
              
            ViewBag.Inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);
            ViewBag.Inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);
            ViewBag.Propietario=repoPropietario.ObtenerPorId(ViewBag.Inmueble.PropietarioId);
             var returnUrl = "/Contratos/ContratosInmueble/"+contrato.InmuebleId; 
         
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            TempData["returnUrl"] = returnUrl;
            return View(contrato);
        }

        public ActionResult ContratosInmueble(int id)
        {
            var contrato = repositorio.ObtenerAllContratosDeInmueble(id);
            var inmuebleSolicitado=repoInmueble.ObtenerPorId(id);
            ViewBag.inmuebleCod=inmuebleSolicitado.Id;
            ViewBag.InmuebleDireccion=inmuebleSolicitado.Direccion;           
            // ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.Inmueble = inmuebleSolicitado;
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(contrato);
        }

        // GET: Contratos
         [Authorize(Policy = "Empleado")]
        public ActionResult ContratosVigentes()
        {
          var lista=repositorio.ObtenerTodosVigentes();
           ViewBag.Cantidad= lista.Count();
           ViewBag.Inquilinos=repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles=repoInmueble.ObtenerTodos();
            return View(lista);
        }

          [Authorize(Policy = "Empleado")]
        public ActionResult ContratosNoVigentes()
        {
          var lista=repositorio.ObtenerTodosNoVigentes();
           ViewBag.Cantidad= lista.Count();
           ViewBag.Inquilinos=repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles=repoInmueble.ObtenerTodos();
            return View(lista);
        }
      }
    }
