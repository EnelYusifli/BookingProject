using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BookingProject.Application.Features.Commands.UserCommands.RefreshCommands;

public class RefreshCommandHandler : IRequestHandler<RefreshCommandRequest, RefreshCommandResponse>
{
	private readonly ITokenService _tokenService;
	private readonly UserManager<AppUser> _userManager;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public RefreshCommandHandler(ITokenService tokenService,UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
		_tokenService = tokenService;
		_userManager = userManager;
		_httpContextAccessor = httpContextAccessor;
	}
    public async Task<RefreshCommandResponse> Handle(RefreshCommandRequest request, CancellationToken cancellationToken)
	{
		var principal = _tokenService.GetPrincFromExpToken(request.Token.accessToken);
		var user=await _userManager.FindByNameAsync(principal.Identity.Name);
		if (user is null || user.RefreshToken != request.Token.refreshToken || user.RefreshTokenExpires <= DateTime.Now)
			throw new BadRequestException("Bad Request");
		var response=await _tokenService.CreateToken(true, user, _httpContextAccessor.HttpContext);
		return new RefreshCommandResponse() { 
			Token=response
		};
	}
}
