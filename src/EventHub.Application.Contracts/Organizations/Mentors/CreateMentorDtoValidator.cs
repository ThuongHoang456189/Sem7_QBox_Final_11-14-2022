using FluentValidation;

namespace EventHub.Organizations.Mentors
{
    public class CreateMentorDtoValidator : AbstractValidator<CreateMentorDto>
    {
        public CreateMentorDtoValidator()
        {
            RuleFor(x => x.Name).Length(1, 50);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required")
                .EmailAddress().WithMessage("A valid email is required");
        }
    }
}
