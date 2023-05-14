namespace ApiMovies.Entities.DTOs
{
    /// <summary>
    /// User login response DTO
    /// </summary>
    public class UserLoginResponseDto
    {
        public User User { get; set; }

        public string Token { get; set; }
    }
}
