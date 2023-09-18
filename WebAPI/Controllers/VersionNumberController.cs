using ForecastingSystem.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("VersionNumber" , Description = "Methods to get BackEnd API version number")]
    [ApiController]
    public class VersionNumberController : ControllerBase
    {

        [HttpGet] //URL/api/versionnumber  http metod Get
        [SwaggerResponse(HttpStatusCode.OK , typeof(VersionNumber) , Description = "Successfully Returned List Of Clients Or Empty List")]
        public IActionResult GetVersion()
        {

            return Ok(new VersionNumber
            {
                BackendVersion = Version.VersionNumber ,
                DataSyncVersion = ForecastingSystem.DataSyncServices.Version.DataSyncVersionNumber
            });
        }
    }
}
