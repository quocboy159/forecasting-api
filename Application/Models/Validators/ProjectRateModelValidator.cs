using FluentValidation;
using ForecastingSystem.Application.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models.Validators
{
    public class ProjectRateModelValidator : AbstractValidator<ProjectRateModel>
    {
        IProjectRateService _projectRateService;
        public ProjectRateModelValidator(IProjectRateService projectRateService)
        {
            _projectRateService = projectRateService;

            RuleFor(rate => rate.RateName).NotEmpty()
            .WithMessage("Name must not be empty");

            RuleFor(rate => rate.ProjectId)
                .Must(projectId => projectId != 0)
            .WithMessage("ProjectId must be provided");
            //.Must(IsProjectExisted)
            //.WithMessage("Project with Id {} does not exists");

            RuleFor(rate => rate.RoleId).Must(roleId => roleId != 0)
            .WithMessage("RoleId must be provided");

            RuleFor(rate => rate).MustAsync(IsRateNameUniqueAsync).WithMessage("Rate name is duplicated");
        }

        private async Task<bool> IsRateNameUniqueAsync(ProjectRateModel rate, CancellationToken cancellationToken)
        {
            var allProjectRates = (await _projectRateService.GetProjectRatesAsync(rate.ProjectId)).ProjectRates;

            var sameNameRate = allProjectRates.FirstOrDefault(x => x.RateName.Equals(rate.RateName, StringComparison.CurrentCultureIgnoreCase));
            if (sameNameRate == null || sameNameRate.ProjectRateId == rate.ProjectRateId) { return true; }

            //exists same name rate with different rateId => not unique, (case insensitive)
            return false;
        }


    }
}
