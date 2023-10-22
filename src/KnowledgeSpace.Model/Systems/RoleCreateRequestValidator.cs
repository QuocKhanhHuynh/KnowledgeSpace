using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Systems
{
    public class RoleCreateRequestValidator : AbstractValidator<RoleCreateRequest>
    {
        public RoleCreateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required").MaximumLength(50).WithMessage("Id do not exceed 50 charactor");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(50).WithMessage("Name do not exceed 50 charactor");
        }
    }
}
