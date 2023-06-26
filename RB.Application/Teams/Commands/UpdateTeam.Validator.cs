using FluentValidation;

namespace RB.Application.Teams.Commands
{
    public class UpdateTeamValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamValidator()
        {
            RuleFor(r => r.Name)
              .NotEmpty()
              .WithMessage("Name is required.")
              .MaximumLength(100)
              .WithMessage("Maximum character limit is 100.");

            RuleFor(r => r.Description)
                .NotEmpty()
                .WithMessage("Description is requried.")
                .MaximumLength(3000)
                .WithMessage("Maximum character limit is 3000.");

            RuleFor(r => r.LeadId)
                .Must(m => m > 0)
                .WithMessage("LeadId is required.");
        }
    }
}
