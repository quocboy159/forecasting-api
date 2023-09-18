using ForecastingSystem.Application.Interfaces;
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
    [OpenApiTag("DBTools", Description = "Methods to interact with database")]
    [ApiController]
    public class DBToolsController : ControllerBase
    {
        private readonly IDatabaseManagementService _databaseManagementService;
        private readonly IDataSyncProcessManagementService _dataSyncProcessManagementService;
        public DBToolsController(
            IDatabaseManagementService databaseManagementService,
            IDataSyncProcessManagementService dataSyncProcessManagementService)
        {
            _databaseManagementService = databaseManagementService;
            _dataSyncProcessManagementService = dataSyncProcessManagementService;
        }

#if ENABLE_DROP_ALL_TABLES_API
        /// <summary>
        /// Drop all tables from the database
        /// </summary>      
        [HttpGet("ResetDatabase")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string), Description = "Successfully dropped all tables")]
        public async Task<IActionResult> ResetDatabase()
        {
            if (IsDataSyncRunning())
            {
                return BadRequest("DataSync is running, please try again in 5 minutes");
            }

            await _databaseManagementService.DropTables();
            await _databaseManagementService.RecreateTables();

            return Ok("Database is reset. Please wait for next data-sync to copy data over again");
        }
#endif

        /// <summary>
        /// Reset DataSyncType
        /// </summary>      
        [HttpPut("ResetDataSyncType")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string), Description = "Successfully reset DataSyncType")]
        public async Task<IActionResult> ResetDataSyncType(string dataSyncType)
        {
            await _databaseManagementService.ResetDataSyncTypeAsync(dataSyncType);

            return Ok();
        }

        /// <summary>
        /// Get Datasync list
        /// </summary>      
        [HttpGet("DataSyncs")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string), Description = "Successfully get Datasync list")]
        public async Task<IActionResult> GetDataSyncs()
        {
            var result = await _dataSyncProcessManagementService.GetAll();

            return Ok(result);
        }


        private bool IsDataSyncRunning()
        {
            var syncProcesses = _dataSyncProcessManagementService.GetAll().Result;
            bool isRunning = syncProcesses.Any(x => x.Status == Domain.Common.DataSyncProcessStatuses.Inprogess);
            return isRunning;
        }

    }
}
