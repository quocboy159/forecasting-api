using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class SkillsetService : ISkillsetService
    {
        private readonly ISkillsetRepository _skillsetRepository;
        private readonly ISkillsetCategoryRepository _skillsetCategoryRepository;
        private readonly IMapper _mapper;
        public SkillsetService(IMapper mapper, ISkillsetRepository skillsetRepository, ISkillsetCategoryRepository skillsetCategoryRepository)
        {
            _mapper = mapper;
            _skillsetRepository = skillsetRepository;
            _skillsetCategoryRepository = skillsetCategoryRepository;
        }
        public IList<SkillsetModel> GetSkillsets()
        {
            var skillsets = _skillsetRepository.GetAll();
            return _mapper.Map<IList<SkillsetModel>>(skillsets);
        }

        public bool IsExistingSkillset(int id)
        {
            return _skillsetRepository.GetById(id) != null;
        }

        public async Task<SkillsetModel> AddAsync(SkillsetModel skillsetModel)
        {
            var skillset = _mapper.Map<Skillset>(skillsetModel);

            var skillsetCategoryId = await _skillsetCategoryRepository.GetSkillsetCategoryIdByNameAsync(skillsetModel.SkillsetCategoryName);

            if (skillsetCategoryId.HasValue)
            {
                skillset.SkillsetCategory = _skillsetCategoryRepository.GetById(skillsetCategoryId.Value);
            }
            else
            {
                skillset.SkillsetCategory = new SkillsetCategory
                {
                    CategoryName = skillsetModel.SkillsetCategoryName
                };
            }

            _skillsetRepository.Add(skillset);
            _skillsetRepository.SaveChanges();

            var result = await _skillsetRepository.GetSkillsetByIdAsync(skillset.SkillsetId);
            return _mapper.Map<SkillsetModel>(result);
        }

        public async Task<bool> IsSkillsetNameUniqueAsync(string skillsetName)
        {
            return await _skillsetRepository.IsSkillsetNameUniqueAsync(skillsetName);
        }
    }
}
