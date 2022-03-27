using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zanche_Martin_InmobiliariaULP.Models;

namespace Zanche_Martin_InmobiliariaULP.Controllers
{
    public class PropietariosController : Controller
    {
      RepositorioPropietario repositorio;
      public PropietariosController()
      {
        repositorio = new RepositorioPropietario();
      }
        // GET: Propietarios
        public ActionResult Index()
        {
          var lista= repositorio.ObtenerTodos();       
          return View(lista);
        }

        // GET: Propietarios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Propietarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
              int res= repositorio.Alta(p);
              if (res>0) {
              return RedirectToAction(nameof(Index));
              }
              else
               return View();
            
             
            }
            catch (Exception e)
            {
              Console.WriteLine(e);
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {
          try
          {
            var prop = repositorio.ObtenerPorId(id);
            return View(prop);
          //pasa el modelo a la vista
            
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
            throw;
          }
           
            
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario p)
        {
          Propietario ? propEdit= null;
            try
            {
               
                propEdit = repositorio.ObtenerPorId(id);
                propEdit.Nombre=p.Nombre;
                propEdit.Apellido=p.Apellido;
                propEdit.Dni=p.Dni;
                propEdit.Email=p.Email;
                repositorio.Modificacion(p);
                TempData["Mensaje"] = "Datos guardados correctamente";

                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
              Console.WriteLine(e);
                return View();
            }
        }

        // GET: Propietarios/Delete/5
        public ActionResult Delete(int id)
        {
          try
          {
             var prop = repositorio.ObtenerPorId(id);
            return View(prop);            
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
            throw;
          }
            
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario propietario)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
              Console.WriteLine(e);
              throw;
                // return View();
            }
        }
    }
}