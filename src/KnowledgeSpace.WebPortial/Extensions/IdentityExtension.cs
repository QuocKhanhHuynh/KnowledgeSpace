using System.Security.Claims;

namespace KnowledgeSpace.WebPortal.Extensions
{
    public static class IdentityExtension
    {
        public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .SingleOrDefault(x => x.Type == "fullName");
            return claim.Value;
        }
    }
}
