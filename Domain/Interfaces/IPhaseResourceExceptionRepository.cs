using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IPhaseResourceExceptionRepository : IAsyncRepository<PhaseResourceException>
    {
        IList<PhaseResourceException> GetPhaseResourceExceptions(int phaseId);
        Task<IEnumerable<PhaseResourceException>> GetFullAsync(Expression<Func<PhaseResourceException, bool>> predicate);
    }
}
