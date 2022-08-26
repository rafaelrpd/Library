using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Api.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<LIBRARYContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("LIBRARYConnectionString") ?? throw new InvalidOperationException("Connection string 'LIBRARYConnectionString' not found.")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    // Todo: serve file of MIT LICENSE as API License.
    //var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";");
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Library API",
        Description = "An ASP.NET Core Web API for managing a library",
        //TermsOfService = new Uri("https://uri-aqui.com"),
        Contact = new OpenApiContact
        {
            Name = "Rafael Pereira Dias",
            Email = "rafaelpdias@pm.me"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License"
            //Url = new Uri($"{urls[1]}/{Path.Combine(AppContext.BaseDirectory, LICENSE)}")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
