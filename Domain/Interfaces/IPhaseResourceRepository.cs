using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IPhaseResourceRepository : IAsyncRepository<PhaseResource>
    {
        Task<IEnumerable<PhaseResource>> GetFullAsync(Expression<Func<PhaseResource, bool>> predicate);
        Task<PhaseResource> AddPhaseResourceAsync(PhaseResource phaseResource);
        Task<PhaseResource> EditPhaseResourceAsync(PhaseResource phaseResource);
        Task<bool> IsPhaseResourceUsedByProjectRateIdAsync(int projectRateId);
        Task<List<PhaseResourceView>> GetPhaseResourceUtilisations(DateTime startDateTime);       
    }
}
