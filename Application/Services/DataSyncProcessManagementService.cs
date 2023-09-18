using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class DataSyncProcessManagementService : IDataSyncProcessManagementService
    {
        private readonly IDataSyncProcessRepository _dataSyncProcessRepository;
        private readonly IMapper _mapper;
        public DataSyncProcessManagementService(IDataSyncProcessRepository dataSyncProcessRepository,
            IMapper mapper)
        {
            _dataSyncProcessRepository = dataSyncProcessRepository;
            _mapper = mapper;
        }

        public async Task<List<DataSyncProcessModel>> GetAll()
        {
            var list = await _dataSyncProcessRepository.GetAllAsync();
            return _mapper.Map<List<DataSyncProcessModel>>(list);
        }
    }
}
