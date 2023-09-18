using FluentValidation;
using ForecastingSystem.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models.Validators
{
    public class PhaseResourceExceptionModelValidator : AbstractValidator<PhaseResourceExceptionModel>
    {
        private readonly IPhaseResourceService _phaseResourceService;
        private readonly IPhaseResourceExceptionService _phaseResourceExceptionService;
        public PhaseResourceExceptionModelValidator(IPhaseResourceExceptionService phaseResourceExceptionService
            , IPhaseResourceService phaseResourceService)
        {
            _phaseResourceExceptionService = phaseResourceExceptionService;
            _phaseResourceService = phaseResourceService;

            RuleFor(phase => phase)
                .MustAsync(MustProvideValidPhaseResourceIdAsync)
                .WithMessage("PhaseResourceId must be provided and valid")
                .DependentRules(() => {
                    RuleFor(phase => phase)
                    .MustAsync(TimeMustNotOverlapAsync)
                    .WithMessage("StartWeek and NumberOfWeeks must not be overlapped");
                });

            RuleFor(phase => phase.NumberOfWeeks)
                .Must(Weeks => Weeks > 0)
                .WithMessage("NumberOfWeeks must be an integer and greater than 0");

        }

        private async Task<bool> TimeMustNotOverlapAsync(PhaseResourceExceptionModel model, CancellationToken arg2)
        {
            var phaseResource = await _phaseResourceService.GetPhaseResourceByIdAsync(model.PhaseResourceId);
            var phaseResourceExceptions = await _phaseResourceExceptionService.GetPhaseResourceExceptionsByPhaseIdAsync(phaseResource.PhaseId);
            var otherPhaseResourceExceptions = phaseResourceExceptions.PhaseResourceExceptions.Where(x => x.PhaseResourceId == model.PhaseResourceId && x.PhaseResourceExceptionId != model.PhaseResourceExceptionId);
            var result = true;
            otherPhaseResourceExceptions.ToList().ForEach(x => {
                var otherStartDate = x.StartWeek;
                var otherEndDate = x.StartWeek.AddDays(x.NumberOfWeeks * 7).AddSeconds(-1);
                var endDate = model.StartWeek.AddDays(model.NumberOfWeeks * 7).AddSeconds(-1);
                var isValid = (model.StartWeek < otherStartDate && endDate < otherStartDate)
                    || (model.StartWeek > otherEndDate);
                if (!isValid)
                {
                    result = false;
                    return;
                }
            });
            return result;
        }

        private async Task<bool> MustProvideValidPhaseResourceIdAsync(PhaseResourceExceptionModel model, CancellationToken arg2)
        {
            var existingItem = await _phaseResourceExceptionService.GetPhaseResourceExceptionByIdAsync(model.PhaseResourceExceptionId);
            if (existingItem != null)
            {
                return model.PhaseResourceId == existingItem.PhaseResourceId;
            }
            else if(model.PhaseResourceExceptionId == 0)
            {
                var existingPhaseResource = await _phaseResourceService.GetPhaseResourceByIdAsync(model.PhaseResourceId);
                return  existingPhaseResource != null && existingPhaseResource.EmployeeId != null;
            }
            return false;
        }
    }
}
