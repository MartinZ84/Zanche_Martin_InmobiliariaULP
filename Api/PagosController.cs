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
  public class PagosController : Controller
  {
    private readonly DataContext contexto;

     private readonly IWebHostEnvironment environment;

    public PagosController(DataContext contexto,IWebHostEnvironment environment)
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
        var  pagos= await (contexto.Pagos.Include(e => e.Contrato).Where(e => e.Contrato.Inmueble.Propietario.Email == usuario).ToArrayAsync());
        List <PagoView> pagosView = new List<PagoView>();
        foreach (var pago in pagos)
        {
          pagosView.Add(new PagoView(pago));
        }
        return pagos != null ? Ok( pagosView) : NotFound();
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
        var pagos= await (contexto.Pagos.Include(c=> c.Contrato).Where(c => c.Contrato.Inmueble.Propietario.Email == usuario && c.ContratoId==id)).ToListAsync();
       List <PagoView> pagosView = new List<PagoView>();
        foreach (var pago in pagos)
        {
          pagosView.Add(new PagoView(pago));
        }
        return pagos != null ? Ok( pagosView) : NotFound();
      
      }
    
      catch (Exception ex)
      {
        return BadRequest("Contrato no encontrado" +"\r\n"+ ex) ;
      }
    }    
  }
}
