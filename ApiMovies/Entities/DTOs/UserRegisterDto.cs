using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTOs
{
    /// <summary>
    /// User register DTO
    /// </summary>
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
