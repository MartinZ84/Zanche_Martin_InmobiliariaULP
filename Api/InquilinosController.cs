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
  public class InquilinosController : Controller
  {
    private readonly DataContext contexto;

     private readonly IWebHostEnvironment environment;

    public InquilinosController(DataContext contexto,IWebHostEnvironment environment)
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
      //   return Ok(contexto.Inmuebles.Include(e => e.Propietario).Where(e => e.Propietario.Email == usuario)//.Select(x => new InmuebleView(x)));
      //  .Single(e => e.Id == id));
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
  }
}
