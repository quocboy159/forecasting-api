using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class PhaseResourceUtilisationRepository : GenericAsyncRepository<PhaseResourceUtilisation>, IPhaseResourceUtilisationRepository
    {
        public PhaseResourceUtilisationRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
        }
        public Task<List<PhaseResourceUtilisation>> GetPhaseResourceUtilisations(DateTime startDateTime)
        {
            throw new Exception();
            //var entryQuery = DbContext.Set<PhaseResourceUtilisation>().AsNoTracking();
            //    //.Include(s=> s.PhaseResourceUtilisationByWeeks).AsQueryable();
            //entryQuery = entryQuery.Where(s => s.PhaseResourceUtilisationByWeeks.Any(x => x.StartWeek >= startDateTime));
            //return entryQuery.ToListAsync();
        }
    }
}
