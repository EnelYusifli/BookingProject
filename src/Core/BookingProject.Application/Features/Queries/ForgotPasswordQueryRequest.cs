using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookingProject.Application.Features.Queries;

public class ForgotPasswordQueryRequest:IRequest<ForgotPasswordQueryResponse>
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
