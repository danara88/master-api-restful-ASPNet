using ApiMovies.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTOs
{
    /// <summary>
    /// Movie DTO
    /// </summary>
    public class MovieDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        public string Name { get; set; }

        public string ImagePath { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The duration is required.")]
        public int Duration { get; set; }

        public TypeClassificationEnum Classification { get; set; }

        public int CategoryId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
