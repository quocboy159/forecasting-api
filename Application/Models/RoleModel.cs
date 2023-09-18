using System.ComponentModel.DataAnnotations;

namespace ForecastingSystem.Application.Models
{
    public class RoleModel
    {
        /*
        This is the model that the user sees it in the presentation/view,
        so here you can use Data Annotation like [Required] or [MaxLength(20)]
        */
        [Required]
        public int RoleId { get; set; }
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = null!;

        public string Description { get; set; }

        public int? DefaultRate { get; set; }
    }
}
