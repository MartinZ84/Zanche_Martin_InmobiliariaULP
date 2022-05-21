using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Zanche_Martin_InmobiliariaULP.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zanche_Martin_InmobiliariaULP.Api
{
  [Route("api/[controller]")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class InmueblesController : Controller
  {
    private readonly DataContext contexto;

     private readonly IWebHostEnvironment environment;

    public InmueblesController(DataContext contexto,IWebHostEnvironment environment)
    {
      this.contexto = contexto;
      this.environment = environment;
    }


    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      try
      {
        var usuario = User.Identity.Name;
        return Ok(contexto.Inmuebles.Include(e => e.Propietario).Where(e => e.Propietario.Email == usuario).Select(x => new InmuebleView(x))
);
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }

    // GET api/<controller>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      try
      {
        var usuario = User.Identity.Name;
        return Ok(contexto.Inmuebles.Include(e => e.Propietario).Where(e => e.Propietario.Email == usuario)//.Select(x => new InmuebleView(x)));
       .Single(e => e.Id == id));
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }

    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Inmueble entidad)
    {
      try
      {
        if (ModelState.IsValid)
        {
          entidad.PropietarioId = contexto.Propietarios.Single(e => e.Email == User.Identity.Name).Id;

        // if (entidad.Foto!= null && entidad.Id > 0)
        //         {
        //             string wwwPath = environment.WebRootPath;
        //             string path = Path.Combine(wwwPath, "Uploads/Inmuebles");
        //             if (!Directory.Exists(path))
        //             {
        //                 Directory.CreateDirectory(path);
        //             }
        //             //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
        //             string fileName = "Inmueble_" + entidad.Id + "Propietario_" + entidad.PropietarioId + Path.GetExtension(entidad.FotoFile.FileName);
        //             string pathCompleto = Path.Combine(path, fileName);
        //             entidad.Foto = Path.Combine("/Uploads/Inmuebles", fileName);
        //             // Esta operación guarda la foto en memoria en el ruta que necesitamos
        //             using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
        //             {
        //                 entidad.FotoFile.CopyTo(stream);
        //             }
        //            contexto.Inmuebles.Update(entidad);
				//            await contexto.SaveChangesAsync(); 
        //         }

        //   contexto.Inmuebles.Add(entidad);
          contexto.SaveChanges();
          return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
        }
        return BadRequest();
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }

    // PUT api/<controller>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Inmueble entidad)
    {
      try
      {
        Propietario p=contexto.Propietarios.Single(e => e.Email == User.Identity.Name);
        if (ModelState.IsValid && contexto.Inmuebles.AsNoTracking().Include(e => e.Propietario).FirstOrDefault(e => e.Id == id && p.Email==User.Identity.Name) != null)
        {
          entidad.Id = id;
          entidad.PropietarioId=p.Id;
          contexto.Inmuebles.Update(entidad);
          contexto.SaveChanges();
          return Ok(entidad);
        }
        return BadRequest();
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }

    
    // PUT api/<controller>/5
    [HttpPut("CambiarEstado/{id}")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] Inmueble i)
    {
      try
      {
        Propietario p=contexto.Propietarios.Single(e => e.Email == User.Identity.Name);
        if (contexto.Inmuebles.Include(e => e.Propietario).FirstOrDefault(e => e.Id == id && p.Email==User.Identity.Name) != null)
        {
          var inmueble = contexto.Inmuebles.Single(e => e.Id == id);
          inmueble.Estado=i.Estado;
          contexto.SaveChanges();
          return Ok(inmueble);
        }
        return BadRequest();
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }


    // DELETE api/<controller>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Borrar(int id)
    {
      try
      {
        var entidad = contexto.Inmuebles.Include(e => e.Propietario).FirstOrDefault(e => e.Id == id && e.Propietario.Email == User.Identity.Name);
        if (entidad != null)
        {
          contexto.Inmuebles.Remove(entidad);
          contexto.SaveChanges();
          return Ok();
        }
        return BadRequest();
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }

    // DELETE api/<controller>/5
    [HttpDelete("BajaLogica/{id}")]
    public async Task<IActionResult> BajaLogica(int id)
    {
      try
      {
        var entidad = contexto.Inmuebles.Include(e => e.Propietario).FirstOrDefault(e => e.Id == id && e.Propietario.Email == User.Identity.Name);
        if (entidad != null)
        {
          entidad.Superficie = -1;//cambiar por estado = 0
          contexto.Inmuebles.Update(entidad);
          contexto.SaveChanges();
          return Ok();
        }
        return BadRequest();
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }
  }
}
