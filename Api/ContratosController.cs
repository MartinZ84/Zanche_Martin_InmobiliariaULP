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
  public class ContratosController : Controller
  {
    private readonly DataContext contexto;

     private readonly IWebHostEnvironment environment;

    public ContratosController(DataContext contexto,IWebHostEnvironment environment)
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
        var contratos=  (contexto.Contratos.Include(c=> c.Inmueble).Include(i=>i.Inquilino).Where(c => c.Inmueble.Propietario.Email == usuario)).ToListAsync();

    
        return contratos != null ? Ok(await contratos) : NotFound();
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }

     // GET: api/<controller>
    [HttpGet("Vigentes")]
    public async Task<IActionResult>ContratosVigentes()
    {
      try
      {
        var usuario = User.Identity.Name;
        var contratos= (contexto.Contratos.Include(c=> c.Inmueble).Include(i=>i.Inquilino).Where(c => c.Inmueble.Propietario.Email == usuario && c.FechaInicio<=DateTime.Now && c.FechaFin>=DateTime.Now)).ToListAsync();

        return (contratos != null ? Ok(await contratos) : NotFound());
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }  
  }
}
  



    
