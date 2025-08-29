using FarmaciaAPI.IServices;
using FarmaciaAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar ambos servicios
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ISalidaService, SalidaService>();
builder.Services.AddScoped<IEntradasService, EntradasService>();

// Registrar IConfiguration y ILogger
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddLogging();

// Controladores + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        policy => policy.WithOrigins("https://localhost:7000", "http://localhost:5000")
                       .AllowAnyHeader()
                       .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();