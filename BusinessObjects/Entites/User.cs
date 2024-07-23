using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BusinessObjects.Entites;

public class User : IdentityUser<int>
{
    [Required]
    override
    public string UserName { get; set; }

    // Add any additional properties if needed
}
