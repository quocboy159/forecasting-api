using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class PhaseSkillsetRepository : BaseRepository<PhaseSkillset>, IPhaseSkillsetRepository
    {
        public PhaseSkillsetRepository(ForecastingSystemDbContext dbContext) : 
            base(dbContext) {}

        public IEnumerable<PhaseSkillset> Get(Expression<Func<PhaseSkillset, bool>> predicate, Expression<Func<PhaseSkillset, object>> includde)
        {
            return _dbContext.Set<PhaseSkillset>().Include(includde).Where(predicate).ToList();
        }

        public async Task<IList<PhaseSkillset>> GetAsync(Expression<Func<PhaseSkillset, bool>> predicate, Expression<Func<PhaseSkillset, object>> includde)
        {
            return await _dbContext.Set<PhaseSkillset>().AsNoTracking().Include(includde).Where(predicate).ToListAsync();
        }
    }
}
