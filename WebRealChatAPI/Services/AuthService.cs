using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebRealChatAPI.Context;
using WebRealChatAPI.Models;

namespace WebRealChatAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ChatDbContext _context;
        private readonly IConfiguration _configuration;

        // Constructor to inject database context and configuration
        public AuthService(ChatDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Method to handle user registration
        public AuthResultModel Register(RegisterRequestModel request)
        {
            try
            {
                // Check if the username is already taken
                if (_context.Users.Any(u => u.Username == request.Username))
                {
                    return new AuthResultModel { Success = false, Message = "Username is already taken." };
                }

                // Create new user with hashed password
                var user = new UserModel
                {
                    Username = request.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Name = request.Name
                };

                // Save user to database
                _context.Users.Add(user);
                _context.SaveChanges();

                return new AuthResultModel { Success = true, Message = "User registered successfully." };
            }
            catch (Exception ex)
            {
                return new AuthResultModel { Success = false, Message = $"Registration error: {ex.Message}" };
            }
        }

        // Method to handle user login
        public AuthResultModel Login(LoginRequestModel request)
        {
            try
            {
                // Find user by username
                var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);

                // Verify user and password
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new AuthResultModel { Success = false, Message = "Invalid username or password." };
                }

                // Generate JWT token for authenticated user
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("username", user.Username) }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new AuthResultModel { Success = true, Token = tokenHandler.WriteToken(token) };
            }
            catch (Exception ex)
            {
                return new AuthResultModel { Success = false, Message = $"Login error: {ex.Message}" };
            }
        }
    }
}
