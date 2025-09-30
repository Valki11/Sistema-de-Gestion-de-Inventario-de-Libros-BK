using Inventario.Infrastructure;
using Inventario.Infrastructure.Data; 
using Microsoft.EntityFrameworkCore;   

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddDbContext<BibliotecaDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, LogLevel.Information));

var MyCors = "_mycors";
builder.Services.AddCors(o => o.AddPolicy(MyCors, p =>
    p.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.UseCors(MyCors);

app.Run();
