namespace BookingProject.Application.Features.Queries.StaffLanguageQueries;

public class StaffLanguageGetAllQueryResponse
{
    public int Id { get; set; }
    public string StaffLanguageName { get; set; }
    public bool IsDeactive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
