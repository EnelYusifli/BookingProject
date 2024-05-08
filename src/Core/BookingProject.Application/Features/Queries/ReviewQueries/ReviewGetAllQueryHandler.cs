using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Features.Queries.ReviewQueries;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Application.Features.Queries.ReviewQueries;

public class ReviewGetAllQueryHandler : IRequestHandler<ReviewGetAllQueryRequest, ICollection<ReviewGetAllQueryResponse>>
{
	private readonly IReviewRepository _repository;
	private readonly IMapper _mapper;
	private readonly IHotelRepository _hotelRepository;

	public ReviewGetAllQueryHandler(IReviewRepository repository, IMapper mapper, IHotelRepository hotelRepository)
	{
		_repository = repository;
		_mapper = mapper;
		_hotelRepository = hotelRepository;
	}

	public async Task<ICollection<ReviewGetAllQueryResponse>> Handle(ReviewGetAllQueryRequest request, CancellationToken cancellationToken)
	{
		Hotel hotel = await _hotelRepository.Table.FirstOrDefaultAsync(x => x.Id == request.HotelId);
		if (hotel is null)
			throw new NotFoundException("Hotel not found");
		ICollection<CustomerReview> act = await _repository.Table
		   .Where(x => x.HotelId == request.HotelId)
		   .Include(x => x.ReviewImages)
		   .ToListAsync();
		if (act is null) throw new Exception("Review not found");
		ICollection<ReviewGetAllQueryResponse> dtos = _mapper.Map<ICollection<ReviewGetAllQueryResponse>>(act);
		return dtos;
	}
}
