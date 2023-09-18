using ForecastingSystem.Domain.Models;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IClientRepository : IAsyncRepository<Client>
    {
        // This is where we put the methods specific for that class
        Task<bool> IsClientNameUniqueAsync(string clientName);
    }
}
