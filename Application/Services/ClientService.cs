using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        public ClientService(IClientRepository clientRepository , IMapper mapper)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<ClientListModel> GetClientsAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            var clientListViewModel = _mapper.Map<IEnumerable<ClientModel>>(clients);

            return new ClientListModel()
            {
                Clients = clientListViewModel
            };
        }

        public async Task<bool> IsClientNameUniqueAsync(string clientName)
        {
            return await _clientRepository.IsClientNameUniqueAsync(clientName);
        }
    }
}
