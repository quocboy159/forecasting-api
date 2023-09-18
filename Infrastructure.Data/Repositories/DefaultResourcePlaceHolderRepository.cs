using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class DefaultResourcePlaceHolderRepository : GenericAsyncRepository<DefaultResourcePlaceHolder>, IDefaultResourcePlaceHolderRepository
    {
        public DefaultResourcePlaceHolderRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
        }
    }
    
}
