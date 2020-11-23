using FluentValidation;

namespace TodoApi.Models
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            // Validate the name and between 1 to 50
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Must fill this field")
            .Length(1, 50).WithMessage("Must be betweem 1 to 50 characters");
        }


    }

}
