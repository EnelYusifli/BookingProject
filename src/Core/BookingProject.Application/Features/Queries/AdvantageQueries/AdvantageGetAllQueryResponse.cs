namespace BookingProject.Application.Features.Queries.AdvantageQueries;

public class AdvantageGetAllQueryResponse
{
    public int Id { get; set; }
    public string AdvantageName { get; set; }
    public bool IsDeactive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
