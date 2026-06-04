using ProjectService.Api.Endpoints.Documents;
using ProjectService.Api.Endpoints.Employees;
using ProjectService.Api.Endpoints.Projects;
using ProjectService.Api.Endpoints.Tasks;
using ProjectService.Domain;
using ProjectService.Infrastructure;
using ProjectService.Infrastructure.OpenApi;
using ProjectService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddValidation();
builder.Services.AddOpenApi(OpenApiConfigurator.Configure);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

await app.Services.ApplyMigrationsAsync();

app.MapEmployeesEndpoints();
app.MapProjectsEndpoints();
app.MapDocumentsEndpoints();
app.MapTasksEndpoints();

app.Run();
