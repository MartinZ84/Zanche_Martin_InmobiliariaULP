using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Zanche_Martin_InmobiliariaULP.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zanche_Martin_InmobiliariaULP.Api
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class PropietariosController : ControllerBase//
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
    private readonly IWebHostEnvironment environment;


		public PropietariosController(DataContext contexto, IConfiguration config,IWebHostEnvironment environment)
		{
			this.contexto = contexto;
			this.config = config;
      this.environment = environment;
		}
		// GET: api/<controller>
		[HttpGet]
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{
				// contexto.Inmuebles
        //             .Include(x => x.Propietario)
        //             .Where(x => x.Propietario.Nombre == "")//.ToList() => lista de inmuebles
        //             .Select(x => x.Propietario)
        //             .ToList();//lista de propietarios
				var usuario = User.Identity.Name;
				// contexto.Contratos.Include(x => x.Inquilino).Include(x => x.Inmueble).ThenInclude(x => x.Propietario)
        //             .Where(c => c.Inmueble.Propietario.Email == usuario);
				/*var res = contexto.Propietarios.Select(x => new { x.Nombre, x.Apellido, x.Email })
                    .SingleOrDefault(x => x.Email == usuario);*/
			//	return await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
        return await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
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
				var entidad = await contexto.Propietarios.SingleOrDefaultAsync(x => x.Id == id);
				return entidad != null ? Ok(entidad) : NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// GET api/<controller>/GetAll
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				return Ok(await contexto.Propietarios.ToListAsync());
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// POST api/<controller>/login
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginView loginView)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
				var p = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
				if (p == null || p.Clave != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, p.Email),
						new Claim("FullName", p.Nombre + " " + p.Apellido),
            new Claim("Id", p.Id.ToString()),
						new Claim(ClaimTypes.Role, "Propietario"),
					};

					var token = new JwtSecurityToken(
            issuer: config["TokenAuthentication:Issuer"],
            audience: config["TokenAuthentication:Audience"],
            claims: claims,
            //expires: DateTime.Now.AddMinutes(60),
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: credenciales);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// POST api/<controller>
    //Alta de propietario
		[HttpPost]
		public async Task<IActionResult> Post([FromForm] Propietario entidad)
		{
			try
			{
				if (ModelState.IsValid)
				{
          if(contexto.Propietarios.Any(x => x.Email == entidad.Email))
          {
            return BadRequest("El email ingresado ya esta registrado.");
          }
          if(contexto.Propietarios.Any(x => x.Dni == entidad.Dni))
          {
            return BadRequest("El DNI ingresado ya esta registrado.");
          }
          string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: entidad.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                        entidad.Clave=hashed;
					await contexto.Propietarios.AddAsync(entidad);
					contexto.SaveChanges();
          if (entidad.AvatarFile != null && entidad.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + entidad.Id + Path.GetExtension(entidad.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    entidad.Avatar = Path.Combine("/Uploads", fileName);
                    // Esta operación guarda la foto en memoria en el ruta que necesitamos
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        entidad.AvatarFile.CopyTo(stream);
                    }
                   contexto.Propietarios.Update(entidad);
				           await contexto.SaveChangesAsync(); 
                }
                    // repositorio.Modificacion(usuario);
                entidad.AvatarFile=null;     
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
    //Update propietario
	//	[HttpPut("{id}")]
    	[HttpPut]
		public async Task<IActionResult> Put([FromBody] Propietario entidad)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Propietario original = await contexto.Propietarios.AsNoTracking().SingleAsync(x=>x.Email==User.Identity.Name);
					entidad.Id = original.Id;
					if (String.IsNullOrEmpty(entidad.Clave))
					{
						entidad.Clave = original.Clave;
					}
					else
					{
						entidad.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: entidad.Clave,
							salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));
					}
           if (entidad.AvatarFile != null && entidad.Id > 0)
                        {
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "Uploads");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string fileName = "avatar_" + entidad.Id + Path.GetExtension(entidad.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            entidad.Avatar = Path.Combine("/Uploads", fileName);
                            // Esta operación guarda la foto en memoria en el ruta que necesitamos
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                entidad.AvatarFile.CopyTo(stream);
                            }     
                            // fin de edicion de avatar
                      }  //si no viene con archivo de avatar obtengo los datos del avatar del usuario guardado y luego guardo
                      else {
                        entidad.Avatar = original.Avatar;
                      } 
          //ChangeTrakcer.Clear para limpiar el cambio de tracker de la entidad porque si no arroja error error "System.InvalidOperationException: 'The instance of entity type 'entidadQueSeEstaTrakeando' cannot be tracked because another instance with the same key value for {'LstEmployeeId'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.          
          //contexto.ChangeTracker.Clear();         
					contexto.Propietarios.Update(entidad);
          
					await contexto.SaveChangesAsync();
					return Ok(entidad);
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
				if (ModelState.IsValid)
				{
					var p = contexto.Propietarios.Find(id);
					if (p == null)
						return NotFound();
					contexto.Propietarios.Remove(p);
					contexto.SaveChanges();
					return Ok("Se borro el propietario " + p.Id + " " + p.Nombre + " " + p.Apellido + " " + p.Dni); 
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// GET: api/Propietarios/test
		[HttpGet("test")]
		[AllowAnonymous]
		public IActionResult Test()
		{
			try
			{
				return Ok("anduvo");
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// GET: api/Propietarios/test/5
		[HttpGet("test/{codigo}")]
		[AllowAnonymous]
		public IActionResult Code(int codigo)
		{
			try
			{
				//StatusCodes.Status418ImATeapot //constantes con códigos
				return StatusCode(codigo, new { Mensaje = "Anduvo", Error = false });
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
	}
}
//PARA RECIBIR LA IMAGEN DE BYTES DE Y GUARDARLA. lUEGO MANEJAR EL DOMINIO DESDE EL LADO DE LA ANDROID
  //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
// 										string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFileName);
// 										string pathCompleto = Path.Combine(path, fileName);
// 										u.Avatar = Path.Combine("/Uploads", fileName);
// 										// Esta operación guarda la foto en memoria en la ruta que necesitamos
// 										/*using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
// 										{
// 												u.AvatarFile.CopyTo(stream);
// 										}*/
									
// 										System.IO.File.WriteAllBytes(pathCompleto, u.AvatarFileContent); repositorio.Modificacion(u);

// }
