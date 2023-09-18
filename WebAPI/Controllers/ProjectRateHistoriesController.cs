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
    [OpenApiTag("ProjectRateHistories" , Description = "Methods to work with ProjectRateHistory")]
    [ApiController]
    public class ProjectRateHistoriesController : ControllerBase
    {
        private readonly IProjectRateService _projectRateService;

        public ProjectRateHistoriesController(IProjectRateService  projectRateService)
        {
            _projectRateService = projectRateService;
        }

        /// <summary>
        /// Get project rate histories by project id
        /// </summary>
        [HttpGet("{projectId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectRatesListModel), Description = "Successfully Returned List Of Project Rates Histories")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "List Of Project Rates Histories Is Empty")]
        public async Task<IActionResult> GetProjectRateHistoriesAsync(int projectId)
        {
            var projectRateHistoryListViewModel = await _projectRateService.GetRateHistoriesByProjectIdAsync(projectId);

            if (projectRateHistoryListViewModel != null)
            {
                return Ok(projectRateHistoryListViewModel);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
