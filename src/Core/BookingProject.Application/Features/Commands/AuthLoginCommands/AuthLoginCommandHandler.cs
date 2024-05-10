using BookingProject.Application.CustomExceptions;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingProject.Application.Features.Commands.AuthCommands.AuthLoginCommands
{
    public class AuthLoginCommandHandler : IRequestHandler<AuthLoginCommandRequest, AuthLoginCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthLoginCommandHandler(
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                IConfiguration configuration,
				IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}

        public async Task<AuthLoginCommandResponse> Handle(AuthLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new BadRequestException("Invalid credentials. Please try again.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            if (!result.Succeeded)
            {
                throw new BadRequestException("Invalid credentials. Please try again.");
            }

            string token = await GenerateToken(user,_httpContextAccessor.HttpContext);

            return new AuthLoginCommandResponse()
            {
                UserName = user.UserName,
                Token = token
            };
        }

        private async Task<string> GenerateToken(AppUser user, HttpContext httpContext)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var date = DateTime.UtcNow;
			var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(4),
                SigningCredentials = credentials,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
				NotBefore = date
			};
			tokenDescriptor.Expires = date.AddMinutes(10);
			var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
			var encrypterToken = tokenHandler.WriteToken(token);
            httpContext.Response.Cookies.Append("token", encrypterToken,
                new CookieOptions
                {
                    Expires=DateTime.UtcNow.AddDays(4),
                    HttpOnly=true,
                    Secure=true,
                    IsEssential=true,
                    SameSite=SameSiteMode.None
				});
            return encrypterToken;
        }
    }
}
