using Microsoft.AspNetCore.Identity;

namespace SB.Auth.Domain.User;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid EmployeeId { get; set; }
}
