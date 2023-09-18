using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ForecastingSystem.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [OpenApiTag("Employee" , Description = "Methods to work with Employee")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly string DefaultProjectManagerRoleName = "NZ PM / Architect";
        private readonly string _projectManagerRoleName;
        private readonly IEmployeeService _employeeService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeService employeeService, IConfiguration configuration, IMapper mapper)
        {
            _employeeService = employeeService;
            _configuration = configuration;
            _mapper = mapper;
            _projectManagerRoleName = _configuration["ProjectManagerRoleName"];
            if (string.IsNullOrEmpty(_projectManagerRoleName.Trim()))
                _projectManagerRoleName = DefaultProjectManagerRoleName;
        }

        /// <summary>
        /// Get employee by id
        /// </summary>
        [HttpGet("{employeeId}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(EmployeeDetailModel), Description = "Successfully Returned Employee")]
        [SwaggerResponse(HttpStatusCode.NotFound, null, Description = "The Employee Is Not Found")]
        public async Task<IActionResult> GetProjectAsync(int employeeId)
        {
            var employeeDetailViewModel = await _employeeService.GetEmployeeDetailAsync(employeeId);

            if (employeeDetailViewModel != null)
            {
                return Ok(employeeDetailViewModel);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Save EmployeeUtilisationNotes
        /// </summary>
        /// <param name="addEditModel"></param>
        /// <returns></returns>
        [HttpPost("{employeeId}/utilisationnotes")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(EmployeeUtilisationNotesModel), Description = "Ok")]
        public async Task<IActionResult> SaveAsync(int employeeId, [FromBody] EmployeeUtilisationNotesAddEditModel addEditModel)
        {
            var model = _mapper.Map<EmployeeUtilisationNotesModel>(addEditModel);
            model.EmployeeId = employeeId;

            await _employeeService.SaveEmployeeUtilisationNotes(model);

            return Ok();
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(EmployeeModel), Description = "Successfully Returned List Of Employees Or Empty List")]
        public IActionResult GetEmployees()
        {
            var employeeListViewModel = _employeeService.GetEmployees();
            return Ok(employeeListViewModel);
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        [HttpGet("projectresources")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(EmployeeModel), Description = "Successfully Returned List Of Employees and default Place Holders Or Empty List")]
        public async Task<IActionResult> GetProjectResources()
        {
            var projectResources = await _employeeService.GetProjectResourcesAsync();
            return Ok(projectResources);
        }        
    }
}
