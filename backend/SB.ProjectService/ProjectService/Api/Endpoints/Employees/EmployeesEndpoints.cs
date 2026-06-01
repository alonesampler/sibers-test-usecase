namespace ProjectService.Api.Endpoints.Employees;

public static class EmployeesEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder builder)
    {
        var v1Group = builder.MapGroup("api/v1/employees");

        v1Group.MapPost("", CreateEmployeeEndpoint.Handle);
    }
}
