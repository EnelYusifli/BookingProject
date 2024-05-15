using BookingProject.Domain.Entities;

namespace BookingProject.Application.Features.Queries.UserQueries;

public class GetUserQueryResponse
{
	public AppUser? User {  get; set; }
}
