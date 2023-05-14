using ApiMovies.Entities;

namespace ApiMovies.Repositories.IRepositories
{
    /// <summary>
    /// Category repository interface
    /// </summary>
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int categoryId);

        bool CategoryExists(string categoryName);

        bool CategoryExists(int categoryId);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);

        bool Save();
    }
}
