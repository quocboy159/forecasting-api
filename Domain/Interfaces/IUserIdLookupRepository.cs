using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IUserIdLookupRepository : IAsyncRepository<UserIdLookup>
    {
        Task<List<UserIdLookup>> GetByBambooHREmailsAsync(List<string> bambooHREmails);
        Task<UserIdLookup> GetByBambooEmailAsync(string email);
    }
}
