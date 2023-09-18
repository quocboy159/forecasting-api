using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class ResourceUtilisationNoteRepository : GenericAsyncRepository<ResourceUtilisationNote>, IResourceUtilisationNoteRepository
    {
        public ResourceUtilisationNoteRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
        }
    }
}
