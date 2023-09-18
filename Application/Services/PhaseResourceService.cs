using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class PhaseResourceService : IPhaseResourceService
    {
        private readonly IProjectPhaseRepository _projectPhaseRepository;
        private readonly IPhaseResourceRepository _phaseResourceRepository;
        private readonly IResourcePlaceHolderRepository _resourcePlaceHolderRepository;
        private readonly IMapper _mapper;
        private readonly IPhaseResourceUtilisationService _phaseResourceUtilisationService;
        public PhaseResourceService(
            IProjectPhaseRepository projectPhaseRepository,
            IPhaseResourceRepository phaseResourceRepository,
            IResourcePlaceHolderRepository resourcePlaceHolderRepository,
            IMapper mapper,
            IPhaseResourceUtilisationService phaseResourceUtilisationService)
        {
            _projectPhaseRepository = projectPhaseRepository;
            _mapper = mapper;
            _phaseResourceRepository = phaseResourceRepository;
            _resourcePlaceHolderRepository = resourcePlaceHolderRepository;
            _phaseResourceUtilisationService = phaseResourceUtilisationService;
        }

        public async Task<PhaseResourceModelToView> AddAsync(PhaseResourceModel phaseResourceRequest)
        {
            var phaseResource = _mapper.Map<PhaseResource>(phaseResourceRequest);
            var addedPhaseResource = await _phaseResourceRepository.AddPhaseResourceAsync(phaseResource);
            await _phaseResourceUtilisationService.GeneratePhaseResourceUtilisations(phaseResource.PhaseId);
            return await GetPhaseResourceByIdAsync(addedPhaseResource.PhaseResourceId);
        }      

        public async Task<PhaseResourceModel> EditAsync(PhaseResourceModel phaseResourceRequest)
        {
            var phaseResource = _mapper.Map<PhaseResource>(phaseResourceRequest);
            await _phaseResourceRepository.EditPhaseResourceAsync(phaseResource);
            await _phaseResourceUtilisationService.GeneratePhaseResourceUtilisations(phaseResource.PhaseId);
            return await GetPhaseResourceByIdAsync(phaseResourceRequest.PhaseResourceId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var phaseResource = await _phaseResourceRepository.GetByIdAsync(id);

            if (phaseResource != null)
            {
                if (phaseResource.ResourcePlaceHolderId > 0)
                {
                    // Delete ResourcePlaceHolder
                    var entity = await _resourcePlaceHolderRepository.GetByIdAsync(phaseResource.ResourcePlaceHolderId.Value);
                    await _resourcePlaceHolderRepository.DeleteAsync(entity);
                }
                await _phaseResourceRepository.DeleteAsync(phaseResource);
                await _phaseResourceRepository.SaveChangesAsync();
                await _phaseResourceUtilisationService.GeneratePhaseResourceUtilisations(phaseResource.PhaseId);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PhaseResourceModelToView> GetPhaseResourceByIdAsync(int id)
        {
            var phaseResource = await _phaseResourceRepository.GetFullAsync(x => x.PhaseResourceId == id);
            if (phaseResource.FirstOrDefault() != null)
                return _mapper.Map<PhaseResourceModelToView>(phaseResource.First());
            return null;
        }

        public async Task<PhaseResourceListModel> GetPhaseResourcesByPhaseIdAsync(int phaseId)
        {
            var phaseResources = await _phaseResourceRepository.GetFullAsync(x => x.PhaseId == phaseId);
            var phaseResourceViews = _mapper.Map<IEnumerable<PhaseResourceModelToView>>(phaseResources);
            
            return new PhaseResourceListModel()
            {
                PhaseResources = phaseResourceViews
            };
        }

        public async Task<PhaseResourceListModel> GetEmployeePhaseResourcesByPhaseIdAsync(int phaseId)
        {
            var phaseResources = await _phaseResourceRepository.GetFullAsync(x => x.PhaseId == phaseId && x.EmployeeId.HasValue);
            var phaseResourceViews = _mapper.Map<IEnumerable<PhaseResourceModelToView>>(phaseResources);
            phaseResourceViews.ToList().ForEach(x => x.NameWithRate = $"{x.FullName} - {x.ProjectRateName}");

            return new PhaseResourceListModel()
            {
                PhaseResources = phaseResourceViews
            };
        }

        public async Task<bool> IsPhaseResourceUsedByProjectRateIdAsync(int projectRateId)
        {
            return await _phaseResourceRepository.IsPhaseResourceUsedByProjectRateIdAsync(projectRateId);
        }

        public async Task<PhaseResourceListModel> GetEmployeePhaseResourcesByProjectPhaseIdAsync(int projectPhaseId)
        {
            var phaseId = await _projectPhaseRepository.GetPhaseIdFromProjectPhaseId(projectPhaseId);
            if(phaseId == null)
            {
                return new PhaseResourceListModel(); ;
            }

            return await GetEmployeePhaseResourcesByPhaseIdAsync(phaseId.Value);
        }
    }
}
