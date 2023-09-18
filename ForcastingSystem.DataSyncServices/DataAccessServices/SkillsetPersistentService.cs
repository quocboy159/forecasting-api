using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class SkillsetPersistentService : ISkillsetPersistentService
    {
        private readonly ISyncSkillsetRepository _skillsetRepository;
        private readonly ISyncSkillsetCategoryRepository _skillsetCategoryRepository;

        public SkillsetPersistentService(ISyncSkillsetRepository skillsetRepository , ISyncSkillsetCategoryRepository skillsetCategoryRepository)
        {
            _skillsetRepository = skillsetRepository;
            _skillsetCategoryRepository = skillsetCategoryRepository;
        }

        public void Save(Skillset[] skillsets)
        {
            try
            {
                int skillsetCategoryId = _skillsetCategoryRepository.CreateSkillsetCategoryIfNotExist(SkillsetCategories.Default);

                foreach (var skillset in skillsets)
                {
                    skillset.SkillsetCategoryId = skillsetCategoryId;
                    var existingSkillsetWithSameName = _skillsetRepository.FirstOrDefaultAsync(skill => skill.SkillsetName == skillset.SkillsetName).Result;
                    var existingSkillsetWithSameExternalId = _skillsetRepository.FirstOrDefaultAsync(skill => skill.ExternalId == skillset.ExternalId).Result;

                    if (existingSkillsetWithSameExternalId != null)
                    {
                        skillset.SkillsetId = existingSkillsetWithSameExternalId.SkillsetId;
                        //Update if data changed only
                        if (existingSkillsetWithSameExternalId.SkillsetName != skillset.SkillsetName)
                        {
                            _skillsetRepository.UpdateAsync(skillset).Wait();
                        }
                    }
                    else if (existingSkillsetWithSameName != null && existingSkillsetWithSameName.ExternalId == 0)
                    {
                        existingSkillsetWithSameName.ExternalId = skillset.ExternalId;
                        _skillsetRepository.UpdateAsync(existingSkillsetWithSameName).Wait();
                    }
                    else
                    {
                        _skillsetRepository.AddAsync(skillset).Wait();
                    }
                }

                _skillsetRepository.SaveChangesAsync().Wait();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw;
            }
            
        }
    }
}
