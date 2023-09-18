using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Net;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("Roles", Description = "Methods to work with Roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Get all roles and their default rates
        /// </summary>      
        [HttpGet] //URL/api/roles  http metod Get
        [SwaggerResponse(HttpStatusCode.OK, typeof(RoleListModel), Description = "Successfully Returned List Of Roles")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "List Of Roles Is Empty")]
        public IActionResult GetAll()
        {
            var roleListViewModel = _roleService.GetRoles();

            if (roleListViewModel != null)
            {
                return Ok(roleListViewModel);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
