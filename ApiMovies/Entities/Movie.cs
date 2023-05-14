using ApiMovies.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovies.Entities
{
    /// <summary>
    /// Movie Entity
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Movie primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Movie name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Movie image path
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Movie description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Movie duration
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Movie classification
        /// </summary>
        public TypeClassificationEnum Classification { get; set; }

        /// <summary>
        /// Moview category ID
        /// </summary>
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Category navigation property
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Moview created on
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
