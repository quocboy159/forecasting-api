using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IRateService
    {
        Task<IEnumerable<RateModel>> GetDefaultRatesAsync();
    }
}
