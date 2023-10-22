using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Systems
{
    public class UserPasswordChangeRequestValidator : AbstractValidator<UserPasswordChangeRequest>
    {
        public UserPasswordChangeRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required");
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required");
        }
    }
}
