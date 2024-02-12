using AppContext.Context;
using AppContext.Repository.Implements;
using AppContext.Repository.Interfaces;
using LogicDomain.Repository.Implements;
using LogicDomain.Repository.Interfaces;
using MagicVilla_API_2024.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var config = builder.Configuration;
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IVillaService, VillaService>();
builder.Services.AddScoped<IVillaService_Domain, VillaService_Domain>();
builder.Services.AddAutoMapper(typeof(MapperConfig));

#region Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(
    config.GetConnectionString("SQLConnection"),
    b => b.MigrationsAssembly("MagicVilla_API_2024")
);
    });
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
