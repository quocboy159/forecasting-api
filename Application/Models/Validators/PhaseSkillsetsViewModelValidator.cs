using FluentValidation;
using ForecastingSystem.Application.Interfaces;

namespace ForecastingSystem.Application.Models.Validators
{
    public class PhaseSkillsetsModelValidator : AbstractValidator<PhaseSkillsetModel>
    {
        private readonly ISkillsetService _skillsetService;
        public PhaseSkillsetsModelValidator(ISkillsetService skillsetService, int? parentPhaseId)
        {
            _skillsetService= skillsetService;

            RuleFor(model => model.PhaseId)
                    .Must(phaseId => 
                    // Add new Phase include Skillsets
                    (parentPhaseId == 0 && phaseId == 0) 
                    // Update Phase include Skillsets
                    || (parentPhaseId > 0 && phaseId > 0 && parentPhaseId == phaseId)
                    // Add/Update Skillsets
                    || (parentPhaseId == null && phaseId > 0))
                .WithMessage("PhaseId must be provided or the same to its parent phase");

            RuleFor(model => model.SkillsetId)
                    .Must(skillsetId => skillsetId > 0)
                .WithMessage("SkillsetId must be provided");

            RuleFor(model => model.SkillsetId).Must(_skillsetService.IsExistingSkillset)
                .WithMessage("SkillsetId does not exist");

        }
    }
}
