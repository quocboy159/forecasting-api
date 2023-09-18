using ForecastingSystem.Application.Models;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IClientService
    {
        Task<ClientListModel> GetClientsAsync();
        Task<bool> IsClientNameUniqueAsync(string clientName);
    }
}
