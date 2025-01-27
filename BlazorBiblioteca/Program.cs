using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using BlazorBiblioteca;
using BlazorBiblioteca.Data;
using BlazorBiblioteca.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Agregar BlazorBootstrap
builder.Services.AddBlazorBootstrap();


// Configurar HttpClient para BookService
builder.Services.AddHttpClient<BookService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5113/api/");
});

// Construir la aplicación
await builder.Build().RunAsync();
