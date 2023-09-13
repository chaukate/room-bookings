using FluentValidation;

namespace RB.Application.Clients.Commands
{
    public class CreateClientValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(300)
                .WithMessage("Maximum character limit is 300.");

            RuleFor(r => r.AdminEmail)
                .NotEmpty()
                .WithMessage("Admin email is required.")
                .MaximumLength(300)
                .WithMessage("Maximum character limit is 300.")
                .EmailAddress()
                .WithMessage("Invalid email address.");
        }
    }
}
