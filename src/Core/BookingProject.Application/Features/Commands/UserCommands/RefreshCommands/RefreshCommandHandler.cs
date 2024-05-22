// RefreshCommandHandler.cs
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.UserCommands.RefreshCommands;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookingProject.Application.Features.Commands.UserCommands.RefreshCommands
{
	public class RefreshCommandHandler : IRequestHandler<RefreshCommandRequest, RefreshCommandResponse>
	{
		private readonly ITokenService _tokenService;
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public RefreshCommandHandler(ITokenService tokenService, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_tokenService = tokenService;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<RefreshCommandResponse> Handle(RefreshCommandRequest request, CancellationToken cancellationToken)
		{
			var principal = _tokenService.GetPrincFromExpToken(request.Token.AccessToken);
			var user = await _userManager.FindByNameAsync(principal.Identity.Name);
			if (user == null || user.RefreshToken != request.Token.RefreshToken || user.RefreshTokenExpires <= DateTime.UtcNow)
				throw new BadRequestException("Bad Request");

			var response = await _tokenService.RefreshToken(user, _httpContextAccessor.HttpContext);

			return new RefreshCommandResponse()
			{
				Token = response
			};
		}
	}
}
