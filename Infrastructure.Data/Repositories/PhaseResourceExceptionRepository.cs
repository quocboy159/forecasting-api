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
    public class PhaseResourceExceptionRepository : GenericAsyncRepository<PhaseResourceException>, IPhaseResourceExceptionRepository
    {
        public PhaseResourceExceptionRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
            /*
                This is the place where we create the logic for query,
                for saving and calling data for that entity.
             */
        }

        public IList<PhaseResourceException> GetPhaseResourceExceptions(int phaseId)
        {
            var phaseResourceExceptions = DbContext.PhaseResources.Where(x => x.PhaseId == phaseId).SelectMany(x => x.PhaseResourceExceptions).AsNoTracking().ToList();
            return phaseResourceExceptions;
        }

        public async Task<IEnumerable<PhaseResourceException>> GetFullAsync(Expression<Func<PhaseResourceException, bool>> predicate)
        {
            return await DbSet.AsNoTracking()
                .Include(t => t.PhaseResource).ThenInclude(t => t.Employee)
                .Include(t => t.PhaseResource).ThenInclude(t => t.ProjectRate)
                .Where(predicate).ToListAsync();
        }
    }
}
