using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities
{
    /// <summary>
    /// Category entity
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Category ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// Category creation datetime
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
