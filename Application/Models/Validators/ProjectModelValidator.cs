using FluentValidation;
using FluentValidation.Internal;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models.Validators
{
    public class ProjectModelValidator : AbstractValidator<ProjectDetailAddEditModel>
    {
        private readonly IProjectService _projectService;
        private readonly IClientService _clientService;
        private readonly IProjectRateService _projectRateService;
        private readonly IPhaseResourceService _phaseResourceService;

        public ProjectModelValidator(IProjectService projectService, IClientService clientService, IProjectRateService projectRateService, IPhaseResourceService phaseResourceService)
        {
            _projectService = projectService;
            _clientService = clientService;
            _projectRateService = projectRateService;
            _phaseResourceService = phaseResourceService;

            //ProjectId = 0 or existed
            RuleFor(p => p.ProjectId).MustAsync(IsExistedProjectIdAsync).WithMessage("Project Id You Want To Update Doesn't Exist").WhenAsync(async (x, ct) => await Task.FromResult(x.ProjectId != 0));

            //Name must not empty and not duplicate
            RuleFor(p => p.ProjectName).NotEmpty()
           .WithMessage("Name must not be empty");

            //ProjectType value must be Opportunity or Project
            RuleFor(p => p.ProjectType)
                .Must(pt => pt == Constants.ProjectType.Opportunity || pt == Constants.ProjectType.Project)
                .WithMessage($"ProjectType must be {Constants.ProjectType.Opportunity} or {Constants.ProjectType.Project}");

            RuleFor(project => project.ClientName).MustAsync(IsClientNameUniqueAsync).WithMessage("Client with same name existed").WhenAsync(async (x, ct) => await Task.FromResult(x.ClientId == 0));

            RuleForEach(x => x.Rates).Must(x => !string.IsNullOrWhiteSpace(x.RateName) && x.RateName.Length <= 50)
                .WithMessage("Rate name must not be empty and not contain more than 50 characters.");

            RuleFor(p => p).CustomAsync(async (x, c, t) => await IsPhaseResourceUsedByProjectRateIdAsync(x, c, t))
            .WhenAsync(async (x, ct) => await Task.FromResult(x.ProjectId != 0));


            RuleFor(project => project).Must(IsConfident100IfProjectCodeFilled).WithMessage("Confident must be 100% for project code.");

            RuleFor(p => p).Custom((x, c) => IsNotDupplicateRateName(x, c));
        }

        private async Task<bool> IsExistedProjectIdAsync(int projectId, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            return project != null;
        }

        private async Task<bool> IsClientNameUniqueAsync(string clientName, CancellationToken cancellationToken)
        {
            return await _clientService.IsClientNameUniqueAsync(clientName);
        }

        private async Task<bool> IsPhaseResourceUsedByProjectRateIdAsync(ProjectDetailAddEditModel project, ValidationContext<ProjectDetailAddEditModel> context, CancellationToken cancellationToken)
        {
            var projectRateIds = await _projectRateService.GetRateIdsByProjectIdAsync(project.ProjectId);
            if (projectRateIds == null || projectRateIds.Count == 0) return true;
            var newProjectRateIds = project.Rates.Where(x => x.ProjectRateId != 0).Select(x => x.ProjectRateId).ToList();

            var deletedProjectRateIds = projectRateIds.Where( x => !newProjectRateIds.Contains(x)).ToList();
            var rateNames = new List<string>();

            foreach (var projectRateId in deletedProjectRateIds)
            {
                bool isUsedInPhaseResouce = await _phaseResourceService.IsPhaseResourceUsedByProjectRateIdAsync(projectRateId);
                if (isUsedInPhaseResouce) 
                {
                    var rateName = await _projectRateService.GetRateNameById(projectRateId);
                    rateNames.Add(rateName);

                } 
            }

            if (rateNames.Count > 0) 
            {
                context.AddFailure($"The rate '{String.Join(", ", rateNames)}' cannot be deleted because it is currently being used in a resource assignment.");

                return false;
            }

            return true;
        }

        private bool IsConfident100IfProjectCodeFilled(ProjectDetailAddEditModel project)
        {
            if (!string.IsNullOrEmpty(project.ProjectCode) && project.Confident != 100)
            {
                return false;
            }

            return true;
        }

        private bool IsNotDupplicateRateName(ProjectDetailAddEditModel project, ValidationContext<ProjectDetailAddEditModel> context)
        {
            var dupplicateRateNames = project.Rates.GroupBy(x => x.RateName?.Trim()).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            if (dupplicateRateNames.Count > 0)
            {
                context.AddFailure($"Duplicated rate names '{String.Join(", ", dupplicateRateNames)}' is not allowed.");

                return false;
            }

            return true;
        }
    }
}
