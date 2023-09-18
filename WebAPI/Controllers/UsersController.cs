using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("User" , Description = "Methods to work with User")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public UsersController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Get logged user role
        /// </summary>
        [HttpGet("loggedUserRole")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(EmployeeRoleTypeModel), Description = "Successfully Returned Role Of Logged User")]
        public async Task<IActionResult> GetLoggedUserRole()
        {
            var userRoleType = await _currentUserService.GetUserRoleTypeAsync();
            return Ok(userRoleType);
        }
    }
}
