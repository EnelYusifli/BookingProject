namespace BookingProject.Application.Features.Queries.ActivityQueries;

public class ActivityGetAllQueryResponse
{
    public int Id { get; set; }
    public string ActivityName { get; set; }
    public bool IsDeactive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
