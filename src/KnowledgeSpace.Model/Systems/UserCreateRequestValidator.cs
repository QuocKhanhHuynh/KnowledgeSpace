using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Systems
{
    public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateRequestValidator()
        {
            //RuleFor(x => x.UserName).Null(); /NotEmpty().WithMessage("User Name is required")
            RuleFor(x => x.Password).Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$").When(x => !string.IsNullOrEmpty(x.Password)).WithMessage("Password is not match"); //NotEmpty().WithMessage("Password is required")
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required").MaximumLength(50).WithMessage("First Name cannot over 50 charactor");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required").MaximumLength(50).WithMessage("First Name cannot over 50 charactor");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number is required");
            RuleFor(x => x.Email).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email cannot match").NotEmpty().WithMessage("Email is required");
        }
    }
}
