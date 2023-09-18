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
    public class SkillsetRepository : BaseRepository<Skillset>, ISkillsetRepository
    {
        public SkillsetRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
            /*
                This is the place where we create the logic for query,
                for saving and calling data for that entity.
             */
        }

        public new IReadOnlyList<Skillset> GetAll()
        {
            return _dbContext.Set<Skillset>().Include(x => x.SkillsetCategory).ToList();
        }

        public async Task<Skillset> GetSkillsetByIdAsync(int skillsetId)
        {
            return await _dbContext.Set<Skillset>().Include(x => x.SkillsetCategory).Where(x =>x.SkillsetId == skillsetId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsSkillsetNameUniqueAsync(string skillsetName)
        {
            var hasRecord = await DbContext.Set<Skillset>().Where(x => x.SkillsetName == skillsetName).AnyAsync();

            return hasRecord == false;
        }
    }
}
