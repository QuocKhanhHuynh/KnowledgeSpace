using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Contents
{
    public class CommentCreateRequestValidator : AbstractValidator<CommentCreateRequest>
    {
        public CommentCreateRequestValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required").MaximumLength(500).WithMessage("Content cannot over 500 charactor");
            RuleFor(x => x.KnowledgeBaseId).GreaterThan(0).WithMessage("KnowledgeBase Id is required");
        }
    }
}
