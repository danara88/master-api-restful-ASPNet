using ApiMovies.Entities;
using ApiMovies.Entities.DTOs;
using AutoMapper;

namespace ApiMovies.MoviesMapper
{
    /// <summary>
    /// Movies Mapper
    /// </summary>
    public class MoviesMapper : Profile
    {
        public MoviesMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Category, CreateCategoryDto>().ReverseMap();

            CreateMap<Movie, MovieDto>().ReverseMap();

            CreateMap<Movie, CreateMovieDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
