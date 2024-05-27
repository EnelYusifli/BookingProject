using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Commands.DiscountCommands.DiscountCreateCommands;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookingProject.Application.Features.Commands.DiscountCommands
{
	public class DiscountCreateCommandHandler : IRequestHandler<DiscountCreateCommandRequest, DiscountCreateCommandResponse>
	{
		private readonly IDiscountRepository _repository;
		private readonly IMapper _mapper;
		private readonly IRoomRepository _roomRepository;

		public DiscountCreateCommandHandler(IDiscountRepository repository, IMapper mapper,IRoomRepository roomRepository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_roomRepository = roomRepository;
		}

		public async Task<DiscountCreateCommandResponse> Handle(DiscountCreateCommandRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
				throw new ArgumentNullException(nameof(request));

			if (request.RoomId <= 0)
				throw new BadRequestException("Invalid room ID");
			var room=_roomRepository.Table.Include(x=>x.Discounts).FirstOrDefault(x=>x.Id==request.RoomId);
			if (room is null)
				throw new NotFoundException("Room not found");
			if (room.Discounts.Any(x => (x.StartTime <= request.StartTime && x.EndTime >= request.StartTime) ||
							  (x.StartTime <= request.EndTime && x.EndTime >= request.EndTime) ||
							  (x.StartTime >= request.StartTime && x.EndTime <= request.EndTime)))
				throw new BadRequestException("There is already a discount for this room during the specified time range.");

			if (request.Percent < 0 || request.Percent > 100)
				throw new BadRequestException("Invalid percent value. It must be between 0 and 100.");

			if (request.StartTime >= request.EndTime)
				throw new BadRequestException("End time must be greater than start time");
			if (request.StartTime < DateTime.Now)
				throw new BadRequestException("Start time must be in the future");

			var discount = _mapper.Map<Discount>(request);

			await _repository.CreateAsync(discount);
			await _repository.CommitAsync();

			return new DiscountCreateCommandResponse();
		}
	}
}
