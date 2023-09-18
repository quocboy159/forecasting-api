using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.SyncDataAccess.Repositories
{
    public class SyncSkillsetCategoryRepository : SyncGenericAsyncRepository<SkillsetCategory>, ISyncSkillsetCategoryRepository
    {
        public SyncSkillsetCategoryRepository(SyncForecastingSystemDbContext dbContext) : base(dbContext)
        {
            /*
                This is the place where we create the logic for query,
                for saving and calling data for that entity.
             */
        }

        public int CreateSkillsetCategoryIfNotExist(string skillsetCategoryName)
        {
            int? existedId = GetSkillsetCategoryIdByNameAsync(skillsetCategoryName);
            if (existedId.HasValue)
            {
                return existedId.Value;
            }
            else
            {
                var newCategory = base.AddAsync(new SkillsetCategory { CategoryName = skillsetCategoryName }).Result;
                base.SaveChangesAsync().Wait();
                return newCategory.SkillsetCategoryId;
            }
        }

        private int? GetSkillsetCategoryIdByNameAsync(string skillsetCategoryName)
        {
            var id = DbContext.Set<SkillsetCategory>()
                .Where(x => x.CategoryName.ToLower() == skillsetCategoryName.ToLower())
                .Select(x => (int?)x.SkillsetCategoryId)
                .FirstOrDefault();

            return id;
        }
    }
}
