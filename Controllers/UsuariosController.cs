using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zanche_Martin_InmobiliariaULP.Models;

namespace Zanche_Martin_InmobiliariaULP.Controllers
{
    public class UsuariosController : Controller
    {
          private RepositorioUsuario repositorio;

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

           public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            repositorio = new RepositorioUsuario(configuration);
          
        }
        // GET: Usuarios
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var usuarios = repositorio.ObtenerTodos();
            return View(usuarios);
           
        }

        // GET: Usuarios/Details/5
         [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
           var usuario = repositorio.ObtenerPorId(id);
          
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(usuario);
        }

        // GET: Usuarios/Create
       [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
             ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario usuario)
        
        {
          if (!ModelState.IsValid)
                return View();
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                usuario.Clave = hashed;
                usuario.Rol = User.IsInRole("SuperAdministrador") ? usuario.Rol : (int)enRoles.Empleado;
                var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                int res = repositorio.Alta(usuario);
                if (usuario.AvatarFile != null && usuario.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/Uploads", fileName);
                    // Esta operación guarda la foto en memoria en el ruta que necesitamos
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuario.AvatarFile.CopyTo(stream);
                    }
                    repositorio.Modificacion(usuario);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error=ex.Message;
                ViewBag.StackTrace=ex.StackTrace; 
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
        }

        
        // GET: Usuarios/Edit/5
        [Authorize]
        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            var u = repositorio.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View("Edit", u);
        }


        // GET: Usuarios/Edit/5
         [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
              ViewData["Title"] = "Editar usuario";
            var usuario = repositorio.ObtenerPorId(id);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario usuario)
        {         
          var vista = nameof(Edit);//de que vista provengo
            try
            {
                if (!User.IsInRole("Administrador"))//no soy admin
                {
                     vista = nameof(Perfil);//solo puedo ver mi perfil
                    // var usuarioActual = repositorio.ObtenerPorEmail(User.Identity.Name);
                    //if (usuarioActual.Id != id)//si no es admin, solo puede modificarse él mismo
                    //  if (usuarioActual.Email != User.Identity.Name)
                    //     return RedirectToAction(nameof(Index), "Home");

                    //ACA MODIFICACION DESDE EL PERFIL DE USUARIO
                    //else 
                    if(usuario.Clave==null)//si es el formulario de edicion de los datos sin la clave
                    {
                        // var usuarioActual = repositorio.ObtenerPorId(id);
                        var usuarioActual=repositorio.ObtenerPorEmail(User.Identity.Name);
                        usuario.Clave = usuarioActual.Clave;
                        usuario.Id=usuarioActual.Id;
                        usuario.Rol = usuarioActual.Rol;
                        //Identifico Si viene con archivo de avatar nuevo
                        if (usuario.AvatarFile != null && usuario.Id > 0)
                        {
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "Uploads");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                            string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            usuario.Avatar = Path.Combine("/Uploads", fileName);
                            // Esta operación guarda la foto en memoria en el ruta que necesitamos
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                usuario.AvatarFile.CopyTo(stream);
                            }
                            repositorio.Modificacion(usuario);
                            return RedirectToAction(vista);
                            // fin de edicion de avatar

                      }  else { //si no viene con archivo de avatar obtengo los datos del avatar del usuario guardado y luego guardo
                            usuario.Avatar = usuarioActual.Avatar;
                            repositorio.Modificacion(usuario);
                            return RedirectToAction(vista);
                            }
                          
                    }// Fin edicion de datos sin clave

                    else //entra por el formulario de actualizacion de clave              
                    {     
                        string claveNueva;                  
                        var usuarioActual=repositorio.ObtenerPorEmail(User.Identity.Name);
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: usuario.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
                            claveNueva = hashed;
                        repositorio.ModificacionClave(usuarioActual.Id, claveNueva);
                        return RedirectToAction(vista);
                    }//FIN EDICION DE CLAVE
                }//FIN DE EDICION DESDE EL PERFIL DE USUARIO

                //soy admin , identifico si viene a por form de datos o form clave
                else if(usuario.Clave==null)//si es el formulario de edicion de los datos sin la clave
                {
                    var usuarioActual = repositorio.ObtenerPorId(id);
                    usuario.Clave = usuarioActual.Clave;
                    //Identifico Si viene con archivo de avatar nuevo
                    if (usuario.AvatarFile != null && usuario.Id > 0)
                    {
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "Uploads");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                        string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        usuario.Avatar = Path.Combine("/Uploads", fileName);
                        // Esta operación guarda la foto en memoria en el ruta que necesitamos
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            usuario.AvatarFile.CopyTo(stream);
                        }
                        repositorio.Modificacion(usuario);
                        return RedirectToAction(vista);
                        // fin de edicion de avatar

                  }  else { //si no viene con archivo de avatar obtengo los datos del avatar del usuario guardado y luego guardo
                        usuario.Avatar = usuarioActual.Avatar;
                        repositorio.Modificacion(usuario);
                        return RedirectToAction(vista);
                        }
                      
                }// Fin edicion de datos sin clave

                else //entra por el formulario de actualizacion de clave              
                {     
                    string claveNueva;                    
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                        claveNueva = hashed;
                    repositorio.ModificacionClave(id, claveNueva);
                    return RedirectToAction(vista);
                }
              
              

               // return RedirectToAction(vista);
            }//del try
            catch (Exception ex)
            {
                Console.WriteLine(ex);  
                return View();
            }
        }

        // GET: Usuarios/Delete/5
         [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = repositorio.ObtenerPorId(id);
            return View(usuario);
           
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
         [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
               repositorio.Baja(id);  
               return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(usuario);
            }
        }

          [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

         // POST: Usuarios/Login/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string)? "/Home" : TempData["returnUrl"].ToString();                
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repositorio.ObtenerPorEmail(login.Usuario);
                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
      

          // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}