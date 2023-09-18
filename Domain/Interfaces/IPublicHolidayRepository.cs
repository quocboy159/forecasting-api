using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IPublicHolidayRepository : IAsyncRepository<PublicHoliday>
    {
        Task<List<PublicHoliday>> GetPublicHolidaysAsync(DateTime? fromDate);
    }
}
