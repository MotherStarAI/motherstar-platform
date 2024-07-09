using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using RCommon.Linq;
using RCommon;
using RCommon.Mediator.Subscribers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RCommon.Persistence.Crud;
using MotherStar.Platform.Application.Contracts.Extensions;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Queries;

namespace MotherStar.Platform.Application.SEO.Lighthouse.QueryHandling
{
    public class SearchPageAuditsQueryHandler : IAppRequestHandler<SearchPageAuditsQuery, PageAuditListModelResponse>
    {
        private readonly IGraphRepository<PageAudit> _pageAuditRepository;
        private readonly IMapper _mapper;

        public SearchPageAuditsQueryHandler(IGraphRepository<PageAudit> pageAuditRepository, IMapper mapper)
        {
            _pageAuditRepository = pageAuditRepository ?? throw new ArgumentNullException(nameof(pageAuditRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<PageAuditListModelResponse> HandleAsync(SearchPageAuditsQuery request, CancellationToken cancellationToken = default)
        {
            var predicate = PredicateBuilder.False<PageAudit>(); // This allows us to build compound expressions

            if (!request.SearchPageUrl.IsNullOrEmpty())
            {
                predicate = predicate.And(audit => audit.PageUrl.StartsWith(request.SearchPageUrl));
            }

            if (request.LighthouseProfileId.HasValue)
            {
                predicate = predicate.And(audit => audit.Id == request.LighthouseProfileId);
            }

            if (request.IncludeAuditDetails)
            {
                _pageAuditRepository.Include(x => x.PageAuditItems); // This is how we eager load items into the repository. 
            }

            var pageAudits = await _pageAuditRepository.FindAsync(predicate, request.DeriveOrderByExpression<PageAudit>(), request.SortDirection.ToBoolean(),
                request.PageNumber, request.PageSize.Value, cancellationToken);

            return new PageAuditListModelResponse(pageAudits, request, pageAudits.TotalCount);
        }
    }
}
