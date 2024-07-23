using Microsoft.AspNetCore.Authorization;

namespace WebAPI;

public class AdminEmailRequirement : IAuthorizationRequirement
{
    public AdminEmailRequirement()
    {
        
    }
}
