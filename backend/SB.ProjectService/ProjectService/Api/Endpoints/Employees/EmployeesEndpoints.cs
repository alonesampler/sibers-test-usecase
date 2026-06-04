using ProjectService.Domain;

namespace ProjectService.Api.Endpoints.Employees;

public static class EmployeesEndpoints
{
    public static void MapEmployeesEndpoints(this IEndpointRouteBuilder builder)
    {
        var v1Group = builder.MapGroup("api/v1/employees").WithTags("Employees");

        v1Group.MapPost("", CreateEmployeeEndpoint.Handle)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director));

        v1Group.MapGet("", GetEmployeesEndpoint.GetAll)
            .RequireAuthorization();

        v1Group.MapGet("search", GetEmployeesEndpoint.Search)
            .RequireAuthorization();

        v1Group.MapGet("{id:guid}", GetEmployeesEndpoint.GetById)
            .RequireAuthorization();

        v1Group.MapPut("{id:guid}", UpdateDeleteEmployeeEndpoint.Update)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director));

        v1Group.MapDelete("{id:guid}", UpdateDeleteEmployeeEndpoint.Delete)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director));
    }
}
