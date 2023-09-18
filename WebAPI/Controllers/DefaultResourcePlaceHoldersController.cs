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
    [OpenApiTag("DefaultResourcePlaceHolders" , Description = "Methods to work with DefaultResourcePlaceHolders")]
    [ApiController]
    public class DefaultResourcePlaceHoldersController : ControllerBase
    {
        private readonly IDefaultResourcePlaceHolderService _resourcePlaceHolderService;
        public DefaultResourcePlaceHoldersController(IDefaultResourcePlaceHolderService resourcePlaceHolderService)
        {
            _resourcePlaceHolderService = resourcePlaceHolderService;
        }

        /// <summary>
        /// Get default resource placeholders
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(RateModel), Description = "Successfully Returned List Of Default Resource PlaceHolder")]
        public async Task<IActionResult> GetDefaultRatesAsync()
        {
            var defaultResourcePlaceHolders = await _resourcePlaceHolderService.GetDefaultResourcePlaceHolderAsync();
            return Ok(defaultResourcePlaceHolders);
        }
    }
}
