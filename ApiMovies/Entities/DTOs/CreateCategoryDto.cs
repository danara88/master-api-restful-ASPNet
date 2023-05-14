using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTOs
{
    /// <summary>
    /// Create category DTO
    /// </summary>
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "The name is required.")]
        [MaxLength(60, ErrorMessage = "The maximum number of characters is 60.")]
        public string Name { get; set; }
    }
}
