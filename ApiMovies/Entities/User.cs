using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities
{
    /// <summary>
    /// User Entity
    /// </summary>
    public class User
    {
        /// <summary>
        /// User primery key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Complete name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        public string Role { get; set; }
    }
}
