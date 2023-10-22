using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KnowledgeSpace.BackendServer.Helpers
{
    public class ApiValidatorFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(context.ModelState));
            }

            base.OnActionExecuting(context);
        }
    }
}