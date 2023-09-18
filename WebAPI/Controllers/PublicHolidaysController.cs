using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("PublicHolidays" , Description = "Methods to work with PublicHoliday")]
    [ApiController]
    public class PublicHolidaysController : ControllerBase
    {
        private readonly IPublicHolidayService _publicHolidayService;

        public PublicHolidaysController(IPublicHolidayService publicHolidayService)
        {
            _publicHolidayService = publicHolidayService;
        }

        /// <summary>
        /// Get all public holidays
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PublicHolidayModel), Description = "Successfully Returned List Of Public Holidays Or Empty List")]
        public async Task<IActionResult> GetPublicHolidaysAsync()
        {
            var publicHolidayListViewModel = await _publicHolidayService.GetAllPublicHolidaysAsync();

            return Ok(publicHolidayListViewModel);
        }
    }
}
