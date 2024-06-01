using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.DTOs;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using BookingProject.MVC.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;

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
}


