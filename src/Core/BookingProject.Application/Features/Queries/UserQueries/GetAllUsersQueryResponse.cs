namespace BookingProject.Application.Features.Queries.UserQueries;

public class GetAllUsersQueryResponse
{
	public string Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public int HotelCount { get; set; }
	public IList<string> Roles { get; set; }
	public string UserName { get; set; }
	public DateOnly? Birthdate { get; set; }
	public string? RecoveryEmail { get; set; }
	public string? ProfilePhotoUrl { get; set; }
}
