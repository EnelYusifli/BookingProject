using AutoMapper;
using BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodCreateCommands;
using BookingProject.Application.Features.Commands.PaymentMethodCommands.PaymentMethodUpdateCommands;
using BookingProject.Application.Features.Queries.PaymentMethodQueries;
using BookingProject.Domain.Entities;

namespace BookingProject.Application.Mappings;

public class PaymentMethodMappingProfile : Profile
{
    public PaymentMethodMappingProfile()
    {
        CreateMap<PaymentMethodCreateCommandRequest, PaymentMethod>().ReverseMap();
        CreateMap<PaymentMethodUpdateCommandRequest, PaymentMethod>().ReverseMap();
        CreateMap<PaymentMethodGetAllQueryResponse, PaymentMethod>().ReverseMap();
        CreateMap<PaymentMethodGetByIdQueryResponse, PaymentMethod>().ReverseMap();
    }
}