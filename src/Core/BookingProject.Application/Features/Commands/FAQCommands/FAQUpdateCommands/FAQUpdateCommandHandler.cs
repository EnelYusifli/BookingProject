using AutoMapper;
using BookingProject.Application.CustomExceptions;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookingProject.Application.Features.Commands.FAQCommands.FAQUpdateCommands
{
    public class FAQUpdateCommandHandler : IRequestHandler<FAQUpdateCommandRequest, FAQUpdateCommandResponse>
    {
        private readonly IFAQsRepository _repository;
        private readonly IMapper _mapper;

        public FAQUpdateCommandHandler(IFAQsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FAQUpdateCommandResponse> Handle(FAQUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            FAQ faq = await _repository.GetByIdAsync(request.Id);
            if (faq is null)
            {
                throw new NotFoundException("FAQ not found");
            }
            if (string.IsNullOrEmpty(request.Question))
            {
                throw new BadRequestException("Question cannot be null or empty");
            }
            if (string.IsNullOrEmpty(request.Answer))
            {
                throw new BadRequestException("Answer cannot be null or empty");
            }

            FAQ existingFAQ = await _repository.Table.FirstOrDefaultAsync(x => x.Question.ToLower() == request.Question.ToLower(), cancellationToken);
            if (existingFAQ != null && existingFAQ.Id != faq.Id)
            {
                throw new BadRequestException("A FAQ with the same question already exists");
            }

            faq = _mapper.Map(request, faq);
            await _repository.CommitAsync();

            return new FAQUpdateCommandResponse();
        }
    }
}
