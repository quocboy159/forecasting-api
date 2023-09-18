using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ForecastingSystem.Domain.Common;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class PhaseResourceRepository : GenericAsyncRepository<PhaseResource>, IPhaseResourceRepository
    {
        private readonly IResourcePlaceHolderRepository _resourcePlaceHolderRepository;
        public PhaseResourceRepository(ForecastingSystemDbContext dbContext,
            IResourcePlaceHolderRepository resourcePlaceHolderRepository
            ) : base(dbContext)
        {
            _resourcePlaceHolderRepository = resourcePlaceHolderRepository;
        }

        public async Task<IEnumerable<PhaseResource>> GetFullAsync(Expression<Func<PhaseResource, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Include(t => t.Employee)
                .Include(t => t.Phase)
                .Include(t => t.ResourcePlaceHolder)
                .Include(t => t.ProjectRate)
                .Include(t => t.PhaseResourceExceptions)
                .Where(predicate).ToListAsync();
        }

        public async Task<PhaseResource> AddPhaseResourceAsync(PhaseResource phaseResource)
        {
            using IDbContextTransaction transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                if (phaseResource.ResourcePlaceHolder != null)
                {
                    await _resourcePlaceHolderRepository.AddAsync(phaseResource.ResourcePlaceHolder);
                }
                await DbSet.AddAsync(phaseResource);
                await SaveChangesAsync();
                if (phaseResource.ResourcePlaceHolder != null)
                    phaseResource.ResourcePlaceHolderId = phaseResource.ResourcePlaceHolder.ResourcePlaceHolderId;
                else phaseResource.ResourcePlaceHolderId = null;
                DbSet.Update(phaseResource);
                await SaveChangesAsync();
                await transaction.CommitAsync();
                return phaseResource;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PhaseResource> EditPhaseResourceAsync(PhaseResource phaseResource)
        {
            using IDbContextTransaction transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                if (phaseResource.ResourcePlaceHolder == null && phaseResource.ResourcePlaceHolderId.HasValue)
                {
                    // Delete existing ResourcePlaceHolderName cause use Employee
                    var entity = await _resourcePlaceHolderRepository.GetByIdAsync(phaseResource.ResourcePlaceHolderId.Value);
                    await _resourcePlaceHolderRepository.DeleteAsync(entity);
                    phaseResource.ResourcePlaceHolderId = null;
                }
                else if (phaseResource.ResourcePlaceHolder != null && phaseResource.ResourcePlaceHolderId.HasValue && phaseResource.ResourcePlaceHolderId.Value > 0)
                {
                    // Upddate existing ResourcePlaceHolderName
                    phaseResource.ResourcePlaceHolder.ResourcePlaceHolderId = phaseResource.ResourcePlaceHolderId.Value;
                }
                else if (phaseResource.ResourcePlaceHolder != null && (phaseResource.ResourcePlaceHolderId == null || phaseResource.ResourcePlaceHolderId == 0))
                {
                    // Add new ResourcePlaceHolder
                    phaseResource.EmployeeId = null;
                    await _resourcePlaceHolderRepository.AddAsync(phaseResource.ResourcePlaceHolder);
                }
                
                await UpdateAsync(phaseResource);
                await SaveChangesAsync();
                if (phaseResource.ResourcePlaceHolder != null)
                    phaseResource.ResourcePlaceHolderId = phaseResource.ResourcePlaceHolder.ResourcePlaceHolderId;
                else phaseResource.ResourcePlaceHolderId = null;
                await UpdateAsync(phaseResource);
                await SaveChangesAsync();

                await transaction.CommitAsync();
                return phaseResource;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> IsPhaseResourceUsedByProjectRateIdAsync(int projectRateId)
        {
            var phaseIds = await DbContext.PhaseResources.Where(x => x.ProjectRateId == projectRateId).Select(x => x.PhaseId).ToListAsync();
            var result = await DbContext.Phases.Where( x => phaseIds.Contains(x.PhaseId) && x.Status == PhaseStatus.Active).AnyAsync();   

            return result;
        }

        public async Task<List<PhaseResourceView>> GetPhaseResourceUtilisations(DateTime startDateTime)
        {
            var entryQuery = DbContext.PhaseResources.AsNoTracking()
            .Include(s=> s.Employee)
            .Include(s=> s.ResourcePlaceHolder)
            .Include(s=> s.Phase.Project)          
            .AsQueryable();
            // exclude the Opportunity linked to project, Status must be Active
            entryQuery = entryQuery.Where(s => (s.Phase.Project.Status == ProjectStatus.Active) 
                                               && !(s.Phase.Project.ProjectType == Constants.ProjectType.Opportunity  
                                                        && !string.IsNullOrEmpty(s.Phase.Project.ProjectCode)));
            entryQuery = entryQuery.Where(s => s.Phase.Status == PhaseStatus.Active);
            entryQuery = entryQuery.Where(s => s.PhaseResourceUtilisations.Any(x => x.StartWeek >= startDateTime));          

            var result = await entryQuery.Select(s=> new PhaseResourceView { 
                EmployeeId = s.EmployeeId,
                Username = s.Employee != null ? s.Employee.UserName : null,
                Country = s.Employee != null ? s.Employee.Country : null,
                ResourcePlaceHolderId = s.ResourcePlaceHolderId,
                ResourcePlaceHolderName = s.ResourcePlaceHolder != null ? s.ResourcePlaceHolder.ResourcePlaceHolderName : null,
                PhaseId = s.PhaseId,
                PhaseName = s.Phase.PhaseName,
                ProjectId = s.Phase.ProjectId,
                ExternalProjectId = s.Phase.Project.ExternalProjectId,
                ProjectName = s.Phase.Project.ProjectName,
                ProjectCode = s.Phase.Project.ProjectCode,
                ProjectType = s.Phase.Project.ProjectType,
                ClientName = s.Phase.Project.Client.ClientName,
                FTE = s.FTE,
                PhaseResourceId = s.PhaseResourceId,
                PhaseResourceUtilisations = s.PhaseResourceUtilisations.ToList()
            }).ToListAsync();
            return result;
        }
    }
}
