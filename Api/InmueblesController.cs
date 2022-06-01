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
        var inmuebles= (contexto.Inmuebles.Include(e => e.Propietario).Where(e => e.Propietario.Email == usuario));
        //.Select(x => new InmuebleView(x)));
        foreach (var i in inmuebles)
        {
          if(i.Estado.Equals("Disponible")){
                i.EstadoInmueble=true;
              } else
              {
                i.EstadoInmueble=false;
              }
        }
        //cast to IQueryable<InmuebleView>
        //var inmueblesView = inmuebles.Select(x => new InmuebleView(x));
        //return Ok(inmueblesView);
        return Ok(inmuebles.Select(x => new InmuebleView(x)));
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
        var inmueble=contexto.Inmuebles.Include(e => e.Propietario).Where(e => e.Propietario.Email == usuario).Single(e => e.Id == id);
        if(inmueble.Estado.Equals("Disponible")){
        inmueble.EstadoInmueble=true;
      } else
      {
        inmueble.EstadoInmueble=false;
      }

      return Ok(new InmuebleView(inmueble));
      }
    
      catch (Exception ex)
      {
        return BadRequest("Inmuebles no encontrado" +"\r\n"+ ex) ;
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
        if((bool)entidad.EstadoInmueble){
            entidad.Estado="Disponible";
          } else
          {
            entidad.Estado="No Disponible";
          }
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
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] InmuebleApi inmuebleApi)
    {
      try
      {
        Inmueble entidad = contexto.Inmuebles.Single(e => e.Id == id);
        if (contexto.Inmuebles.AsNoTracking().Include(e => e.Propietario).FirstOrDefault(e => e.Id == id && e.Propietario.Email == User.Identity.Name) != null)
        {
         // entidad.EstadoInmueble=EstadoInmueble;
          if((bool)inmuebleApi.EstadoInmueble){
            entidad.Estado="Disponible";
            entidad.EstadoInmueble=true;
          } else
          {
            entidad.Estado="No Disponible";
            entidad.EstadoInmueble=false;
          }
          contexto.Inmuebles.Update(entidad);
          contexto.SaveChanges();
          return Ok(new InmuebleView(entidad));
        }
        return BadRequest();
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }
    // {
    //   try
    //   {
    //        if((bool)i.EstadoInmueble){
    //         i.Estado="Disponible";
    //       } else
    //       {
    //         i.Estado="No Disponible";
    //       }
        
    //     Propietario p=contexto.Propietarios.Single(e => e.Email == User.Identity.Name);
    //     if (contexto.Inmuebles.Include(e => e.Propietario).FirstOrDefault(e => e.Id == id && p.Email==User.Identity.Name) != null)
    //     {
    //       var inmueble = contexto.Inmuebles.Single(e => e.Id == id);
    //       inmueble.Estado=i.Estado;
    //       contexto.SaveChanges();
    //       return Ok("Se actualizo estado a del inmueble id:" + inmueble.Id + " a "+'"'+inmueble.Estado+'"');
    //     }
    //     return BadRequest();
    //   }
    //   catch (Exception ex)
    //   {
    //     return BadRequest(ex);
    //   }
    // }


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
