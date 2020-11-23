using FluentValidation;


namespace TodoApi.Models
{
    public class TodoValidator : AbstractValidator<Todo>
    {
        public TodoValidator()
        {
            // Validate the description and between 1 to 50
            RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Must fill this field")
            .Length(1, 50).WithMessage("Must be betweem 1 to 50 characters");
        }
    }
}
