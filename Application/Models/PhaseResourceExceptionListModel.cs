using System.Collections.Generic;

namespace ForecastingSystem.Application.Models
{
    public class PhaseResourceExceptionListModel
    {
        public IEnumerable<PhaseResourceExceptionModelToView> PhaseResourceExceptions { get; set; } = new List<PhaseResourceExceptionModelToView>();
    }
}
