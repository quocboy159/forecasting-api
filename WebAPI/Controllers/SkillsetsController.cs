using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models.Validators;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsetsController : ControllerBase
    {
        private readonly ISkillsetService _skillsetService;
        private readonly IMapper _mapper;

        public SkillsetsController(ISkillsetService skillsetService, IMapper mapper)
        {
            _skillsetService = skillsetService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all skillsets
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Application.Models.SkillsetModel), Description = "Successfully Returned List Of SkillsetViewModel Or Empty List")]
        public IActionResult GetSkillsets()
        {
            var skillsets = _skillsetService.GetSkillsets();
            return Ok(skillsets);
        }

        /// <summary>
        /// Add Skillset
        /// </summary>
        /// <param name="modelToAdd"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(SkillsetAddModel), Description = "Ok")]
        public async Task<IActionResult> AddAsync(SkillsetAddModel modelToAdd)
        {

            SkillsetModelValidator vaditator = new SkillsetModelValidator(_skillsetService);
            var validationResult = await vaditator.ValidateAsync(modelToAdd);

            if (!validationResult.IsValid)
            {
                return BadRequest(string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage)));
            }

            var model = _mapper.Map<SkillsetModel>(modelToAdd);
            model = await _skillsetService.AddAsync(model);

            return Ok(model);
        }
    }
}
