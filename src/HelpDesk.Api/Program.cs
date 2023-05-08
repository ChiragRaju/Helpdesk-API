using HelpDesk.Api.Services;
using HelpDesk.Application;
using HelpDesk.Application.Interfaces.Services;
using HelpDesk.Domain.Data;
using HelpDesk.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using HelpDesk.Domain.Data;
using HelpDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

// Configure Services
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
// TODO: Remove this line if you want to return the Server header
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

builder.Services.AddSingleton(builder.Configuration);

// Adds in Application dependencies
builder.Services.AddApplication(builder.Configuration);
// Adds in Infrastructure dependencies
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = ApiVersion.Default;
});

builder.Services.AddScoped<IPrincipalService, PrincipalService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HelpDesk.Api", Version = "v1" });
});

// Configure Application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HelpDesk.Api v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.Use(async (httpContext, next) =>
{
    var apiMode = httpContext.Request.Path.StartsWithSegments("/api");
    if (apiMode)
    {
        httpContext.Request.Headers[HeaderNames.XRequestedWith] = "XMLHttpRequest";
    }
    await next();
});

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health");
    endpoints.MapControllers();
});

app.Run();
