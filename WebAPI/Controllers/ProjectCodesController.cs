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
    [OpenApiTag("ProjectCodes" , Description = "Methods to work with ProjectCodes")]
    [ApiController]
    public class ProjectCodesController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectCodesController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Get default ProjectCodes
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(RateModel), Description = "Successfully Returned List Of Default ProjectCodes")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "List Of Default ProjectCodes Is Empty")]
        public async Task<IActionResult> GetDefaultProjectCodesAsync()
        {
            var projectCodes = await _projectService.GetProjectCodeListAsync();

            if (projectCodes != null)
            {
                return Ok(projectCodes);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
