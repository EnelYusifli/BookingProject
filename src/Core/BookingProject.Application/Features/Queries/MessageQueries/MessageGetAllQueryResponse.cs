namespace BookingProject.Application.Features.Queries.MessageQueries;

public class MessageGetAllQueryResponse
{
	public DateTime CreatedDate { get; set; } = DateTime.Now;
	public DateTime ModifiedDate { get; set; } = DateTime.Now;
	public string Name { get; set; }
	public int Id { get; set; }
	public bool IsReplied { get; set; }
	public string Email { get; set; }
	public string MessageText { get; set; }
}
