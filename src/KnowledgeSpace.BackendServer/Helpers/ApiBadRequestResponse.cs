using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KnowledgeSpace.BackendServer.Helpers
{
    public class ApiBadRequestResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get;}
        public ApiBadRequestResponse(ModelStateDictionary modelState) : base(400)
        {
            Errors = modelState.SelectMany(c => c.Value.Errors).Select(x => x.ErrorMessage).ToArray();
        }
        public ApiBadRequestResponse(IdentityResult identityResult) : base(400)
        {
            Errors = identityResult.Errors.Select(x => x.Code + " - " + x.Description);
        }
        public ApiBadRequestResponse(string message) : base(400,message)
        {
        }
    }
}
