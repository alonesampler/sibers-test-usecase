using SB.Auth.Api.Endpoints;
using SB.Auth.Domain;
using SB.Auth.Infrastructire;
using SB.Auth.Infrastructire.OpenApi;
using SB.Auth.Infrastructire.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddOpenApi(OpenApiConfigurator.Configure);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

await app.Services.ApplyMigrationsAsync();
await app.Services.SeedAsync();

app.MapGet("/health", () => Results.Ok());

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.Run();
