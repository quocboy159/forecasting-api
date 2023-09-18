using ForecastingSystem.Domain.Models;
using System;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IDefaultResourcePlaceHolderRepository : IAsyncRepository<DefaultResourcePlaceHolder>
    {
    }
}
