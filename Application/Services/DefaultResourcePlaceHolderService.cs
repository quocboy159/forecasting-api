using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class DefaultResourcePlaceHolderService : IDefaultResourcePlaceHolderService
    {
        private readonly IDefaultResourcePlaceHolderRepository _defaultResourcePlaceHolderRepository;
        private readonly IMapper _mapper;
        public DefaultResourcePlaceHolderService(
            IMapper mapper,
            IDefaultResourcePlaceHolderRepository defaultResourcePlaceHolderRepository)
        {
            _mapper = mapper;
            _defaultResourcePlaceHolderRepository = defaultResourcePlaceHolderRepository;
        }

        public async Task<IEnumerable<DefaultResourcePlaceHolderModel>> GetDefaultResourcePlaceHolderAsync()
        {
            var defaultResourcePlaceHolders = await _defaultResourcePlaceHolderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DefaultResourcePlaceHolderModel>>(defaultResourcePlaceHolders);
        }
    }
}
