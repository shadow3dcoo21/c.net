var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers();

// Registrar HttpClient en el contenedor de servicios
builder.Services.AddHttpClient();

// Configurar CORS si es necesario
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins(
                    "http://localhost:3000", 
                    "https://localhost:3000",
                    "http://frontnetex1.vercel.app",
                    "https://frontnetex1.vercel.app")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Configurar Kestrel para HTTP y HTTPS
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5295); // Puerto HTTP
    serverOptions.ListenAnyIP(44381, listenOptions =>
    {
        listenOptions.UseHttps(); // Puerto HTTPS
    });
});

var app = builder.Build();

// Configurar CORS
app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
