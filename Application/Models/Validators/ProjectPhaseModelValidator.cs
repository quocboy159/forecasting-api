using FluentValidation;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models.Validators
{
    public class ProjectPhaseModelValidator : AbstractValidator<ProjectPhaseModelToAdd>
    {
        private readonly IProjectPhaseService _projectPhaseService;
        private readonly IProjectService _projectService;
        private readonly ISkillsetService _skillsetService;
        string _calculatingByResourceMessage = "EndDate or Budget must be provided";

        public ProjectPhaseModelValidator(
            ISkillsetService skillsetService,
            IProjectPhaseService projectPhaseService, 
            IProjectService projectService, int phaseId)
        {
            _skillsetService = skillsetService;
            _projectPhaseService = projectPhaseService;
            _projectService = projectService;

            RuleFor(phase => phase.PhaseName).Must(name => !string.IsNullOrWhiteSpace(name) && name.Length <= 255)
           .WithMessage("Name must not be empty and not contain more than 255 characters.");

            RuleFor(phase => phase.ProjectId)
                .Must(projectId => projectId != 0)
            .WithMessage("ProjectId must be provided");

            RuleFor(phase => phase).Must(IsPhaseNameUnique).WithMessage("Phase name is duplicated");

            var phaseStatusesString = Enum.GetNames(typeof(PhaseStatus)).ToList();

            RuleFor(phase => phase.Status).Must(x => phaseStatusesString.Contains(x))
                .WithMessage($"Status must be one of { string.Join(",", phaseStatusesString)}");

            RuleFor(phase => phase.PhaseSkillsets).Must(IsNotSkillsetDuplicated).WithMessage("Skillsets are duplicated");
            RuleForEach(phase => phase.PhaseSkillsets).SetValidator(new PhaseSkillsetsModelValidator(skillsetService, phaseId));

            RuleFor(phase => phase.EndDate).Must(EndDate => EndDate != null).When(phase => phase.IsCalculatingByResource , ApplyConditionTo.CurrentValidator).WithMessage("EndDate must be provided");
            RuleFor(phase => phase.Budget).Must(budget => budget > 0).When(phase => !phase.IsCalculatingByResource , ApplyConditionTo.CurrentValidator).WithMessage("Budget must be provided");
        }


        private bool IsPhaseNameUnique(ProjectPhaseModelToAdd phase)
        {
            var allProjectPhases = _projectPhaseService.GetProjectPhases(phase.ProjectId).ProjectPhases;

            var sameNamePhase = allProjectPhases.FirstOrDefault(x => x.PhaseName.Equals(phase.PhaseName, StringComparison.CurrentCultureIgnoreCase));
            if (sameNamePhase == null || sameNamePhase.PhaseId == phase.PhaseId) { return true; }

            //exists same name phase with different phaseId => not unique, (case insensitive)
            return false;
        }

        private bool IsNotSkillsetDuplicated(IEnumerable<PhaseSkillsetModel> phaseSkillsets)
        {
            return phaseSkillsets == null || !phaseSkillsets.Any(x => phaseSkillsets.Where(y => y.SkillsetId == x.SkillsetId).Count() > 1);
        }
    }
}
