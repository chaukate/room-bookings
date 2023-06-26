using FluentValidation;

namespace RB.Application.Members.Commands
{
    public class AssignMemberToTeamsValidator : AbstractValidator<AssignMemberToTeamsCommand>
    {
        public AssignMemberToTeamsValidator()
        {
            RuleFor(r => r.MemberId)
                .NotEmpty()
                .WithMessage("MemberId is required.");

            RuleFor(r => r.TeamsId)
                .Must(m => m != null && m.Length > 0)
                .WithMessage("TeamsId is required.");

            RuleForEach(r => r.TeamsId)
                .NotEmpty()
                .WithMessage("TeamId is required.")
                .When(w => w.TeamsId != null && w.TeamsId.Length > 0);
        }
    }
}
