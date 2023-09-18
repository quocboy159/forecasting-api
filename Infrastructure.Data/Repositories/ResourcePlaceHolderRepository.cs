using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class ResourcePlaceHolderRepository : GenericAsyncRepository<ResourcePlaceHolder>, IResourcePlaceHolderRepository
    {
        public ResourcePlaceHolderRepository(ForecastingSystemDbContext context) : base(context)
        {
        }
    }
}
