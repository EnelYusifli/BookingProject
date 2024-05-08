using MediatR;

namespace BookingProject.Application.Features.Commands.HotelCommands.HotelUpdateCommands;

public class HotelUpdateCommandHandler : IRequestHandler<HotelUpdateCommandRequest, HotelUpdateCommandResponse>
{
	public Task<HotelUpdateCommandResponse> Handle(HotelUpdateCommandRequest request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
