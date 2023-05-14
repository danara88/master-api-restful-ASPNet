using ApiMovies.Entities;
using ApiMovies.Entities.DTOs;
using ApiMovies.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMovies()
        {
            var movies = _movieRepository.GetMovies().ToList();
            var moviesDto = _mapper.Map<List<MovieDto>>(movies);

            return Ok(moviesDto);
        }

        [AllowAnonymous]
        [HttpGet("{movieId:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int movieId)
        {
            var movie = _movieRepository.GetMovie(movieId);

            if (movie is null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDto>(movie);
            return Ok(movieDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromBody] CreateMovieDto createMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createMovieDto is null)
            {
                return BadRequest(ModelState);
            }

            if (_movieRepository.MovieExists(createMovieDto.Name))
            {
                ModelState.AddModelError("", $"The movie {createMovieDto.Name} already exists.");
                return StatusCode(404, ModelState);
            }

            var movie = _mapper.Map<Movie>(createMovieDto);

            if (!_movieRepository.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"Something went wrong while saving the movie {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetMovie", new { movieId = movie.Id }, movie);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{movieId:int}", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMovie(int movieId, [FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (movieDto is null || movieDto.Id != movieId)
            {
                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(movieDto);

            if (!_movieRepository.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the movie {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{movieId:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMovie(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }

            var movie = _movieRepository.GetMovie(movieId);

            if (!_movieRepository.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the movie {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("get-movies-by-category/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMoviesByCategoryId(int categoryId)
        {
            var movies = _movieRepository.GetMoviesByCategoryId(categoryId);

            if (movies is null)
            {
                return NotFound();
            }

            var moviesDto = _mapper.Map<List<MovieDto>>(movies);

            return Ok(moviesDto);
        }

        [AllowAnonymous]
        [HttpGet("search-movies")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMoviesByCategoryId(string movieName)
        {
            try
            {
                var movies = _movieRepository.SearchMovie(movieName.Trim());

                if (movies.Any())
                {
                    var moviesDto = _mapper.Map<List<MovieDto>>(movies);

                    return Ok(moviesDto);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
            }
        }
    }
}
