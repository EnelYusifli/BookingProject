using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Queries.DiscountQueries;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.DiscountQueries;

public class DiscountGetAllByRoomQueryHandler : IRequestHandler<DiscountGetAllByRoomQueryRequest, ICollection<DiscountGetAllByRoomQueryResponse>>
{
	private readonly IDiscountRepository _repository;
	private readonly IMapper _mapper;
	private readonly IRoomRepository _roomRepository;

	public DiscountGetAllByRoomQueryHandler(IDiscountRepository repository, IMapper mapper, IRoomRepository roomRepository)
	{
		_repository = repository;
		_mapper = mapper;
		_roomRepository = roomRepository;
	}
	public async Task<ICollection<DiscountGetAllByRoomQueryResponse>> Handle(DiscountGetAllByRoomQueryRequest request, CancellationToken cancellationToken)
	{
		Room room = await _roomRepository.Table.FirstOrDefaultAsync(x => x.Id == request.RoomId);
		if (room is null)
			throw new NotFoundException("Room not found");
		ICollection<Discount> disc = await _repository.Table.Where(x => x.RoomId == request.RoomId).ToListAsync();
		if (disc is null) throw new Exception("Discount not found");
		ICollection<DiscountGetAllByRoomQueryResponse> dtos = _mapper.Map<ICollection<DiscountGetAllByRoomQueryResponse>>(disc);
		return dtos;
	}
}
