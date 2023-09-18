using System.Collections.Generic;

namespace ForecastingSystem.Application.Models
{
    public class PhaseResourceListModel
    {
        public IEnumerable<PhaseResourceModelToView> PhaseResources { get; set; } = new List<PhaseResourceModelToView>();
    }
}
