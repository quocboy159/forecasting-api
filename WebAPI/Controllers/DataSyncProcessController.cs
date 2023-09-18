using ForecastingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("DataSyncProcess")]
    [ApiController]
    public class DataSyncProcessController : ControllerBase
    {
        private readonly IDataSyncProcessManagementService _dataSyncProcessManagementService;

        public DataSyncProcessController(IDataSyncProcessManagementService dataSyncProcessManagementService)
        {
            _dataSyncProcessManagementService = dataSyncProcessManagementService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var syncProcesses = await _dataSyncProcessManagementService.GetAll();
            return Ok(syncProcesses);
        }
    }
}
