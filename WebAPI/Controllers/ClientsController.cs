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
    [OpenApiTag("Clients" , Description = "Methods to work with Clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Get all clients and their default rates
        /// </summary>      
        [HttpGet] //URL/api/clients  http metod Get
        [SwaggerResponse(HttpStatusCode.OK , typeof(ClientListModel) , Description = "Successfully Returned List Of Clients Or Empty List")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _clientService.GetClientsAsync();
            return Ok(result);
        }
    }
}
