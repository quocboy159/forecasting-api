using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class ProjectPhaseRepository : GenericAsyncRepository<Phase>, IProjectPhaseRepository
    {
        public ProjectPhaseRepository(ForecastingSystemDbContext dbContext) 
            : base(dbContext) {}

        public async Task<Phase> GetProjectPhaseByExternalPhaseId(int externalPhaseId)
        {
            return await DbContext.Phases.AsNoTracking().FirstOrDefaultAsync(x => x.Status == PhaseStatus.Active && x.ExternalPhaseId == externalPhaseId);
        }

        public Phase GetPhaseToCalculateRevenueByTimeSheetPhaseId(int timesheetPhaseId)
        {
            var query = DbContext.Phases.AsNoTracking()
                                                .Include(x => x.PhaseResources).ThenInclude(x => x.PhaseResourceExceptions)
                                                .Include(x => x.PhaseResources).ThenInclude(x => x.Employee)
                                                .Include(x => x.PhaseResources).ThenInclude(x => x.ResourcePlaceHolder)
                                                .Include(x => x.PhaseResources).ThenInclude(x => x.ProjectRate).ThenInclude(x => x.ProjectRateHistories)
                                                .AsQueryable();

            query = query.Where(x => x.Status == PhaseStatus.Active && x.TimesheetPhaseId == timesheetPhaseId);

            var phase = query.FirstOrDefault();
            return phase;
        }

        public async Task<bool> CheckExistResouceAssignmentsByExtenalPhaseId(int extenalPhaseId)
        {
            var phase = await DbContext.Phases.AsNoTracking()
                                                  .Include(x => x.PhaseResources)
                                                  .FirstOrDefaultAsync(x => x.Status == PhaseStatus.Active && x.ExternalPhaseId == extenalPhaseId && x.Project.ProjectType == Constants.ProjectType.Project);
            if (phase == null)
            {
                return false;
            }

            var result = phase.PhaseResources.Any();
            return result;
        }

        public async Task<ICollection<Phase>> GetListByProjectIdAsync(int projectId)
        {
            return await DbContext.Phases.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<Phase> GetProjectPhaseByIdAsync(int id)
        {
            return await DbContext.Phases.Include(x => x.PhaseSkillsets).ThenInclude(t => t.Skillset).FirstOrDefaultAsync(x => x.PhaseId == id);
        }

        public void DeleteSkillsets(IEnumerable<PhaseSkillset> phaseSkillsets)
        {
            phaseSkillsets.ToList().ForEach(x => DbContext.Entry(x).State = EntityState.Deleted);
        }

        public async Task<Phase> GetPhaseToCalculateRevenue(int phaseId)
        {
            var query = DbContext.Phases.AsNoTracking()
                                                 .Include(x => x.Project)
                                                 .Include(x => x.PhaseResources).ThenInclude(x => x.PhaseResourceExceptions)
                                                 .Include(x => x.PhaseResources).ThenInclude(x => x.Employee)
                                                 .Include(x => x.PhaseResources).ThenInclude(x => x.ResourcePlaceHolder)
                                                 .Include(x => x.PhaseResources).ThenInclude(x => x.ProjectRate).ThenInclude(x => x.ProjectRateHistories)
                                                 .AsQueryable();
            
            query = query.Where(x => x.PhaseId == phaseId);
            
            var phase = await query.FirstOrDefaultAsync();
            return phase;
        }

        public async Task<List<KeyValuePair<string, int>>> GetTimesheetPhaseIdListByProjectCodeAsync(string projectCode)
        {
            var projectId = await DbContext.Projects.AsNoTracking().Where(x => x.ProjectCode == projectCode
                                                                               && x.ProjectType == Constants.ProjectType.Project
                                                                               && x.Status == ProjectStatus.Active).Select(x => x.ProjectId).FirstAsync();

            var result = await DbContext.Phases.AsNoTracking()
                                               .Where(x => x.ProjectId == projectId && x.ExternalPhaseId.HasValue && x.Status == PhaseStatus.Active)
                                               .Select(x => new
                                               {
                                                   PhaseName = x.PhaseName,
                                                   PhaseCode = x.PhaseCode,
                                                   ExternalPhaseId = (int)x.ExternalPhaseId
                                               })
                                               .ToListAsync();

            return result.Select(x => new KeyValuePair<string, int>($"{x.PhaseCode} - {x.PhaseName}", x.ExternalPhaseId))
                         .OrderBy(x => x.Key)
                         .ToList();
        }

        public async Task<string> GetTimesheetPhaseCodeNameAsync(int timesheetPhaseId)
        {
            string name = null;

            var result = await DbContext.Phases.AsNoTracking()
                                   .Where(x => x.ExternalPhaseId == timesheetPhaseId)
                                   .Select(x => new
                                   {
                                       PhaseName = x.PhaseName,
                                       PhaseCode = x.PhaseCode,
                                   }).FirstOrDefaultAsync();


            if (result != null)
            {
                name = $"{result.PhaseCode} - {result.PhaseName}";
            }

            return name;
        }

        public async Task<List<PhaseResource>> GetPhaseResourcesToCalculateUtilisation(int phaseId)
        {
            var result = await DbContext.PhaseResources.AsNoTracking()
                .Include(s => s.Phase.Project)
                .Include(s => s.Employee)
                .Include(s => s.ResourcePlaceHolder)
                .Include(s=> s.PhaseResourceExceptions)
                .Where(s => s.PhaseId == phaseId)
                .ToListAsync();
            return result;
        }

        public async Task UpdatePhaseValues(int phaseId , decimal phaseValue , decimal? budget , DateTime? estimatedEndDate)
        {
            string sql = "UPDATE Phase SET    " +
                "           Budget = @budget,    " +
                "           PhaseValue = @phaseValue,   " +
                "           EstimatedEndDate = @estimatedEndDate   " +
                "       WHERE PhaseId = @phaseId";

            var phaseBudgetParam = new SqlParameter("@budget" , budget);
            if (phaseBudgetParam.Value == null)
            {
                phaseBudgetParam.Value = DBNull.Value;
            }
            var phaseValueParam =  new SqlParameter("@phaseValue" , phaseValue);
            var estimatedEndDateParam = new SqlParameter("@estimatedEndDate" , estimatedEndDate);
            if (estimatedEndDateParam.Value == null)
            {
                estimatedEndDateParam.Value = DBNull.Value;
            }
            var phaseIdParam = new SqlParameter("@phaseId" , phaseId);

            int rowsAffected = await DbContext.Database.ExecuteSqlRawAsync(sql , phaseBudgetParam , phaseValueParam , estimatedEndDateParam , phaseIdParam);
        }

        public async Task<int?> GetPhaseIdFromProjectPhaseId(int projectPhaseId)
        {
            var projectPhase = await DbContext.Phases.AsNoTracking().FirstOrDefaultAsync(x => x.PhaseId == projectPhaseId);
            if(projectPhase == null || projectPhase?.ExternalPhaseId == null)
            {
                return null;
            }

            return (await DbContext.Phases.AsNoTracking().FirstOrDefaultAsync(x => x.TimesheetPhaseId == projectPhase.ExternalPhaseId))?.PhaseId;
        }

        public async Task<Phase> GetLinkedOpportunityFromProjectPhaseId(int projectPhaseId)
        {
            var projectPhase = await DbContext.Phases.AsNoTracking().FirstOrDefaultAsync(x => x.Status == PhaseStatus.Active && x.PhaseId == projectPhaseId);
            if (projectPhase == null || projectPhase?.ExternalPhaseId == null)
            {
                return null;
            }

            return await DbContext.Phases.AsNoTracking().FirstOrDefaultAsync(x => x.TimesheetPhaseId == projectPhase.ExternalPhaseId);
        }
    }
}
