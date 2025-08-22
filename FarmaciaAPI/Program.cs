using FarmaciaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 👇 Aquí registramos tu servicio como Scoped
builder.Services.AddScoped<ProductoService>();

// Controladores + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
