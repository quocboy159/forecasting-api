using System.Collections.Generic;

namespace ForecastingSystem.Domain.Models
{
    public class ResourcePlaceHolder
    {
        public int ResourcePlaceHolderId { get; set; }

        public string ResourcePlaceHolderName { get;set; }
        public string Country { get; set; }

        public virtual PhaseResource PhaseResource { get; }
    }
}
