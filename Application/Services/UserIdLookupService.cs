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
    public class UserIdLookupService : IUserIdLookupService
    {
        private readonly IUserIdLookupRepository _userIdLookupRepository;
        private readonly IMapper _mapper;
        public UserIdLookupService(IUserIdLookupRepository userIdLookupRepository,
            IMapper mapper)
        {
            _userIdLookupRepository = userIdLookupRepository;
            _mapper = mapper;
        }

        public async Task<List<UserIdLookupModel>> GetAll()
        {
            var list = await _userIdLookupRepository.GetAllAsync();
            return _mapper.Map<List<UserIdLookupModel>>(list);
        }
    }
}
