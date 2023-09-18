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
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("Projects" , Description = "Methods to work with Revenue forecast")]
    [ApiController]
    public class RevenueForecastController : ControllerBase
    {
        private readonly IProjectService _projectService;
        
        public RevenueForecastController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startWeek"></param>
        /// <returns></returns>
        [HttpGet("{startWeek}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectRevenueListModel), Description = "Successfully Returned List Of Project Revenue")]
        public async Task<IActionResult> GetProjectRevenues(DateTime startWeek)
        {
            var revenues = await _projectService.GetProjectsRevenueForecast(startWeek);
            var response = new ProjectRevenueListModel()
            {
                Projects = revenues.Item1,
                StartWeek = startWeek,
                ConsumedTimes = revenues.Item2,
            };
            return Ok(response);
        }


        /// <summary>
        /// For testing only
        /// </summary>
        [HttpGet("ProjectRevenues/{projectIds}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ProjectRevenueModel), Description = "Successfully Returned List Of Project Revenue")]
        public async Task<IActionResult> GetProjectRevenuesByProjectIds(string projectIds)
        {
            var ids = projectIds.Split(',').Select(int.Parse).ToList();
            var revenues = await _projectService.GetProjectRevenues(ids);            
            return Ok(revenues);
        }
    }
}
