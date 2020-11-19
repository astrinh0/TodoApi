using FluentValidation;
using System;

namespace TodoApi.Models
{
    public class TodoUserValidator : AbstractValidator<TodoUser>
    {
        public TodoUserValidator()
        {
            // Validate the name and between 1 to 50
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Must fill this field")
            .Length(1, 50).WithMessage("Must be betweem 1 to 50 characters");
        }

    }

}
