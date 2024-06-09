using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingProject.Application.Features.Queries.MessageQueries;

public class MessageGetAllQueryRequest:IRequest<ICollection<MessageGetAllQueryResponse>>
{
}
