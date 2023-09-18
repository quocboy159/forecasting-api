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
    [OpenApiTag("PhaseResourceExceptions", Description = "Methods to work with PhaseResourceExceptions")]
    [ApiController]
    public class PhaseResourceExceptionsController : ControllerBase
    {
        private readonly IPhaseResourceExceptionService _phaseResourceExceptionService;
        private readonly IPhaseResourceService _phaseResourceService;
        public PhaseResourceExceptionsController(IPhaseResourceExceptionService phaseResourceExceptionService
            , IPhaseResourceService phaseResourceService)
        {
            _phaseResourceExceptionService = phaseResourceExceptionService;
            _phaseResourceService = phaseResourceService;
        }

        /// <summary>
        /// Get all phase resources in the phase
        /// </summary>
        [HttpGet("{phaseId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseResourceExceptionListModel), Description = "Successfully Returned List Of PhaseResourceExceptions or Empty List")]
        public async Task<IActionResult> GetPhaseResourceExceptions(int phaseId)
        {
            var phaseResourceListModel = await _phaseResourceExceptionService.GetPhaseResourceExceptionsByPhaseIdAsync(phaseId);
            return Ok(phaseResourceListModel);
        }

        /// <summary>
        /// Add or Update Phase Resource Exception
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost] //URL/api/soaps   http metod Post
                   //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "Admin")]
                   //[SwaggerResponse(HttpStatusCode.Unauthorized , null , Description = "You Don't Have Authorization For This Request")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseResourceExceptionModelToView), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(PhaseResourceExceptionModelToView), Description = "PhaseResourceExceptions Created")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), Description = "PhaseResourceExceptions Model Not Found")]
        public async Task<IActionResult> AddOrEdit(PhaseResourceExceptionModel model)
        {
            var vaditator = new PhaseResourceExceptionModelValidator(_phaseResourceExceptionService, _phaseResourceService);
            var validationResult = await vaditator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
            }

            if (model.PhaseResourceExceptionId == 0)
            {
                model = await _phaseResourceExceptionService.AddAsync(model);
            }
            else
            {
                var phaseResource = await _phaseResourceExceptionService.GetPhaseResourceExceptionByIdAsync(model.PhaseResourceExceptionId);
                if (phaseResource == null) return NotFound("PhaseResourceExceptions Model You Want To Update Doesn't Exist");
                await _phaseResourceExceptionService.EditAsync(model);
            }

            return Ok(model);
        }

        /// <summary>
        /// Remove phase resource exception from Phase Detail
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")] //URL/api/soaps/id   http metod Delete
                             // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "Admin")]
                             //[SwaggerResponse(HttpStatusCode.Unauthorized , null , Description = "You Don't Have Authorization For This Request")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseResourceExceptionModel), Description = "Successfully Deleted PhaseResourceExceptions Model")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), Description = "PhaseResourceExceptions Model You Want To Delete Doesn't Exist")]
        public async Task<IActionResult> Delete(int id)
        {
            var phaseResource = await _phaseResourceExceptionService.GetPhaseResourceExceptionByIdAsync(id);

            if (phaseResource != null)
            {
                bool status = await _phaseResourceExceptionService.DeleteAsync(id);
                return Ok(status);
            }
            else
            {
                return NotFound("PhaseResourceExceptions Model You Want To Delete Doesn't Exist");
            }
        }
    }
}
