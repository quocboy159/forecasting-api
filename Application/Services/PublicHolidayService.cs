using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class PublicHolidayService : IPublicHolidayService 
    {
        private readonly IMapper _mapper;
        private readonly IPublicHolidayRepository _publicHolidayRepository;

        public PublicHolidayService(IMapper mapper, IPublicHolidayRepository publicHolidayRepository)
        {
            _mapper = mapper;
           _publicHolidayRepository = publicHolidayRepository;
        }

        public async Task<IEnumerable<PublicHolidayModel>> GetAllPublicHolidaysAsync()
        {
            var result = await _publicHolidayRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<PublicHolidayModel>>(result);
        }
    }
}
