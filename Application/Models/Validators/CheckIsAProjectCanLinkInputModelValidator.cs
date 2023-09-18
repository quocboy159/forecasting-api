using FluentValidation;

namespace ForecastingSystem.Application.Models.Validators
{
    public class CheckIsAProjectCanLinkInputModelValidator: AbstractValidator<CheckIsAProjectCanLinkInputModel>
    {
        public CheckIsAProjectCanLinkInputModelValidator()
        {
            RuleFor(phase => phase.ProjectCode).NotEmpty();
        }
    }
}
