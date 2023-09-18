using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("UserIdLookup")]
    [ApiController]
    public class UserIdLookupController : ControllerBase
    {
        private readonly IUserIdLookupService _userIdLookupService;

        public UserIdLookupController(IUserIdLookupService userIdLookupService)
        {
            _userIdLookupService = userIdLookupService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userIdLookup = await _userIdLookupService.GetAll();
            return Ok(userIdLookup);
        }
    }
}
