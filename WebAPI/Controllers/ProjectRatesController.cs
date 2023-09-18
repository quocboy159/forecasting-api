using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Application.Models.Validators;
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
    [OpenApiTag("ProjectRates" , Description = "Methods to work with ProjectRates")]
    [ApiController]
    public class ProjectRatesController : ControllerBase
    {
        private readonly IProjectRateService _projectRateService;
        private readonly IProjectService _projectService;
        public ProjectRatesController(IProjectRateService projectRateService, IProjectService projectService)
        {
            _projectRateService = projectRateService;
            _projectService = projectService;
        }

        /// <summary>
        /// Get rates of project
        /// </summary>
        [HttpGet("project/{projectId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(EmployeeModel), Description = "Successfully Returned List Of Rates for ProjectId Or Empty List")]
        public async Task<IActionResult> GetProjectRates(int projectId)
        {
            var projectResources = await _projectRateService.GetProjectRatesByProjectIdAsync(projectId);
            return Ok(projectResources);
        }
    }
}
