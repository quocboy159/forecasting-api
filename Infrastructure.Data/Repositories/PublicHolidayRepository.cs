using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class PublicHolidayRepository : GenericAsyncRepository<PublicHoliday>, IPublicHolidayRepository
    {
        public PublicHolidayRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<PublicHoliday>> GetPublicHolidaysAsync(DateTime? fromDate)
        {
            var daysQuery = DbContext.Set<PublicHoliday>().AsNoTracking();
            if (fromDate.HasValue)
            {
                daysQuery = daysQuery.Where(s => s.Date >= fromDate.Value.Date);
            }
            return daysQuery.ToListAsync();
        }
    }
}
