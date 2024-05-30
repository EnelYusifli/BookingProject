using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookingProject.Application.Features.Queries.ReviewQueries;

public class ReviewGetAllByOwnerQueryHandler:IRequestHandler<ReviewGetAllByOwnerQueryRequest, ICollection<ReviewGetAllQueryResponse>>
{
    private readonly IReviewRepository _repository;
    private readonly IMapper _mapper;
    private readonly IReviewImageRepository _reviewImageRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHotelRepository _hotelRepository;

    public ReviewGetAllByOwnerQueryHandler(IReviewRepository repository
        , IMapper mapper
        , IReviewImageRepository reviewImageRepository
        , UserManager<AppUser> userManager
        , IConfiguration configuration
        , IHttpContextAccessor httpContextAccessor
        , IReservationRepository reservationRepository
        ,IHotelRepository hotelRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _reviewImageRepository = reviewImageRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _hotelRepository = hotelRepository;
    }
    public async Task<ICollection<ReviewGetAllQueryResponse>> Handle(ReviewGetAllByOwnerQueryRequest request, CancellationToken cancellationToken)
    {
        AppUser user = null;
        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
        }
        if (user == null)
            throw new NotFoundException("User not found");

        var ownedHotels = await _hotelRepository.Table
            .Where(h => h.AppUserId == user.Id)
            .ToListAsync();

        if (ownedHotels == null || !ownedHotels.Any())
            throw new NotFoundException("Hotels not found for the user");

        var hotelIds = ownedHotels.Select(h => h.Id).ToList();
        var reviews = await _repository.Table
            .Where(r => hotelIds.Contains(r.HotelId))
            .Include(r => r.ReviewImages)
            .Include(x => x.User)
            .Include(r => r.Hotel)
            .ToListAsync();

        if (reviews == null || !reviews.Any())
            throw new NotFoundException("Reviews not found");

        var dtos = _mapper.Map<ICollection<ReviewGetAllQueryResponse>>(reviews);

        return dtos;
    }


}
