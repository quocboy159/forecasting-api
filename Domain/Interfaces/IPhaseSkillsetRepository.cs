using ForecastingSystem.Domain.Interfaces.Base;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IPhaseSkillsetRepository : IBaseRepository<PhaseSkillset>
    {
        IEnumerable<PhaseSkillset> Get(Expression<Func<PhaseSkillset, bool>> predicate, Expression<Func<PhaseSkillset, object>> includde);
        Task<IList<PhaseSkillset>> GetAsync(Expression<Func<PhaseSkillset, bool>> predicate, Expression<Func<PhaseSkillset, object>> includde);
    }
}
