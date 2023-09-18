using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IPhaseResourceUtilisationRepository : IAsyncRepository<PhaseResourceUtilisation>
    {
        Task<List<PhaseResourceUtilisation>> GetPhaseResourceUtilisations(DateTime startDateTime);
    }
}
