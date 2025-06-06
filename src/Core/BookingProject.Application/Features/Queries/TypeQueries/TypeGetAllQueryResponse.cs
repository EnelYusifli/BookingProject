﻿namespace BookingProject.Application.Features.Queries.TypeQueries;

public class TypeGetAllQueryResponse
{
    public int Id { get; set; }
    public string TypeName { get; set; }
    public bool IsDeactive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
