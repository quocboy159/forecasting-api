using FluentValidation;
using ForecastingSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models.Validators
{
    public class SkillsetModelValidator : AbstractValidator<SkillsetAddModel>
    {
        private readonly ISkillsetService _skillsetService;

        public SkillsetModelValidator(ISkillsetService skillsetService)
        {
            _skillsetService = skillsetService;

            RuleFor(x => x.SkillsetName).Must(x => !string.IsNullOrWhiteSpace(x) && x.Length <= 255)
                .WithMessage("Skillset name must not be empty and not contain more than 255 characters.");

            RuleFor(project => project.SkillsetName).MustAsync(IsSkillsetNameUniqueAsync).WithMessage("Skillset with same name existed");
        }

        private async Task<bool> IsSkillsetNameUniqueAsync(string skillsetName, CancellationToken cancellationToken)
        {
            return await _skillsetService.IsSkillsetNameUniqueAsync(skillsetName);
        }
    }
}
