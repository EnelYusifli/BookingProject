using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BookingProject.Application.Features.Commands.FAQCommands.FAQCreateCommands
{
    public class FAQCreateCommandHandler : IRequestHandler<FAQCreateCommandRequest, FAQCreateCommandResponse>
    {
        private readonly IFAQsRepository _repository;
        private readonly IMapper _mapper;

        public FAQCreateCommandHandler(IFAQsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FAQCreateCommandResponse> Handle(FAQCreateCommandRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new NotFoundException("Request not found");
            }
            if (string.IsNullOrEmpty(request.Question))
            {
                throw new BadRequestException("Question cannot be null or empty");
            }
            if (string.IsNullOrEmpty(request.Answer))
            {
                throw new BadRequestException("Answer cannot be null or empty");
            }
            if (await _repository.Table.AnyAsync(x => x.Question.ToLower() == request.Question.ToLower(), cancellationToken))
            {
                throw new BadRequestException("A FAQ with the same question already exists");
            }

            FAQ faq = _mapper.Map<FAQ>(request);
            await _repository.CreateAsync(faq);
            await _repository.CommitAsync();

            return new FAQCreateCommandResponse();
        }
    }
}
