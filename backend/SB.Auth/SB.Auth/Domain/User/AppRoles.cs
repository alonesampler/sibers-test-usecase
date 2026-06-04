namespace SB.Auth.Domain.User;

public static class AppRoles
{
    public const string Director = "Director";
    public const string ProjectManager = "ProjectManager";
    public const string Employee = "Employee";

    public static readonly string[] All = [Director, ProjectManager, Employee];
}
