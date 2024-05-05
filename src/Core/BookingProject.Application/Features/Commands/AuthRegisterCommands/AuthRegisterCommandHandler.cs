using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BookingProject.Application.Features.Commands.AuthCommands.AuthRegisterCommands;

public class AuthRegisterCommandHandler : IRequestHandler<AuthRegisterCommandRequest, AuthRegisterCommandResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;


    public AuthRegisterCommandHandler(UserManager<AppUser> userManager,IMapper mapper)
    {
        _userManager = userManager;
        _mapper=mapper;
    }

    public async Task<AuthRegisterCommandResponse> Handle(AuthRegisterCommandRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new BadRequestException("Request not found");
        }

        try
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                throw new ConflictException("Username already exists");
            }

            user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                throw new ConflictException("Email already exists");
            }

            var passwordRequirements = _userManager.Options.Password;

            if (request.Password.Length < passwordRequirements.RequiredLength)
            {
                throw new BadRequestException($"Password must be at least {passwordRequirements.RequiredLength} characters long");
            }

            //if (passwordRequirements.RequireNonAlphanumeric && !request.Password.Any(char.IsSymbol))
            //{
            //    throw new BadRequestException("Password must contain at least one non-alphanumeric character");
            //}
            if (passwordRequirements.RequireNonAlphanumeric)
            {
                bool hasNonAlphanumeric = false;
                foreach (char c in request.Password)
                {
                    if (!char.IsLetterOrDigit(c))
                    {
                        hasNonAlphanumeric = true;
                        break;
                    }
                }

                if (!hasNonAlphanumeric)
                {
                    throw new BadRequestException("Password must contain at least one non-alphanumeric character");
                }
            }


            if (passwordRequirements.RequireDigit && !request.Password.Any(char.IsDigit))
            {
                throw new BadRequestException("Password must contain at least one digit");
            }

            if (passwordRequirements.RequireLowercase && !request.Password.Any(char.IsLower))
            {
                throw new BadRequestException("Password must contain at least one lowercase letter");
            }

            if (passwordRequirements.RequireUppercase && !request.Password.Any(char.IsUpper))
            {
                throw new BadRequestException("Password must contain at least one uppercase letter");
            }
            var newUser = _mapper.Map<AppUser>(request);
            //var newUser = new AppUser
            //{
            //    UserName = request.UserName,
            //    Email = request.Email,
            //    FirstName = request.FirstName,
            //    LastName = request.LastName,
            //    Birthdate = request.Birthdate
            //};

            var userResult = await _userManager.CreateAsync(newUser, request.Password);

            if (userResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Customer");
                return new AuthRegisterCommandResponse();
            }
            else
            {
                var errors = string.Join(", ", userResult.Errors);
                throw new Exception($"Failed to register user. Errors: {errors}");
            }
        }
        catch (Exception ex)
        {
            if (ex is BadRequestException || ex is ConflictException)
            {
                throw;
            }
            else
            {
                throw new ServerErrorException("An error occurred while processing your request. Please try again later.");
            }
        }
    }
}
