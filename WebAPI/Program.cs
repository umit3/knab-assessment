using Application.Extensions;
using Infrastructure.Extensions;
using WebAPI.Filters;
using WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FiatConfiguration>(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers(options => { options.Filters.Add<ApiExceptionFilterAttribute>(); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddValidation();

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