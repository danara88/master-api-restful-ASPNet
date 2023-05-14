using ApiMovies.Entities;
using ApiMovies.Entities.DTOs;
using ApiMovies.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiMovies.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRespository;
        private readonly IMapper _mapper;
        protected ApiResponse _apiResponse;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRespository = userRepository;
            this._apiResponse = new();
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsers()
        {
            var users = _userRespository.GetUsers().ToList();
            var usersDto = _mapper.Map<List<UserDto>>(users);

            return Ok(usersDto);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{userId:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(int userId)
        {
            var user = _userRespository.GetUser(userId);

            if (user is null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            bool isUniqueUser = _userRespository.IsUniqueUser(userRegisterDto.UserName);

            if (!isUniqueUser)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add($"Username '{userRegisterDto.UserName} already exists.'");
                return BadRequest(_apiResponse);
            }

            var user = await _userRespository.Register(userRegisterDto);
            if (user is null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add($"The was an error in the description.'");
                return BadRequest(_apiResponse);
            }

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var loginResponse = await _userRespository.Login(userLoginDto);

            if (loginResponse.User is null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add($"Incorrect credentials.");
                return BadRequest(_apiResponse);
            }

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = loginResponse;
            return Ok(_apiResponse);
        }
    }
}
