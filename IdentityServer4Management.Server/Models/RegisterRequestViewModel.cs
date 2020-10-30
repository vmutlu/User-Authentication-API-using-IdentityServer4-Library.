using System.ComponentModel.DataAnnotations;

namespace IdentityServer4Management.Server.Models
{
    public class RegisterRequestViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string EMail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
