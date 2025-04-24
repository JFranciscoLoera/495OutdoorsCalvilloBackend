using Backend_P13C.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(); // ← Esta línea es clave;
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Registra la clase Consulta para la inyecci�n de dependencias
builder.Services.AddSingleton<Consulta>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();  // Mapea los controladores aqu�

app.Run();
