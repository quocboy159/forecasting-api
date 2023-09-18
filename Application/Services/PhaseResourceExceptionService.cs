using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class PhaseResourceExceptionService : IPhaseResourceExceptionService
    {
        private readonly IProjectPhaseRepository _projectPhaseRepository;
        private readonly IPhaseResourceExceptionRepository _phaseResourceExceptionRepository;
        private readonly IPhaseResourceRepository _phaseResourceRepository;
        private readonly IMapper _mapper;
        private readonly IPhaseResourceUtilisationService _phaseResourceUtilisationService;
        public PhaseResourceExceptionService(
            IProjectPhaseRepository projectPhaseRepository,
            IPhaseResourceExceptionRepository phaseResourceExceptionRepository,
            IPhaseResourceRepository phaseResourceRepository,  IMapper mapper,
            IPhaseResourceUtilisationService phaseResourceUtilisationService)
        {
            _projectPhaseRepository = projectPhaseRepository;
            _mapper = mapper;
            _phaseResourceExceptionRepository = phaseResourceExceptionRepository;
            _phaseResourceRepository = phaseResourceRepository;
            _phaseResourceUtilisationService = phaseResourceUtilisationService;
        }

        public async Task<PhaseResourceExceptionModelToView> AddAsync(PhaseResourceExceptionModel phaseResourceExceptionRequest)
        {
            var phaseResourceException = _mapper.Map<PhaseResourceException>(phaseResourceExceptionRequest);          
            var addedPhaseResource = await _phaseResourceExceptionRepository.AddAsync(phaseResourceException);

            await _phaseResourceExceptionRepository.SaveChangesAsync();
            
            var model = await GetPhaseResourceExceptionByIdAsync(addedPhaseResource.PhaseResourceExceptionId);
            await _phaseResourceUtilisationService.GeneratePhaseResourceUtilisations(model.PhaseId);
            return model;
        }      

        public async Task<PhaseResourceExceptionModelToView> EditAsync(PhaseResourceExceptionModel phaseResourceExceptionRequest)
        {
            var phaseResourceException = _mapper.Map<PhaseResourceException>(phaseResourceExceptionRequest);          

            await _phaseResourceExceptionRepository.UpdateAsync(phaseResourceException);
            await _phaseResourceExceptionRepository.SaveChangesAsync();
            var model = await GetPhaseResourceExceptionByIdAsync(phaseResourceExceptionRequest.PhaseResourceExceptionId);
            await _phaseResourceUtilisationService.GeneratePhaseResourceUtilisations(model.PhaseId);
            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var phaseResourceException = await _phaseResourceExceptionRepository.GetByIdAsync(id);

            if (phaseResourceException != null)
            {
                await _phaseResourceExceptionRepository.DeleteAsync(phaseResourceException);
                await _phaseResourceExceptionRepository.SaveChangesAsync();
                
                var phaseResource = await _phaseResourceRepository.GetByIdAsync(phaseResourceException.PhaseResourceId);
                await _phaseResourceUtilisationService.GeneratePhaseResourceUtilisations(phaseResource.PhaseId);
                
                return true;
            }
            else
            {
                return false;
            }
        } 

        public async Task<PhaseResourceExceptionListModel> GetPhaseResourceExceptionsByPhaseIdAsync(int phaseId)
        {
            var phaseResourceExceptions = await _phaseResourceExceptionRepository.GetFullAsync(x => x.PhaseResource.PhaseId == phaseId);

            return new PhaseResourceExceptionListModel()
            {
                PhaseResourceExceptions = _mapper.Map<IEnumerable<PhaseResourceExceptionModelToView>>(phaseResourceExceptions)
            };
        }

        public async Task<PhaseResourceExceptionModelToView> GetPhaseResourceExceptionByIdAsync(int phaseResouceExceptionId)
        {
            var phaseResources = await _phaseResourceExceptionRepository.GetFullAsync(x => x.PhaseResourceExceptionId == phaseResouceExceptionId);
            if(phaseResources.FirstOrDefault() != null)
                return _mapper.Map<PhaseResourceExceptionModelToView>(phaseResources.First());
            return null;
        }
    }
}
