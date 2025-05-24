using Microsoft.EntityFrameworkCore;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.DependencyInjection;
using System.Configuration;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(builder.Configuration);

var app = builder.Build();

app.UseInfrastrcturePolicy();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
}


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
