using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Application.Services.Interfaces;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace BookingProject.Application.Features.Commands.UserCommands.UserUpdateCommands;

public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommandRequest, UserUpdateCommandResponse>
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IEmailService _emailService;
	private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;
	//private readonly IConfiguration _configuration;
	private readonly ICloudinaryService _cloudinaryService;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public UserUpdateCommandHandler(UserManager<AppUser> userManager
		,IEmailService emailService
		,IMapper mapper
		,IUserRepository userRepository
		, IHttpContextAccessor httpContextAccessor
		, /*IConfiguration configuration*/
		ICloudinaryService cloudinaryService)
    {
		_userManager = userManager;
		_emailService = emailService;
		_mapper = mapper;
		_userRepository = userRepository;
		//_configuration = configuration;
		_cloudinaryService = cloudinaryService;
		_httpContextAccessor = httpContextAccessor;
	}
    public async Task<UserUpdateCommandResponse> Handle(UserUpdateCommandRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			throw new BadRequestException("Request not found");
		AppUser user = await _userManager.FindByIdAsync(request.Id);
		if (user is null)
			throw new NotFoundException("User not found");
		AppUser existUser = await _userManager.FindByEmailAsync(request.Email);
		if (existUser is not null && existUser.Email.ToLower() != user.Email.ToLower())
			throw new ConflictEmailException("Email already exists");
		string email = user.Email;
		AppUser existUser2 = await _userManager.FindByNameAsync(request.UserName);
		if (existUser2 is not null && existUser2.UserName.ToLower() != user.UserName.ToLower())
			throw new ConflictUserNameException("Username already exists");
		if (request.ProfilePhoto is not null)
		{
			//SaveFileExtension.Initialize(_configuration);
			string url = await _cloudinaryService.FileCreateAsync(request.ProfilePhoto);
			//string url = await SaveFileExtension.SaveFile(request.ProfilePhoto, "/userpps");
			if (user.ProfilePhotoUrl is not null)
				await _cloudinaryService.FileDeleteAsync(user.ProfilePhotoUrl);
			user.ProfilePhotoUrl = url;
		}
		user = _mapper.Map(request, user);
		await _userManager.UpdateAsync(user);
		if (email.ToLower() != user.Email.ToLower())
		{
            string subject = "Updated Email Address";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email", "updatedemail.html");
            string html = File.ReadAllText(filePath);
            html = html.Replace("{{email}}", request.Email);
            await _emailService.SendEmail(user.Email, subject, html);
        }
		return new UserUpdateCommandResponse();
	}
}
