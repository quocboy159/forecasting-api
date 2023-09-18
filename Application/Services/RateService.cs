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
    public class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;
        public RateService(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public async Task<IEnumerable<RateModel>> GetDefaultRatesAsync()
        {
            var defaultRates = (await _rateRepository.GetAsync(x => x.IsActive)).Select(x => new RateModel
            {
                RateId = x.RateId,
                HourlyRate = x.HourlyRate,
                RateName = x.RateName,
                EffectiveDate = DateTime.Today
            });

            return defaultRates;
        }
    }
}
