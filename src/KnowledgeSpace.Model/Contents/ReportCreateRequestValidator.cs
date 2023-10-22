using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Contents
{
    public class ReportCreateRequestValidator : AbstractValidator<ReportCreateRequest>
    {
        public ReportCreateRequestValidator()
        {
            RuleFor(x => x.KnowledgeBaseId).GreaterThan(0).WithMessage("KnowledgeBase Id is required");
            RuleFor(x => x.Content).MaximumLength(500).WithMessage("Content over 500 charactor");
        }
    }
}
