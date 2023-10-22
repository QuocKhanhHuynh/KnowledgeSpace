using IdentityServer4.Models;
using IdentityServer4;

namespace KnowledgeSpace.BackendServer.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
          new IdentityResource[]
          {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
          };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api.knowledgespace", "KnowledgeSpace API")
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api.knowledgespace", "KnowledgeSpace API")
            };
    }
}
