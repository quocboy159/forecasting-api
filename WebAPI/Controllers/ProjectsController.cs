using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Application.Models.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NSwag.Annotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("Projects", Description = "Methods to work with Projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRateService _projectRateService;
        private readonly IProjectPhaseService _projectPhaseService;
        private readonly IProjectService _projectService;
        private readonly IClientService _clientService;
        private readonly IPhaseResourceService _phaseResourceService;

        public ProjectsController(IProjectService projectService, IProjectRateService projectRateService, IClientService clientService,
            IProjectPhaseService projectPhaseService,
            IPhaseResourceService phaseResourceService)
        {
            _projectService = projectService;
            _projectRateService = projectRateService;
            _clientService = clientService;
            _projectPhaseService = projectPhaseService;
            _phaseResourceService = phaseResourceService;
        }

        /// <summary>
        /// Get project by id
        /// </summary>
        [HttpGet("{projectId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectDetailModel), Description = "Successfully Returned Project")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "The Project Is Not Found")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, null, Description = "User Does Not Have Access To Project")]
        public async Task<IActionResult> GetProjectAsync(int projectId)
        {
            if (!await _projectService.IsUserHasAccessToProjectAsync(projectId)) return Unauthorized();

            var projectDetailViewModel = await _projectService.GetProjectDetailAsync(projectId);

            if (projectDetailViewModel != null)
            {
                if (projectDetailViewModel.IsObsoleteProjectValue ?? true)
                {
                    var projectRevenue = await _projectService.GetProjectRevenue(projectId);
                    projectDetailViewModel.ProjectValue = projectRevenue.ProjectValue;
                }
                return Ok(projectDetailViewModel);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get all rates in the project
        /// </summary>
        [HttpGet("{projectId}/ProjectRates")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectRatesListModel), Description = "Successfully Returned List Of ProjectRates")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "List Of ProjectRates Is Empty")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, null, Description = "User Does Not Have Access To Project")]
        public async Task<IActionResult> GetProjectRatesAsync(int projectId)
        {
            if (!await _projectService.IsUserHasAccessToProjectAsync(projectId)) return Unauthorized();

            var ratesListViewModel = await _projectRateService.GetProjectRatesAsync(projectId);

            if (ratesListViewModel != null)
            {
                return Ok(ratesListViewModel);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectListModel), Description = "Successfully Returned List Of Projects")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "List Of Projects Is Empty")]
        public async Task<IActionResult> GetProjectsAsync()
        {
            var projectsListViewModel = await _projectService.GetAllProjectsAsync();
            var projectValues = await _projectService.GetProjectRevenues(projectsListViewModel.Projects.Select(s => s.ProjectId).ToList());
            if (projectsListViewModel != null)
            {
                foreach (var proj in projectsListViewModel.Projects)
                {
                    var projectValue = projectValues.FirstOrDefault(s => proj.ProjectId == s.ProjectId);
                    if (projectValue != null) proj.ProjectValue = projectValue.ProjectValue;
                }
                return Ok(projectsListViewModel);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Add or Update Project 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "Admin")]
        //[SwaggerResponse(HttpStatusCode.Unauthorized , null , Description = "You Don't Have Authorization For This Request")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectModel), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(ProjectModel), Description = "Project Created")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), Description = "Project Model Not Found")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, null, Description = "User Does Not Have Access To Project")]
        public async Task<IActionResult> SaveAsync(ProjectDetailAddEditModel model)
        {
            if (model.ProjectId != 0 && !await _projectService.IsUserHasAccessToProjectAsync(model.ProjectId)) return Unauthorized();

            ProjectModelValidator vaditator = new ProjectModelValidator(_projectService, _clientService, _projectRateService, _phaseResourceService);
            var validationResult = await vaditator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
            }
            var project = await _projectService.SaveAsync(model);


            var projectDetailViewModel = await _projectService.GetProjectDetailAsync(project.ProjectId);
            if (projectDetailViewModel.IsObsoleteProjectValue ?? true)
            {
                var projectRevenue = await _projectService.GetProjectRevenue(project.ProjectId);
                projectDetailViewModel.ProjectValue = projectRevenue.ProjectValue;
                foreach (var phase in projectDetailViewModel.Phases)
                {
                    var recalculatedPhase = projectRevenue.PhaseRevenues.FirstOrDefault(x => x.PhaseId == phase.PhaseId);
                    if (recalculatedPhase != null)
                    {
                        phase.PhaseBudget = recalculatedPhase.Budget;
                        phase.EstimatedEndDate = recalculatedPhase.EstimatedEndDate;
                    }
                }
            }
            return Ok(projectDetailViewModel);
        }


        /// <summary>
        /// Get all phases in the project
        /// </summary>
        [HttpGet("{projectId}/ProjectPhases")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectPhasesListModel), Description = "Successfully Returned List Of ProjectPhases")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "List Of ProjectPhases Is Empty")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, null, Description = "User Does Not Have Access To Project")]
        public async Task<IActionResult> GetProjectPhasessAsync(int projectId)
        {
            if (!await _projectService.IsUserHasAccessToProjectAsync(projectId)) return Unauthorized();

            var phasesListViewModel = _projectPhaseService.GetProjectPhases(projectId);

            if (phasesListViewModel != null)
            {
                return Ok(phasesListViewModel);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// check project has linked phase
        /// </summary>
        [HttpGet("{projectId}/check-has-linked-phase")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(bool), Description = "Successfully Returned true/false")]
        public async Task<IActionResult> CheckProjectHasLinkedPhase(int projectId)
        {
            var result = await _projectService.CheckProjectHasLinkedPhase(projectId);
            return Ok(result);
        }
    }
}
