using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookingProject.Application.Features.Commands.FAQCommands.FAQDeleteCommands
{
    public class FAQDeleteCommandHandler : IRequestHandler<FAQDeleteCommandRequest, FAQDeleteCommandResponse>
    {
        private readonly IFAQsRepository _repository;

        public FAQDeleteCommandHandler(IFAQsRepository repository)
        {
            _repository = repository;
        }

        public async Task<FAQDeleteCommandResponse> Handle(FAQDeleteCommandRequest request, CancellationToken cancellationToken)
        {
            FAQ faq = await _repository.GetByIdAsync(request.Id);
            if (faq is null)
            {
                throw new NotFoundException("FAQ not found");
            }

            _repository.Delete(faq);
            await _repository.CommitAsync();

            return new FAQDeleteCommandResponse();
        }
    }
}
