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
    public class RateRepository : GenericAsyncRepository<Rate>, IRateRepository
    {
        public RateRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
            /*
                This is the place where we create the logic for query,
                for saving and calling data for that entity.
             */
        }
    }
    
}
