using ForecastingSystem.Domain.Models;
using System;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IRateRepository : IAsyncRepository<Rate>
    {
        // This is where we put the methods specific for that class
    }
}
