using FluentValidation;

namespace RB.Application.Rooms.Commands
{
    public class CreateRoomValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomValidator()
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

            RuleFor(r => r.Capacity)
                .Must(m => m > 0)
                .WithMessage("Capacity must be greater than 0.");
        }
    }
}
