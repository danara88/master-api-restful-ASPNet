using ApiMovies.Data;
using ApiMovies.Entities;
using ApiMovies.Entities.DTOs;
using ApiMovies.Repositories.IRepositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiMovies.Repositories
{
    /// <summary>
    /// User repository
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string _secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUser(int userId)
        {
            return _db.User.FirstOrDefault(u => u.Id == userId);
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public ICollection<User> GetUsers()
        {
            return _db.User.OrderBy(u => u.UserName).ToList();
        }

        /// <summary>
        /// Validates if user is not in DB
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsUniqueUser(string userName)
        {
            var userDB = _db.User.FirstOrDefault(u => u.UserName == userName);

            return userDB is null;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="userRegisterDto"></param>
        /// <returns></returns>
        public async Task<User> Register(UserRegisterDto userRegisterDto)
        {
            // Encrypt password MD5
            var encryptedPassword = GetMD5Encryption(userRegisterDto.Password);

            // Create User instance
            var user = new User() 
            { 
                UserName = userRegisterDto.UserName,
                Name = userRegisterDto.Name,
                Password = encryptedPassword,
                Role = userRegisterDto.Role
            };

            // Add user to DB
            await _db.User.AddAsync(user);

            // Save transaction
            await _db.SaveChangesAsync();

            // Return registered user
            return user;
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="userLoginDto"></param>
        /// <returns></returns>
        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            var encryptedPassword = GetMD5Encryption(userLoginDto.Password);

            var user = _db.User.FirstOrDefault(u => u.UserName.ToLower() == userLoginDto.UserName.ToLower() && u.Password == encryptedPassword);

            if (user is null)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }

            // Create the JWT token
            var handleToken = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handleToken.CreateToken(tokenDescriptor);

            UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto()
            {
                User = user,
                Token = handleToken.WriteToken(token)
            };

            return userLoginResponseDto;
        }

        /// <summary>
        /// Encrypts password with MD5
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetMD5Encryption(string value)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
            {
                resp += data[i].ToString("x2").ToLower();
            }
            return resp;
        }
    }
}
