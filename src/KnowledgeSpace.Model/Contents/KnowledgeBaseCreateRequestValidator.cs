using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Contents
{
    public class KnowledgeBaseCreateRequestValidator : AbstractValidator<KnowledgeBaseCreateRequest>
    {
        public KnowledgeBaseCreateRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required");

            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");

            RuleFor(x => x.Problem).NotEmpty().WithMessage("Problem is required");

            RuleFor(x => x.Note).NotEmpty().WithMessage("Note is required");
            RuleFor(x => x.CaptchaCode).NotEmpty().WithMessage("CaptchaCode is required");
        }
    }
}
