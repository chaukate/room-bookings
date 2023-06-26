using FluentValidation;

namespace RB.Application.Teams.Commands
{
    public class AllocateTeamMembersValidator : AbstractValidator<AllocateTeamMembersCommand>
    {
        public AllocateTeamMembersValidator()
        {
            RuleFor(r => r.TeamId)
                .NotEmpty()
                .WithMessage("TeamId is required.");

            RuleFor(r => r.MembersId)
                .Must(m => m != null && m.Length > 0)
                .WithMessage("MembersId is required.");

            RuleForEach(r => r.MembersId)
                .NotEmpty()
                .WithMessage("MemberId is required.")
                .When(w => w.MembersId != null && w.MembersId.Length > 0);
        }
    }
}
