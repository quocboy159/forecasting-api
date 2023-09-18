using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Application.Models.Validators;
using ForecastingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("Rates" , Description = "Methods to work with Rates")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly IRateService _rateService;
        public RatesController(IRateService rateService)
        {
            _rateService = rateService;
        }

        /// <summary>
        /// Get default rates
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(RateModel), Description = "Successfully Returned List Of Default Rates")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "List Of Default Rates Is Empty")]
        public async Task<IActionResult> GetDefaultRatesAsync()
        {
            var defaultRateModels = await _rateService.GetDefaultRatesAsync();

            if (defaultRateModels != null)
            {
                return Ok(defaultRateModels);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
