using AutoMapper;
using BookingProject.Application.Features.Commands.CountryCommands.CountryCreateCommands;
using BookingProject.Application.Features.Commands.CountryCommands.CountryUpdateCommands;
using BookingProject.Application.Features.Queries.CountryQueries;

namespace BookingProject.Application.Mappings;

public class CountryMappingProfile : Profile
{
	public CountryMappingProfile()
	{
		CreateMap<CountryCreateCommandRequest, Domain.Entities.Country>().ReverseMap();
		CreateMap<CountryUpdateCommandRequest, Domain.Entities.Country>().ReverseMap();
		CreateMap<CountryGetAllQueryResponse, Domain.Entities.Country>().ReverseMap();
	}
}
