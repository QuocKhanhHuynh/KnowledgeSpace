using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Contents
{
    public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(200).WithMessage("Name cannot over 200 charactor");
            RuleFor(x => x.SeoAlias).NotEmpty().WithMessage("Seo Alias is required").MaximumLength(200).WithMessage("Seo Alias cannot over 200 charactor");
            RuleFor(x => x.SeoDescription).MaximumLength(200).WithMessage("Seo Description cannot over 200 charactor");
            RuleFor(x => x.SortOrder).GreaterThan(0).WithMessage("Sort Order is required");
        }
    }
}
