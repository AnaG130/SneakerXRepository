using Microsoft.AspNetCore.StaticFiles; //  agrega esto arriba
using Negocio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();

// Registrar servicios de negocio
builder.Services.AddScoped<UbicacionesNegocio>();
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();

// Sirve estáticos y mapea .webmanifest
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".webmanifest"] = "application/manifest+json";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,
    // (Opcional) evita cachear agresivamente el SW para facilitar updates
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.Equals("sw.js", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            ctx.Context.Response.Headers["Pragma"] = "no-cache";
            ctx.Context.Response.Headers["Expires"] = "0";
        }
    }
});

app.UseRouting();

app.UseAuthorization();

// Redirigir la ruta raíz ("/") a la página de bienvenida ("/Welcome")
app.MapGet("/", () => Results.Redirect("/WelcomePage"));

// Ruta para Razor Pages
app.MapRazorPages();

app.Run();
