using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zanche_Martin_InmobiliariaULi.Models;
using Zanche_Martin_InmobiliariaULP.Models;

namespace Zanche_Martin_InmobiliariaULP.Controllers
{
    public class InquilinosController : Controller
    {
      RepositorioInquilino repositorio;
      public InquilinosController()
      {
        repositorio = new RepositorioInquilino();
      }
        // GET: Inquilinos
        public ActionResult Index()
        {
            var lista= repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilinos/Details/5
        public ActionResult Details(int id)
        {
          var inquilino = repositorio.ObtenerPorId(id);
            return View(inquilino);
        }

        // GET: Inquilinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
        {
            try
          {
              int res= repositorio.Alta(i);
              if (res>0) {
              return RedirectToAction(nameof(Index));
              }
              else
               return View();            
            }
            catch(Exception e)
            {
               Console.WriteLine(e);
                return View();
            }
        }

        // GET: Inquilinos/Edit/5
        public ActionResult Edit(int id)
        {
           try
          {
            var inquilino = repositorio.ObtenerPorId(id);
            return View(inquilino);
          //pasa el modelo a la vista            
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
            throw;
          }
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino i )
        {
            Inquilino ? inquilinoEdit= null;
            try
            {
              inquilinoEdit = repositorio.ObtenerPorId(id);
              inquilinoEdit.Nombre=i.Nombre;
              inquilinoEdit.Apellido=i.Apellido;
              inquilinoEdit.Dni=i.Dni;
              inquilinoEdit.Email=i.Email;
              inquilinoEdit.Lugar_Trabajo=i.Lugar_Trabajo;
              inquilinoEdit.Nombre_Garante=i.Nombre_Garante;
              inquilinoEdit.Apellido_Garante=i.Apellido_Garante;
              inquilinoEdit.Telefono_Garante=i.Telefono_Garante;
              repositorio.Modificacion(i);
              TempData["Mensaje"] = "Datos guardados correctamente";
              return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
              Console.WriteLine(e);
               return View();
            }
        }

        // GET: Inquilinos/Delete/5
        public ActionResult Delete(int id)
        {
           try
           {
            var inquilino = repositorio.ObtenerPorId(id);
            return View(inquilino);
           }
           catch (Exception e)
           {
             Console.WriteLine(e);
             throw;
           }
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino i)
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
              return View();
            }
        }
    }
}