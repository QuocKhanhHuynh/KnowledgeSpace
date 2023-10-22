using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Systems
{
    public class FunctionCreateRequestValidator : AbstractValidator<FunctionCreateRequest>
    {
        public FunctionCreateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required").MaximumLength(50).WithMessage("Id cannot over 50 charactors");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(200).WithMessage("Id cannot over 200 charactors");
            RuleFor(x => x.Url).NotEmpty().WithMessage("URL is required").MaximumLength(200).WithMessage("Id cannot over 200 charactors");
            RuleFor(x => x.SortOrder).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.ParentId).MaximumLength(50).When(x => !string.IsNullOrEmpty(x.ParentId)).WithMessage("Parent Id cannot over 50 charactors");
            //RuleFor(x => x.Icon).MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Icon)).WithMessage("Parent Id cannot over 50 charactors");
        }
    }
}
