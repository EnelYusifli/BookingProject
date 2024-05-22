using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using Google;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.UserCardCommands;

public class CardCreateCommandHandler : IRequestHandler<CardCreateCommandRequest, CardCreateCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly UserManager<AppUser> _userManager;
	private readonly ICardRepository _cardRepository;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CardCreateCommandHandler(IMapper mapper, UserManager<AppUser> userManager,ICardRepository cardRepository,IHttpContextAccessor httpContextAccessor)
	{
		_mapper = mapper;
		_userManager = userManager;
		_cardRepository = cardRepository;
		_httpContextAccessor = httpContextAccessor;
	}
	public async Task<CardCreateCommandResponse> Handle(CardCreateCommandRequest request, CancellationToken cancellationToken)
	{
		if (request.ExpireYear < DateTime.Now.Year ||
	(request.ExpireYear == DateTime.Now.Year && request.ExpireMonth < DateTime.Now.Month))
			throw new BadRequestException("Card expired");
		AppUser user = new();
		if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
		{
			user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
		}
		if (user is null)
			throw new NotFoundException("User not found");

		var existingUserCard = await _cardRepository.Table.FirstOrDefaultAsync(uc => uc.AppUserId == user.Id && uc.CardNumber==request.CardNumber && uc.IsDeactive==false && uc.CVC == request.CVC && uc.ExpireMonth == request.ExpireMonth && uc.ExpireYear == request.ExpireYear);
		if (existingUserCard is not null)
			throw new BadRequestException("User already owns that card");
		var deactivatedUserCard = await _cardRepository.Table.FirstOrDefaultAsync(uc => uc.AppUserId == user.Id && uc.CardNumber == request.CardNumber && uc.IsDeactive == true && uc.CVC==request.CVC && uc.ExpireMonth==request.ExpireMonth && uc.ExpireYear==request.ExpireYear);
		if (deactivatedUserCard is not null)
		{
			deactivatedUserCard.IsDeactive = false;
			await _cardRepository.CommitAsync();
			return new CardCreateCommandResponse()
			{
				Text="Card activated"
			};

		}
		var userCard = _mapper.Map<UserCard>(request);
		userCard.AppUser = user;
		userCard.AppUserId = user.Id;
		userCard.CreatedDate = DateTime.Now;
		userCard.ModifiedDate = DateTime.Now;

		await _cardRepository.CreateAsync(userCard);
		await _cardRepository.CommitAsync();

		return new CardCreateCommandResponse();
	}
}
