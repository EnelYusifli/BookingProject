﻿using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Commands.UserCommands.UserUpdatePasswordCommands;

public class UserUpdatePasswordCommandRequest:IRequest<UserUpdatePasswordCommandResponse>
{
	public required string AppUserId { get; set; }
	[DataType(DataType.Password)]
	public required string OldPassword { get; set; }
	[DataType(DataType.Password)]
	public required string NewPassword { get; set; }
	[DataType(DataType.Password)]
	[Compare("NewPassword")]
	public required string ConfirmNewPassword { get; set; }
}