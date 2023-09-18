using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class ProjectRateRepository : GenericAsyncRepository<ProjectRate>, IProjectRateRepository
    {
        public ProjectRateRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
            /*
                This is the place where we create the logic for query,
                for saving and calling data for that entity.
             */
        }

        public async Task<IList<ProjectRate>> GetProjectRatesFromResouceAssignmentsByPhaseId(int phaseId)
        {
            return await DbContext.PhaseResources.AsNoTracking()
                .Include(x => x.ProjectRate)
                .Where(x => x.PhaseId == phaseId)
                .Select(x => x.ProjectRate)
                .ToListAsync();
        }

        public async Task<IList<ProjectRate>> GetProjectRatesByExternalPhaseId(int externalPhaseId)
        {
            return await DbContext.ProjectRates.AsNoTracking()
                .Where(x => x.Project.Phases.Any(p => p.ExternalPhaseId == externalPhaseId))
                .ToListAsync();
        }

        public async Task<IList<ProjectRateHistory>> GetCurrentRatesAsync(IEnumerable<int> rateIds)
        {
            var records = await DbContext.ProjectRateHistories.AsNoTracking()
                                                             .Where(x => rateIds.Contains(x.ProjectRateId))
                                                             .OrderByDescending(x => x.StartDate)
                                                             .ThenByDescending(x => x.ProjectRateHistoryId)
                                                             .ToListAsync();
            var result = new List<ProjectRateHistory>();

            foreach(var rateId in rateIds)
            {
                // If there are no current rate, we show the nearest future date
                var rate = records.FirstOrDefault(x => x.ProjectRateId == rateId && x.StartDate.Value <= DateTime.UtcNow)
                    ?? records.FirstOrDefault(x => x.ProjectRateId == rateId && x.StartDate.Value > DateTime.UtcNow);
                if(rate != null)
                {
                    result.Add(rate);
                }
            }

            return result;
        }

        public async Task<ProjectRateHistory> GetCurrentRateAsync(int rateId)
        {
            var record = await DbContext.ProjectRateHistories.AsNoTracking()
                                                             .Where(x => x.ProjectRateId == rateId && x.StartDate.Value <= DateTime.UtcNow)
                                                             .OrderByDescending(x => x.StartDate)
                                                             .ThenByDescending(x => x.ProjectRateHistoryId)
                                                             .FirstOrDefaultAsync();

            // If there are no current rate, we show the nearest future date
            if (record == null)
            {
                record = await DbContext.ProjectRateHistories.AsNoTracking()
                                                             .Where(x => x.ProjectRateId == rateId && x.StartDate.Value > DateTime.UtcNow)
                                                             .OrderBy(x => x.StartDate)
                                                             .ThenByDescending(x => x.ProjectRateHistoryId)
                                                             .FirstOrDefaultAsync();
            }

            return record;
        }

        public async Task<ProjectRateHistory> GetMostRecentRateValueForSepecificDateAsync(int rateId, DateTime startDate)
        {
            var rate = await DbContext.ProjectRateHistories.AsNoTracking()
                                                           .Where(x => x.ProjectRateId == rateId && x.StartDate.Value == startDate)
                                                           .OrderByDescending(x => x.ProjectRateHistoryId).FirstOrDefaultAsync();
            return rate;
        }

        public async Task<List<ProjectRate>> GetRateHistoriesByProjectIdAsync(int projectId)
        {
            var result = await DbContext.ProjectRates.AsNoTracking()
                                                 .Include(x => x.ProjectRateHistories)
                                                 .Where(x => x.ProjectId == projectId && x.Status == ProjectRateStatus.Active)
                                                 .ToListAsync();

            foreach (var item in result)
            {
                item.ProjectRateHistories = item.ProjectRateHistories.OrderByDescending(x => x.StartDate).ThenByDescending(x => x.ProjectRateHistoryId).ToList();
            }

            return result;
        }

        public async Task<List<int>> GetRateIdsByProjectIdAsync(int projectId)
        {
            var result = await DbContext.ProjectRates.AsNoTracking()
                                                 .Where(x => x.ProjectId == projectId && x.Status == ProjectRateStatus.Active)
                                                 .Select(x => x.ProjectRateId)
                                                 .ToListAsync();

            return result;
        }

        public async Task<string> GetRateNameById(int projectRateId)
        {
            var result = await DbContext.ProjectRates.AsNoTracking()
                                                 .Where(x => x.ProjectRateId == projectRateId && x.Status == ProjectRateStatus.Active)
                                                 .Select(x => x.RateName)
                                                 .FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> IsActiveProjectRateAsync(int projectRateId)
        {
            var result = await DbContext.ProjectRates.AsNoTracking()
                                     .Where(x => x.ProjectRateId == projectRateId && x.Status == ProjectRateStatus.Active)
                                     .AnyAsync();

            return result;
        }

        public async Task<IList<ProjectRate>> GetOpportunityActiveProjectRatesByProjectCode(string projectCode)
        {
            var result = await DbContext.ProjectRates.AsNoTracking()
                                        .Where(x => x.Project.ProjectCode.Equals(projectCode)
                                          && x.Project.ProjectType == ProjectType.Opportunity
                                          && x.Status == ProjectRateStatus.Active)
                                        .ToListAsync();
            return result;
        }
    }
}
