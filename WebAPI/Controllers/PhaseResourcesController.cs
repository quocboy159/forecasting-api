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
    [OpenApiTag("PhaseResources", Description = "Methods to work with PhaseResources")]
    [ApiController]
    public class PhaseResourcesController : ControllerBase
    {
        private readonly IPhaseResourceService _phaseResourceService;
        private readonly IEmployeeService _employeeService;
        private readonly IProjectRateService _projectRateService;
        public PhaseResourcesController(IPhaseResourceService phaseResourceService,
            IEmployeeService employeeService,
            IProjectRateService projectRateService)
        {
            _phaseResourceService = phaseResourceService;
            _employeeService = employeeService;
            _projectRateService = projectRateService;
        }

        /// <summary>
        /// Get all phase resources in the phase
        /// </summary>
        [HttpGet("{phaseId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseResourceListModel), Description = "Successfully Returned List Of PhaseResources or Empty List")]
        public async Task<IActionResult> GetPhaseResources(int phaseId)
        {
            var phaseResourceListViewModel = await _phaseResourceService.GetPhaseResourcesByPhaseIdAsync(phaseId);
            return Ok(phaseResourceListViewModel);
        }

        /// <summary>
        /// Get all employee phase resources in the phase
        /// </summary>
        [HttpGet("{phaseId}/employees")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseResourceListModel), Description = "Successfully Returned List Of Employee PhaseResources or Empty List")]
        public async Task<IActionResult> GetEmployeePhaseResources(int phaseId, [FromQuery] bool isForProject)
        {
            var phaseResourceListViewModel =
                isForProject ? await _phaseResourceService.GetEmployeePhaseResourcesByProjectPhaseIdAsync(phaseId)
                             : await _phaseResourceService.GetEmployeePhaseResourcesByPhaseIdAsync(phaseId);
            return Ok(phaseResourceListViewModel);
        }

        /// <summary>
        /// Add or Update Phase Resource
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost] //URL/api/soaps   http metod Post
                   //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "Admin")]
                   //[SwaggerResponse(HttpStatusCode.Unauthorized , null , Description = "You Don't Have Authorization For This Request")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseResourceModel), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(PhaseResourceModel), Description = "PhaseResource Created")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), Description = "PhaseResource Model Not Found")]
        public async Task<IActionResult> AddOrEdit(PhaseResourceModel model)
        {
            var vaditator = new PhaseResourceModelValidator(_employeeService, _projectRateService);
            var validationResult = await vaditator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
            }

            if (model.PhaseResourceId == 0)
            {
                model = await _phaseResourceService.AddAsync(model);
            }
            else
            {
                var phaseResource = await _phaseResourceService.GetPhaseResourceByIdAsync(model.PhaseResourceId);
                if (phaseResource == null) return NotFound("PhaseResource Model You Want To Update Doesn't Exist");
                model = await _phaseResourceService.EditAsync(model);
            }

            return Ok(model);
        }

        /// <summary>
        /// Remove phase resource from Phase Detail
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{phaseResourceId}")] //URL/api/soaps/id   http metod Delete
                                          // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "Admin")]
                                          //[SwaggerResponse(HttpStatusCode.Unauthorized , null , Description = "You Don't Have Authorization For This Request")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseResourceModel), Description = "Successfully Deleted PhaseResource Model")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), Description = "PhaseResource Model You Want To Delete Doesn't Exist")]
        public async Task<IActionResult> Delete(int phaseResourceId)
        {
            var phaseResource = await _phaseResourceService.GetPhaseResourceByIdAsync(phaseResourceId);

            if (phaseResource != null)
            {
                await _phaseResourceService.DeleteAsync(phaseResourceId);
                return Ok();
            }
            else
            {
                return NotFound("PhaseResource Model You Want To Delete Doesn't Exist");
            }
        }
    }
}
