using FluentValidation;
using FluentValidation.AspNetCore;
using RecrutingTask.Api.Middleware;
using RecrutingTask.Api.Validators.Books;
using RecrutingTask.Domain.Repos;
using RecrutingTask.Infrastructure;
using RecrutingTask.Infrastructure.Repos;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging(builder => builder.AddConsole());

builder.Services.AddControllers(options => {
    options.FormatterMappings.SetMediaTypeMappingForFormat
            ("xml", MediaTypeHeaderValue.Parse("application/xml"));
    options.FormatterMappings.SetMediaTypeMappingForFormat
        ("js", MediaTypeHeaderValue.Parse("application/json"));
}).AddXmlSerializerFormatters();

//Validation filters (fluent validation)
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBookRequestDtoValidator>();

builder.Services
    .AddMvc(options => options.Filters.Add<CancelledOperationExceptionFilter>());

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Book CRUD API",
        Description = "An ASP.NET Core Web API for CRUD operations on books.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddScoped<IBookRepo, BookRepo>();

//Entity framework

builder.Services.AddDbContext<LibraryContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
