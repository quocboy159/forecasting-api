using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class SkillsetCategoryRepository : BaseRepository<SkillsetCategory>, ISkillsetCategoryRepository
    {
        public SkillsetCategoryRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
            /*
                This is the place where we create the logic for query,
                for saving and calling data for that entity.
             */
        }


        public async Task<int?> GetSkillsetCategoryIdByNameAsync(string skillsetCategoryName)
        {
            var id = await DbContext.Set<SkillsetCategory>().Where(x => x.CategoryName.ToLower() == skillsetCategoryName.ToLower()).Select(x => (int?)x.SkillsetCategoryId).FirstOrDefaultAsync();

            return id;
        }
    }
}
