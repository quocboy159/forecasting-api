using System.ComponentModel.DataAnnotations;

namespace ForecastingSystem.BackendAPI.ViewModels
{
    public class SignInViewModel
    {
        [Required]
        public string UserName { get; set; }


        [Required]
        public string Password { get; set; }
    }
}
