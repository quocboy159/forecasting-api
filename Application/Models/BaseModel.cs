using System;

namespace ForecastingSystem.Application.Models
{
    public class BaseModel
    {
        /*
        This is the base view model, so here you can use 
        Data Annotations like [Required] or [MaxLength(20)]
        */
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int UnitPrice { get; set; }
        public int UnitQuantity { get; set; }
        public string URL { get; set; }
    }
}
