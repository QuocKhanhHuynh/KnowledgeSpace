using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Contents
{
    public class VoteCreateRequestValidator : AbstractValidator<VoteCreateRequest>
    {
        public VoteCreateRequestValidator()
        {
            RuleFor(x => x.KnowledgeBaseId).GreaterThan(0).WithMessage("KnowledgeBase Id is requried");
        }
    }
}
