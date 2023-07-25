using System.ComponentModel.DataAnnotations;

namespace BeanSceneReservationApplication.Areas.Management.Models
{
    public class UserVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }

    }
}
