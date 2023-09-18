using FluentValidation;
using ForecastingSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Threading;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models.Validators
{
    public class PhaseResourceModelValidator : AbstractValidator<PhaseResourceModel>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IProjectRateService _projectRateService;
        public PhaseResourceModelValidator(IEmployeeService employeeService,
            IProjectRateService projectRateService)
        {
            _employeeService = employeeService;
            _projectRateService = projectRateService;

            RuleFor(phase => phase.PhaseId)
                .Must(PhaseId => PhaseId != 0)
            .WithMessage("PhaseId must be provided");

            RuleFor(phase => phase.ProjectRateId).MustAsync(MustProvideValidProjectRateAsync)
           .WithMessage("ProjectRateId must be provided or exist in the system.");

            //RuleFor(phase => phase.HoursPerWeek)
            //    .Must(HoursPerWeek => HoursPerWeek > 0)
            //.WithMessage("HoursPerWeek must be greater than 0");

            //RuleFor(phase => phase.FTE)
            //    .Must(FTE => FTE > 0) 
            //.WithMessage("FTE must be greater than 0");

            RuleFor(phase => phase).MustAsync(MustProvideEmployeeOrPlaceHolderAsync)
                .WithMessage("EmployeeId must be provided or exists in the system or provide a place holder with included country");
        }

        private async Task<bool> MustProvideEmployeeOrPlaceHolderAsync(PhaseResourceModel model, CancellationToken arg2)
        {
            if (!string.IsNullOrWhiteSpace(model.FullName) && model.FullName.ToLower().IndexOf("(place holder)") > 0 && !string.IsNullOrWhiteSpace(model.Country))
            {
                return true;
            }
            else if (model.EmployeeId.HasValue && model.EmployeeId.Value > 0)
            {
                return await _employeeService.IsExistingEmployeeAsync(model.EmployeeId.Value);
            }
            return false;
        }

        private async Task<bool> MustProvideValidProjectRateAsync(int projectRateId, CancellationToken arg2)
        {
            return await _projectRateService.IsActiveProjectRateAsync(projectRateId);
        }

    }
}
