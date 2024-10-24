using GameOfDronesApi.Data;
using GameOfDronesApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<GameContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Aplicar migraciones automáticas al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GameContext>();
        // Aplica todas las migraciones pendientes, crea la base de datos si no existe
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Agregar manejo de excepciones si es necesario (logs, alertas, etc.)
        Console.WriteLine("Ocurrió un error al aplicar las migraciones: " + ex.Message);
    }
}

app.MapControllers();

app.Run();
