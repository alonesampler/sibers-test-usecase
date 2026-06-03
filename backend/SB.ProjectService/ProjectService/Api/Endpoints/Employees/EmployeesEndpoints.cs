namespace ProjectService.Api.Endpoints.Employees;

public static class EmployeesEndpoints
{
    public static void MapEmployeesEndpoints(this IEndpointRouteBuilder builder)
    {
        var v1Group = builder.MapGroup("api/v1/employees").WithTags("Employees");

        v1Group.MapPost("", CreateEmployeeEndpoint.Handle);
        v1Group.MapGet("", GetEmployeesEndpoint.GetAll);
        v1Group.MapGet("search", GetEmployeesEndpoint.Search);
        v1Group.MapGet("{id:guid}", GetEmployeesEndpoint.GetById);
        v1Group.MapPut("{id:guid}", UpdateDeleteEmployeeEndpoint.Update);
        v1Group.MapDelete("{id:guid}", UpdateDeleteEmployeeEndpoint.Delete);
    }
}
