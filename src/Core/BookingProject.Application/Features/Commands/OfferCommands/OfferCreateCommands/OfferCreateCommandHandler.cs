using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.OfferCommands.OfferCreateCommands;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Commands.OfferCommands
{
	public class OfferCreateCommandHandler : IRequestHandler<OfferCreateCommandRequest, OfferCreateCommandResponse>
	{
		private readonly IOfferRepository _repository;
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;

		public OfferCreateCommandHandler(IOfferRepository repository, IMapper mapper, IUserRepository userRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
		}

		public async Task<OfferCreateCommandResponse> Handle(OfferCreateCommandRequest request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (request.Percent < 0 || request.Percent > 100)
				throw new BadRequestException("Invalid percent value. It must be between 0 and 100.");

			if (request.StartTime >= request.EndTime)
				throw new BadRequestException("End time must be greater than start time");

			if (request.StartTime < DateTime.Now)
				throw new BadRequestException("Start time must be in the future");

			var offer = _mapper.Map<Offer>(request);
			string code = offer.Code ?? CodeGenerator.GenerateRandomCode(7);

			var existingOffer = await _repository.Table.AnyAsync(x => x.Code == code);
			while (existingOffer)
			{
				code = CodeGenerator.GenerateRandomCode(7);
				existingOffer = await _repository.Table.AnyAsync(x => x.Code == code);
			}

			offer.Code = code;
			await _repository.CreateAsync(offer);
			await _repository.CommitAsync();

			return new OfferCreateCommandResponse() { Text=code};
		}
		public static class CodeGenerator
		{
			private static readonly Random Random = new Random();

			public static string GenerateRandomCode(int length)
			{
				const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
				return new string(Enumerable.Repeat(chars, length)
											.Select(s => s[Random.Next(s.Length)]).ToArray());
			}
		}

	}
}
