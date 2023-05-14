using ApiMovies.Data;
using ApiMovies.Entities;
using ApiMovies.Repositories.IRepositories;

namespace ApiMovies.Repositories
{
    /// <summary>
    /// Category Repository
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Validates if a category exist by category name
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns>Boolean</returns>
        public bool CategoryExists(string categoryName)
        {
            bool categoryExists = _db.Category.Any(c => c.Name.ToLower().Trim() == categoryName.ToLower().Trim());
            return categoryExists;
        }

        /// <summary>
        /// Validates if a category exists by category ID
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool CategoryExists(int categoryId)
        {
            bool categoryExists = _db.Category.Any(c => c.Id == categoryId);
            return categoryExists;
        }

        /// <summary>
        /// Creates a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Boolean</returns>
        public bool CreateCategory(Category category)
        {
            category.CreatedOn = DateTime.Now;
            _db.Category.Add(category);
            return Save();
        }

        /// <summary>
        /// Deletes a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Boolean</returns>
        public bool DeleteCategory(Category category)
        {
            _db.Category.Remove(category);
            return Save();
        }

        /// <summary>
        /// Gets all the categories
        /// </summary>
        /// <returns>A collection of categories</returns>
        public ICollection<Category> GetCategories()
        {
            return _db.Category.OrderBy(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets a category by categoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Category GetCategory(int categoryId)
        {
            return _db.Category.FirstOrDefault(c => c.Id == categoryId);
        }

        /// <summary>
        /// Saves changes
        /// </summary>
        /// <returns>Boolean</returns>
        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        /// <summary>
        /// Updates a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Boolean</returns>
        public bool UpdateCategory(Category category)
        {
            category.CreatedOn = DateTime.Now;
            _db.Category.Update(category);
            return Save();
        }
    }
}
