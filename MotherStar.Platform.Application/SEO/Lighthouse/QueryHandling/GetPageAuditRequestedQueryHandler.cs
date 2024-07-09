using AutoMapper;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Queries;
using RCommon;
using RCommon.Mediator.Subscribers;
using RCommon.Persistence.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.SEO.Lighthouse.QueryHandling
{
    public class GetPageAuditRequestedQueryHandler : IAppRequestHandler<GetPageAuditRequestedQuery, PageAuditRequestedResponse>
    {
        private readonly IGraphRepository<PageAudit> _pageAuditRepository;
        private readonly IMapper _mapper;

        public GetPageAuditRequestedQueryHandler(IGraphRepository<PageAudit> pageAuditRepository, IMapper mapper)
        {
            _pageAuditRepository = pageAuditRepository ?? throw new ArgumentNullException(nameof(pageAuditRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PageAuditRequestedResponse> HandleAsync(GetPageAuditRequestedQuery request, CancellationToken cancellationToken = default)
        {
            Guard.Against<ArgumentException>(!request.Id.IsEmpty(), nameof(request.Id));

            _pageAuditRepository.Include(x => x.PageAuditItems);
            var pageAudit = await _pageAuditRepository.FindAsync(request.Id, cancellationToken);

            return _mapper.Map<PageAuditRequestedResponse>(pageAudit);
        }
    }
}
