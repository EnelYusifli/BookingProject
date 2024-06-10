using AutoMapper;
using BookingProject.Application.Repositories;
using BookingProject.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookingProject.Application.Features.Queries.FAQQueries
{
    public class FAQGetAllQueryHandler : IRequestHandler<FAQGetAllQueryRequest, ICollection<FAQGetAllQueryResponse>>
    {
        private readonly IFAQsRepository _repository;
        private readonly IMapper _mapper;

        public FAQGetAllQueryHandler(IFAQsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<FAQGetAllQueryResponse>> Handle(FAQGetAllQueryRequest request, CancellationToken cancellationToken)
        {
            ICollection<FAQ> faqs = await _repository.GetAllAsync();
            if (faqs is null)
            {
                throw new Exception("FAQs not found");
            }

            ICollection<FAQGetAllQueryResponse> dtos = _mapper.Map<ICollection<FAQGetAllQueryResponse>>(faqs);
            return dtos;
        }
    }
}
