﻿using MediatR;

namespace BookingProject.Application.Features.Queries.HotelQueries;

public class HotelGetAllByUserQueryRequest:IRequest<ICollection<HotelGetAllByUserQueryResponse>>
{
    public string Id { get; set; }  
}
