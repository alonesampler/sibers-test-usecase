namespace ProjectService.Api.Endpoints.Employees;

public static class EmployeesEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder builder)
    {
        var v1Group = builder.MapGroup("api/v1/employees");

        v1Group.MapPost("", CreateEmployeeEndpoint.Handle).WithTags("Employees");
        v1Group.MapGet("", GetEmployeesEndpoint.GetAll).WithTags("Employees");
        v1Group.MapGet("search", GetEmployeesEndpoint.Search).WithTags("Employees");
        v1Group.MapGet("{id:guid}", GetEmployeesEndpoint.GetById).WithTags("Employees");
        v1Group.MapPut("{id:guid}", UpdateDeleteEmployeeEndpoint.Update).WithTags("Employees");
        v1Group.MapDelete("{id:guid}", UpdateDeleteEmployeeEndpoint.Delete).WithTags("Employees");
    }
}
