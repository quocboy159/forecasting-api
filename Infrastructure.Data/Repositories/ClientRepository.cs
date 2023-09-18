using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class ClientRepository : GenericAsyncRepository<Client>, IClientRepository
    {
        public ClientRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
            /*
                This is the place where we create the logic for query,
                for saving and calling data for that entity.
             */
        }

        public async Task<bool> IsClientNameUniqueAsync(string clientName)
        {
            var hasRecord = await DbContext.Clients.Where(x => x.ClientName == clientName).AnyAsync();

            return hasRecord == false;
        }
    }
}
