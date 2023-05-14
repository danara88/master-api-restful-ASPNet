using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTOs
{
    /// <summary>
    /// Category DTO
    /// </summary>
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [MaxLength(60, ErrorMessage = "The maximum number of characters is 60.")]
        public string Name { get; set; }
    }
}
