﻿using BookingProject.Application.CustomExceptions;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BookingProject.Application.Features.Commands.UserCommands.UserUpdatePasswordCommands;

public class UserUpdatePasswordCommandHandler : IRequestHandler<UserUpdatePasswordCommandRequest, UserUpdatePasswordCommandResponse>
{
	private readonly UserManager<AppUser> _userManager;

	public UserUpdatePasswordCommandHandler(UserManager<AppUser> userManager)
    {
		_userManager = userManager;
	}
    public async Task<UserUpdatePasswordCommandResponse> Handle(UserUpdatePasswordCommandRequest request, CancellationToken cancellationToken)
	{
		AppUser user=await _userManager.FindByIdAsync(request.AppUserId);
		if (user is null) throw new NotFoundException("User not found");
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