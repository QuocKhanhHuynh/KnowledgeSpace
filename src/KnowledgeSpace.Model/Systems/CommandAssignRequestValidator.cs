using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Systems
{
    public class CommandAssignRequestValidator : AbstractValidator<CommandAssignRequest>
    {
        public CommandAssignRequestValidator()
        {
            RuleFor(x => x.AddToAllFunctions).NotNull()
               .WithMessage("AddToAllFunctions is required");
        }
    }
}
