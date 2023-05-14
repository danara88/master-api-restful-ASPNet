using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTOs
{
    /// <summary>
    /// User login DTO
    /// </summary>
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
