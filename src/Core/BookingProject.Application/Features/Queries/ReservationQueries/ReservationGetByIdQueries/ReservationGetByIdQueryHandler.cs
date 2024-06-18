using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.ReservationQueries.ReservationGetByIdQueries;

public class ReservationGetByIdQueryHandler : IRequestHandler<ReservationGetByIdQueryRequest, ReservationGetByIdQueryResponse>
{
	private readonly IReservationRepository _repository;
	private readonly IMapper _mapper;

	public ReservationGetByIdQueryHandler(IReservationRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<ReservationGetByIdQueryResponse> Handle(ReservationGetByIdQueryRequest request, CancellationToken cancellationToken)
	{
		Reservation reservation = await _repository.Table
			.Include(x=>x.Room).ThenInclude(x=>x.RoomImages).Include(x=>x.Room).ThenInclude(x=>x.Hotel).FirstOrDefaultAsync(x=>x.Id==request.Id);
		if (reservation is null) throw new Exception("Reservation not found");
		ReservationGetByIdQueryResponse dto = _mapper.Map<ReservationGetByIdQueryResponse>(reservation);
		return dto;
	}
}
