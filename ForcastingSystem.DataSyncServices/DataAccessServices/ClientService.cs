using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.CustomExceptions;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System.Runtime.CompilerServices;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class ClientService : IClientService
    {
        private readonly ISyncClientRepository _clientRepository;
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly IMapper _mapper;
        public ClientService(
            ISyncClientRepository clientRepository
            , IDataSyncServiceLogger dataSyncLogger
            , IMapper mapper) {
            _clientRepository = clientRepository;
            _dataSyncLogger = dataSyncLogger;
            _mapper = mapper;
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<Outbound.Client> tsClients, CancellationToken cancellationToken)
        {
            try
            {
                var allClients = await _clientRepository.GetAllAsync(cancellationToken);
                var existingClients = allClients.Where(x => x.ExternalClientId.HasValue && tsClients.Select(y => y.OrganizationID).ToList().Contains(x.ExternalClientId ?? 0));
                var updatedTsClients = tsClients.Where(x => existingClients.Select(y => y.ExternalClientId).Contains(x.OrganizationID));
                var addedTsClients = tsClients.Except(updatedTsClients);
                var duplicatedNameClients = new List<Outbound.Client>();
                
                addedTsClients.ToList().ForEach(x =>
                {
                    var t = addedTsClients.Where(y => y.OrganizationName == x.OrganizationName);
                    if (addedTsClients.Where(y => y.OrganizationName == x.OrganizationName).Count() > 1
                    && !duplicatedNameClients.Select(l => l.OrganizationName).Contains(x.OrganizationName))
                        duplicatedNameClients.Add(x);
                });

                duplicatedNameClients.AddRange(addedTsClients.Where(x => allClients.Select(y => y.ClientName).Contains(x.OrganizationName)));
                addedTsClients = addedTsClients.Except(duplicatedNameClients);

                foreach (var client in existingClients)
                {
                    var tsClient = updatedTsClients.FirstOrDefault(x => x.OrganizationID == client.ExternalClientId);
                    if (tsClient != null)
                    {
                        client.ClientName = tsClient.OrganizationName;
                        client.ClientType = tsClient.ClientTypeName;
                        client.ClientCode = tsClient.ClientCode;
                    }
                }

                List<Client> addedClients = new List<Client>();
                foreach (var client in addedTsClients) 
                {
                    addedClients.Add(new Client()
                    {
                        ClientName = client.OrganizationName,
                        ClientType = client.ClientTypeName,
                        ExternalClientId = client.OrganizationID,
                        ClientCode = client.ClientCode
                    });
                }

                await _clientRepository.UpdateRange(existingClients);
                await _clientRepository.UpdateRange(addedClients);
                await _clientRepository.SaveChangesAsync(cancellationToken);

                if (duplicatedNameClients.Any())
                    NotifyDuplicatedClients(duplicatedNameClients);
            }
            catch (CustomException ex)
            {
                _dataSyncLogger.LogWarning($"{GetType().Name} - SyncProcess skipped invalid data: {ex.Message}");
            }
        }

        private void NotifyDuplicatedClients(IEnumerable<Outbound.Client> duplicatedNameClients)
        {
            var duplicatedItems = new Dictionary<int, string>();
            var duplicatedItemsInStr = String.Empty;
            duplicatedNameClients.ToList().ForEach(
                x =>
                {
                    duplicatedItems.Add(x.OrganizationID, x.OrganizationName);
                    duplicatedItemsInStr += $"{x.OrganizationID}-{x.OrganizationName},";
                });
            var msg = String.Format("{0}: {1}", "Duplicated Name Clients: ", duplicatedItems.ToJsonString());
            throw new DuplicatedNameException(msg);
        }
    }
}
