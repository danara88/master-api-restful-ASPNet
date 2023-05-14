using ApiMovies.Data;
using ApiMovies.Entities;
using ApiMovies.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Repositories
{
    /// <summary>
    /// Movie Repository
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _db;

        public MovieRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Validates if a Movie exist by Movie name
        /// </summary>
        /// <param name="MovieName"></param>
        /// <returns>Boolean</returns>
        public bool MovieExists(string movieName)
        {
            bool movieExists = _db.Movie.Any(m => m.Name.ToLower().Trim() == movieName.ToLower().Trim());
            return movieExists;
        }

        /// <summary>
        /// Validates if a Movie exists by Movie ID
        /// </summary>
        /// <param name="MovieId"></param>
        /// <returns></returns>
        public bool MovieExists(int movieId)
        {
            bool movieExists = _db.Movie.Any(m => m.Id == movieId);
            return movieExists;
        }

        /// <summary>
        /// Creates a Movie
        /// </summary>
        /// <param name="Movie"></param>
        /// <returns>Boolean</returns>
        public bool CreateMovie(Movie movie)
        {
            movie.CreatedOn = DateTime.Now;
            _db.Movie.Add(movie);
            return Save();
        }

        /// <summary>
        /// Deletes a Movie
        /// </summary>
        /// <param name="Movie"></param>
        /// <returns>Boolean</returns>
        public bool DeleteMovie(Movie movie)
        {
            _db.Movie.Remove(movie);
            return Save();
        }

        /// <summary>
        /// Gets all the movies
        /// </summary>
        /// <returns>A collection of movies</returns>
        public ICollection<Movie> GetMovies()
        {
            return _db.Movie.OrderBy(m => m.Name).ToList();
        }

        /// <summary>
        /// Gets a Movie by MovieId
        /// </summary>
        /// <param name="MovieId"></param>
        /// <returns></returns>
        public Movie GetMovie(int movieId)
        {
            return _db.Movie.FirstOrDefault(m => m.Id == movieId);
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
        /// Updates a Movie
        /// </summary>
        /// <param name="Movie"></param>
        /// <returns>Boolean</returns>
        public bool UpdateMovie(Movie movie)
        {
            movie.CreatedOn = DateTime.Now;
            _db.Movie.Update(movie);
            return Save();
        }

        /// <summary>
        /// Gets movies by category ID
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>A collection of movies</returns>
        public ICollection<Movie> GetMoviesByCategoryId(int categoryId)
        {
            return _db.Movie.Include(movie => movie.Category).Where(movie => movie.CategoryId == categoryId).ToList();
        }

        /// <summary>
        /// Searches movies by movie name
        /// </summary>
        /// <param name="movieName"></param>
        /// <returns></returns>
        public ICollection<Movie> SearchMovie(string movieName)
        {
            //IQueryable: Significa que podemos hacer consultas sobre peliculas
            IQueryable<Movie> query = _db.Movie;

            if (!string.IsNullOrEmpty(movieName))
            {
                query = query.Where(movie => movie.Name.Contains(movieName) || movie.Description.Contains(movieName));
            }

            return query.ToList();
        }
    }
}
