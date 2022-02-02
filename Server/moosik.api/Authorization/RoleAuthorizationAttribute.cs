using Microsoft.AspNetCore.Authorization;

namespace moosik.api.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RoleAuthorizationAttribute  : AuthorizeAttribute
    {
        public RoleAuthorizationAttribute(params MoosikRoles[] requiredRoles)
        {
            if (!requiredRoles.Any())
            {
                requiredRoles = GetDefaultedRequiredRole().ToArray();
            }

            Roles = string.Join(',', requiredRoles);
        }

        private static IEnumerable<MoosikRoles> GetDefaultedRequiredRole()
        {
            yield return MoosikRoles.User;
        }
    }
    
    
}
