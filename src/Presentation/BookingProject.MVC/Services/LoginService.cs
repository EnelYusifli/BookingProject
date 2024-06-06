using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.DTOs;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using BookingProject.MVC.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;

namespace BookingProject.MVC.Services;

public class LoginService:ILoginService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenService _tokenService;

    public LoginService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
    }

    public async Task LoginUser(LoginViewModel request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null)
            user = await _userManager.FindByEmailAsync(request.UserName);

        if (user is null)
        {
            throw new BadRequestException("Invalid credentials. Please try again.");
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (!result.Succeeded)
        {
            throw new BadRequestException("Invalid credentials. Please try again.");
        }

        TokenDto dto = await _tokenService.CreateToken(true, user, _httpContextAccessor.HttpContext);

        //return new AuthLoginCommandResponse()
        //{
        //    UserName = user.UserName,
        //    Token = dto.AccessToken,
        //    RefreshToken = dto.RefreshToken,
        //};
    }
    public async Task LoginWithGoogle()
    {
        var result = await _httpContextAccessor.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!result.Succeeded)
        {
            throw new BadRequestException("Google authentication failed.");
        }

        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var nameIdentifier = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (email == null || nameIdentifier == null)
        {
            throw new BadRequestException("Google authentication did not return required information.");
        }

        var user = await _userManager.FindByEmailAsync(email);
        await _signInManager.SignInAsync(user, isPersistent: false);

        TokenDto dto = await _tokenService.CreateToken(true, user, _httpContextAccessor.HttpContext);
    }
    public async Task RegisterWithGoogle()
    {

		var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

		if (!authenticateResult.Succeeded)
		{
			throw new BadRequestException("Google authentication failed.");
		}

		var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
		var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
		var nameIdentifier = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		var givenName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
		var surname = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

		if (email == null || nameIdentifier == null)
		{
			throw new BadRequestException("Google authentication did not return required information.");
		}

		var user = await _userManager.FindByEmailAsync(email);
		if (user != null)
		{
			throw new ConflictException("Email already exists");
		}

		var newUser = new AppUser
		{
			UserName = email,
			Email = email,
			FirstName = givenName,
			LastName = surname
		};

		var userResult = await _userManager.CreateAsync(newUser);

		if (userResult.Succeeded)
		{
			await _userManager.AddToRoleAsync(newUser, "Customer");
			
		}
		else
		{
			var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));
			throw new Exception($"Failed to register user. Errors: {errors}");
		}
	}
}



