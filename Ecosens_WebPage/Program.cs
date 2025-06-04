using Ecosens_WebPage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Ruta de inicio de sesión. Si el usuario no está autenticado, se les redirige aquí.
        options.LoginPath = "/Sesion/Login";

        // Ruta que se redirige cuando el usuario intenta acceder a una página restringida.
        options.AccessDeniedPath = "/Sesion/AccessDenied";

        // Otras opciones pueden ser configuradas aquí, como la duración del cookie.
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<LoginService>();
builder.Services.AddHttpClient<SesionDataService>();
builder.Services.AddHttpClient<NotificacionService>();
builder.Services.AddHttpClient<AreaService>();
builder.Services.AddHttpClient<ConjuntoService>();
builder.Services.AddHttpClient<DashboardService>();
builder.Services.AddHttpClient<ContenedoresService>();
builder.Services.AddHttpClient<RegistroService>();


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


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
