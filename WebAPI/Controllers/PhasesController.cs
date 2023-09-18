using AutoMapper;
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
    [OpenApiTag("Phases", Description = "Methods to work with Phases")]
    [ApiController]
    public class PhasesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectPhaseService _projectPhaseService;
        private readonly IProjectService _projectService;
        private readonly ISkillsetService _skillsetService;

        public PhasesController(IMapper mapper, IProjectPhaseService projectPhaseService,
            IProjectService projectService,
            ISkillsetService skillsetService)
        {
            _mapper = mapper;
            _projectPhaseService = projectPhaseService;
            _projectService = projectService;
            _skillsetService = skillsetService;
        }

        /// <summary>
        /// Get Phase including Skillsets by PhaseId
        /// </summary>
        [HttpGet("{phaseId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectPhaseModel), Description = "Successfully Returned Phase with its Skillsets")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "Phase with the provided id does not exist")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, null, Description = "User Does Not Have Access To Project")]
        public async Task<IActionResult> GetPhaseById(int phaseId)
        {
            ProjectPhaseModel phase = await _projectPhaseService.GetProjectPhaseByIdAsync(phaseId);

            if (!await _projectService.IsUserHasAccessToProjectAsync(phase.ProjectId)) return Unauthorized();

            if (phase != null)
            {
                return Ok(phase);
            }
            else
            {
                return NotFound("Phase with the provided id does not exist");
            }
        }

        /// <summary>
        /// Add or Update Project Phase including Skillsets
        /// </summary>
        /// <param name="modelToAdd"></param>
        /// <returns></returns>
        [HttpPost] //URL/api/soaps   http metod Post
                   //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "Admin")]
                   //[SwaggerResponse(HttpStatusCode.Unauthorized , null , Description = "You Don't Have Authorization For This Request")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectPhaseModelToAdd), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(ProjectPhaseModelToAdd), Description = "Phase Created/Updated successfully")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), Description = "Phase Model Not Found")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, null, Description = "User Does Not Have Access To Project")]
        public async Task<IActionResult> AddOrEdit(ProjectPhaseModelToAdd modelToAdd)
        {
            if (!await _projectService.IsUserHasAccessToProjectAsync(modelToAdd.ProjectId)) return Unauthorized();

            var existingPhase = await _projectPhaseService.GetProjectPhaseByIdAsync(modelToAdd.PhaseId);

            ProjectPhaseModelValidator vaditator = new ProjectPhaseModelValidator(_skillsetService, _projectPhaseService, _projectService, modelToAdd.PhaseId);
            var validationResult = vaditator.Validate(modelToAdd);

            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
            }
            var model = _mapper.Map<ProjectPhaseModel>(modelToAdd);
            if (model.PhaseId == 0)
            {
                model = await _projectPhaseService.AddAsync(model);
            }
            else
            {
                if (existingPhase == null) return NotFound("Phase Model You Want To Update Doesn't Exist");

                model = await _projectPhaseService.EditAsync(model);
            }

            return Ok(model);
        }

        /// <summary>
        /// Remove project phase from a project
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")] //URL/api/soaps/id   http metod Delete
                             // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "Admin")]
                             //[SwaggerResponse(HttpStatusCode.Unauthorized , null , Description = "You Don't Have Authorization For This Request")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectPhaseModel), Description = "Successfully Deleted Phase Model")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), Description = "Phase Model You Want To Delete Doesn't Exist")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, null, Description = "User Does Not Have Access To Project")]
        public async Task<IActionResult> Delete(int id)
        {
            var projectPhase = await _projectPhaseService.GetProjectPhaseByIdAsync(id);

            if (!await _projectService.IsUserHasAccessToProjectAsync(projectPhase.ProjectId)) return Unauthorized();

            bool status = false; ;

            if (projectPhase != null)
            {
                if (_projectService.IsOpportunity(projectPhase.ProjectId))
                {
                    status = await _projectPhaseService.DeleteAsync(id);
                    return Ok(status);
                }
                else
                {
                    return BadRequest("Only oppotunity project type can be modified.");
                }

            }
            else
            {
                return NotFound("Phase Model You Want To Delete Doesn't Exist");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("GetPhaseValue/{phaseId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseRevenueModel), Description = "Successfully Returned Phase value and Estimated DateTime")]
        public async Task<IActionResult> GetPhaseValue(int phaseId)
        {
            //TODO: dangle - Why this API is called somany times by FE?
            var phase = await _projectPhaseService.GetProjectPhaseByIdAsync(phaseId);
            var modelReturn = new PhaseRevenueModel()
            {
                PhaseId = phase.PhaseId,
                PhaseName = phase.PhaseName,
                PhaseCode = phase.PhaseCode,
                StartDate = phase.StartDate,
                EndDate = phase.EndDate,
                Budget = phase.Budget,
                IsCalculatingByResource = phase.IsCalculatingByResource,
                EstimatedEndDate = phase.EstimatedEndDate,
                DbPhaseValue = phase.PhaseValue
            };
            return Ok(modelReturn);
        }

        /// <summary>
        /// check is a project phase can link
        /// </summary>
        [HttpGet("{phaseId}/can-link-to/{timeSheetPhaseId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(bool), Description = "Successfully Returned true or false")]
        public async Task<IActionResult> CheckCanLinkToTimeSheetPhaseId([FromRoute] int phaseId, [FromRoute] int timeSheetPhaseId)
        {
            var result = await _projectPhaseService.CheckCanLinkToTimeSheetPhaseId(phaseId, timeSheetPhaseId);
            return Ok(result);
        }
    }
}
