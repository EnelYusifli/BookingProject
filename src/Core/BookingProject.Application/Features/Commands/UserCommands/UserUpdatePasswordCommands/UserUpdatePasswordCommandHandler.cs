using BookingProject.Application.CustomExceptions;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BookingProject.Application.Features.Commands.UserCommands.UserUpdatePasswordCommands;

public class UserUpdatePasswordCommandHandler : IRequestHandler<UserUpdatePasswordCommandRequest, UserUpdatePasswordCommandResponse>
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public UserUpdatePasswordCommandHandler(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
		_userManager = userManager;
		_httpContextAccessor = httpContextAccessor;
	}
    public async Task<UserUpdatePasswordCommandResponse> Handle(UserUpdatePasswordCommandRequest request, CancellationToken cancellationToken)
	{
		AppUser user = new();
		if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
		{
			user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
		}
		if (user is null)
			throw new NotFoundException("User not found");
		var result=await _userManager.ChangePasswordAsync(user,request.OldPassword,request.NewPassword);
		if(!result.Succeeded)
		{
			var errors = new List<string>();	
			foreach (var error in result.Errors)
			{
				errors.Add(error.Description);
			}
				return new UserUpdatePasswordCommandResponse() { Text=string.Join(",",errors)};
		}
		return new UserUpdatePasswordCommandResponse();
	}
}
