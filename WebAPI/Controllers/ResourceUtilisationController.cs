using ForecastingSystem.Application.Common;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Application.Models.Validators;
using ForecastingSystem.Application.Services;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("Employee", Description = "Methods to work with Resource Utilisation")]
    [ApiController]
    public class ResourceUtilisationController : ControllerBase
    {
        private readonly IResourceUtilisationService _resourceUtilisationService;

        public ResourceUtilisationController(IResourceUtilisationService resourceUtilisationService)
        {
            _resourceUtilisationService = resourceUtilisationService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startWeek"></param>
        /// <returns></returns>
        [HttpGet("{startWeek}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ResourceUtilisationListModel), Description = "Successfully Returned List Of Project Revenue")]
        public async Task<IActionResult> GetResourceUtilisations(DateTime startWeek)
        {
            const int maxNumberOfWeeks = 52;
            var (resources, consumedTimes) = await _resourceUtilisationService.GetResourceUtilisations(startWeek, maxNumberOfWeeks);
            var response = new ResourceUtilisationListModel(maxNumberOfWeeks)
            {
                Resources = resources,
                StartWeek = startWeek,
                ConsumedTimes= consumedTimes,
            };
            return Ok(response);
        }
    }
}
