using ForecastingSystem.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IPublicHolidayService
    {
        Task<IEnumerable<PublicHolidayModel>> GetAllPublicHolidaysAsync();
    }
}
