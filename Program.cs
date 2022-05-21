using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Zanche_Martin_InmobiliariaULP.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);
});

	/* PARA MySql - usando Pomelo */
		builder.Services.AddDbContext<DataContext>(
				options => options.UseMySql(
					builder.Configuration["ConnectionStrings:DefaultConnection"],
					ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"])
				)
			);

//Agregar servicios de autoriazación 
builder.Services.AddAuthorization(options =>
{
  
    // options.AddPolicy("Administrador", policy => policy.RequireClaim("Administrador"));
    // options.AddPolicy("Usuario", policy => policy.RequireClaim("Usuario"));
    // options.AddPolicy("Empleado", policy => policy.RequireClaim("Empleado"));
    //agregar políticas de autorización en empleados para que deje tambien a administrador y superadministrador
     options.AddPolicy("Empleado", policy => policy.RequireRole("Empleado", "Administrador", "SuperAdministrador"));
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "SuperAdministrador"));
});       

//Agrega de autenticación con cookie  
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.LogoutPath = "/Home/Logout";
        options.AccessDeniedPath = "/Home/Restringido";
    })
    //Agrego autenticacion con token
    .AddJwtBearer(options =>//la api web valida con token
				{
					options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = builder.Configuration["TokenAuthentication:Issuer"],
						ValidAudience = builder.Configuration["TokenAuthentication:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
						builder.Configuration["TokenAuthentication:SecretKey"])),
					};
					// opción extra para usar el token el hub. Que es esto?
					// options.Events = new JwtBearerEvents
					// {
					// 	OnMessageReceived = context =>
					// 	{
					// 		// Read the token out of the query string
					// 		var accessToken = context.Request.Query["access_token"];
					// 		// If the request is for our hub...
					// 		var path = context.HttpContext.Request.Path;
					// 		if (!string.IsNullOrEmpty(accessToken) &&
					// 			path.StartsWithSegments("/chatsegurohub"))
					// 		{//reemplazar la url por la usada en la ruta ⬆
					// 			context.Token = accessToken;
					// 		}
					// 		return Task.CompletedTask;
					// 	}
					// };
				});;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
