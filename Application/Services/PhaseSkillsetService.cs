using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ForecastingSystem.Application.Services
{
    public class PhaseSkillsetService : IPhaseSkillsetService
    {
        private readonly IPhaseSkillsetRepository _phaseSkillsetRepository;
        private readonly IMapper _mapper;
        public PhaseSkillsetService(IMapper mapper, IPhaseSkillsetRepository phaseSkillsetRepository)
        {
            _mapper = mapper;
            _phaseSkillsetRepository = phaseSkillsetRepository;
        }

        public IList<PhaseSkillsetModelToView> GetPhaseSkillsets(int phaseId)
        {
            var phaseSkillsets = _phaseSkillsetRepository.Get(x => x.PhaseId == phaseId, y => y.Skillset);
            return _mapper.Map<IList<PhaseSkillsetModelToView>>(phaseSkillsets);
        }

        public PhaseSkillsetModel Add(PhaseSkillsetModel model)
        {
            if (!IsPhaseSkillsetUnique(model)) {
                throw new System.Exception($"Phase {model.PhaseId} already has Skillset {model.SkillsetId}.");
            }
            var entity = _mapper.Map<PhaseSkillset>(model);
            _phaseSkillsetRepository.Add(entity);
            _phaseSkillsetRepository.SaveChanges();
            model = _mapper.Map<PhaseSkillsetModel>(model);
            model.PhaseSkillSetId = entity.PhaseSkillSetId;
            return model;
        }

        public PhaseSkillsetModel Update(PhaseSkillsetModel model)
        {
            if (!IsPhaseSkillsetUnique(model))
            {
                throw new System.Exception($"Phase {model.PhaseId} already has Skillset {model.SkillsetId}.");
            }
            var entity = _mapper.Map<PhaseSkillset>(model);
            _phaseSkillsetRepository.Update(entity);
            _phaseSkillsetRepository.SaveChanges();
            model = _mapper.Map<PhaseSkillsetModel>(model);
            return model;
        }

        public bool Delete(int id)
        {
            var model = _phaseSkillsetRepository.GetById(id);

            if (model != null)
            {
                _phaseSkillsetRepository.Delete(model);
                _phaseSkillsetRepository.SaveChanges();
                return true;
            }
            return false;
        }

        private bool IsPhaseSkillsetUnique(PhaseSkillsetModel model)
        {
            return _phaseSkillsetRepository.Get(x => x.PhaseSkillSetId != model.PhaseSkillSetId && x.PhaseId == model.PhaseId && x.SkillsetId == model.SkillsetId).FirstOrDefault() == null;
        }
    }
}
