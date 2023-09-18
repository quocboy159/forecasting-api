using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class UserIdLookupRepository : GenericAsyncRepository<UserIdLookup>, IUserIdLookupRepository
    {
        public UserIdLookupRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<UserIdLookup>> GetByBambooHREmailsAsync(List<string> bambooHREmails)
        {
            var query = DbContext.UserIdLookups.AsNoTracking().Where(x => bambooHREmails.Contains(x.BambooHREmail));
            return await query.ToListAsync();
        }

        public async Task<UserIdLookup> GetByBambooEmailAsync(string email)
        {
            var record = await DbContext.UserIdLookups.AsNoTracking().Where(x => x.BambooHREmail == email).FirstOrDefaultAsync();
            return record;
        }
    }
}
