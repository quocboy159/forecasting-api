using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Application.Models.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhaseSkillsetsController : ControllerBase
    {
        private readonly IPhaseSkillsetService _phaseSkillsetService;
        private readonly ISkillsetService _skillsetService;

        public PhaseSkillsetsController(IPhaseSkillsetService phaseSkillsetService, ISkillsetService skillsetService)
        {
            _phaseSkillsetService = phaseSkillsetService;
            _skillsetService = skillsetService;
        }

        /// <summary>
        /// Get PhaseSkillsets by PhaseId
        /// </summary>
        [HttpGet("{phaseId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseSkillsetModelToView), Description = "Successfully Returned List Of PhaseSkillsetViewModel Or Empty List")]
        public IActionResult GetPhaseSkillsets(int phaseId)
        {
            IList<PhaseSkillsetModelToView> phaseSkillsets = _phaseSkillsetService.GetPhaseSkillsets(phaseId);
            return Ok(phaseSkillsets);
        }

        /// <summary>
        /// Add or Update Phase Skillset
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseSkillsetModel), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(PhaseSkillsetModel), Description = "PhaseSkillset Created")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(string), Description = "PhaseSkillset Not Found")]
        public IActionResult AddOrUpdate(PhaseSkillsetModel model)
        {
            var vaditator = new PhaseSkillsetsModelValidator(_skillsetService, null);
            var validationResult = vaditator.Validate(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
            }

            if (model.PhaseSkillSetId == 0)
            {
                model = _phaseSkillsetService.Add(model);
            }
            else
            {
                model = _phaseSkillsetService.Update(model);
            }

            return Ok(model);

        }

        /// <summary>
        /// Remove PhaseSkillset
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PhaseSkillsetModel), Description = "Successfully Deleted PhaseSkillset")]
        public IActionResult Delete(int id)
        {
            var result = _phaseSkillsetService.Delete(id);
            var msg = result ? "Successfully Deleted PhaseSkillset" : "No PhaseSkillset to delete";
            return Ok(msg);
        }

    }
}
