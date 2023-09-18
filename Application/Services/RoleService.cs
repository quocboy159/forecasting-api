using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using System.Collections.Generic;

namespace ForecastingSystem.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
        }
        public RoleListModel GetRoles()
        {
            var roles = _roleRepository.GetAll();
            var roleListViewModel = _mapper.Map<IEnumerable<RoleModel>>(roles);

            return new RoleListModel()
            {
                Roles = roleListViewModel
            };
        }
    }
}
