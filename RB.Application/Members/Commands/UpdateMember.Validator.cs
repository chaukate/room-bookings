using FluentValidation;

namespace RB.Application.Members.Commands
{
    public class UpdateMemberValidator : AbstractValidator<UpdateMemberCommand>
    {
        public UpdateMemberValidator()
        {
            RuleFor(r => r.Name)
               .NotEmpty()
               .WithMessage("Name is required.")
               .MaximumLength(100)
               .WithMessage("Maximum character limit is 100.");

            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .MaximumLength(100)
                .WithMessage("Maximum character limit is 100.")
                .EmailAddress()
                .WithMessage("Invalid email address.");
        }
    }
}
