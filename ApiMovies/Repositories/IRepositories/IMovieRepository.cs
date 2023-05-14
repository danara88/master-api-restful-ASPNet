using ApiMovies.Entities;

namespace ApiMovies.Repositories.IRepositories
{
    /// <summary>
    /// Movie repository interface
    /// </summary>
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();

        Movie GetMovie(int movieId);

        bool MovieExists(string movieName);

        bool MovieExists(int movieId);

        bool CreateMovie(Movie movie);

        bool UpdateMovie(Movie movie);

        bool DeleteMovie(Movie movie);

        ICollection<Movie> GetMoviesByCategoryId(int categoryId);

        ICollection<Movie> SearchMovie(string movieName);

        bool Save();
    }
}
