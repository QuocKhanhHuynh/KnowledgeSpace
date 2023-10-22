using Microsoft.AspNetCore.Identity;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    public class Role : IdentityRole
    {
        public List<Permission> Permissions { get; set; }
    }
}
