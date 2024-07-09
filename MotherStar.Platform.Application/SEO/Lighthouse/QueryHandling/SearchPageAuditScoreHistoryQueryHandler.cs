using LeadVenture.SharedServices.Lighthouse.Contracts;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using MotherStar.Platform.Application.Contracts.Extensions;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Queries;
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
    public class SearchPageAuditScoreHistoryQueryHandler : IAppRequestHandler<SearchPageAuditScoreHistoryQuery, PageAuditScoreHistoryListModelResponse>
    {
        private readonly IGraphRepository<PageAudit> _pageAuditRepository;

        public SearchPageAuditScoreHistoryQueryHandler(IGraphRepository<PageAudit> pageAuditRepository)
        {
            _pageAuditRepository = pageAuditRepository ?? throw new ArgumentNullException(nameof(pageAuditRepository));
        }

        public async Task<PageAuditScoreHistoryListModelResponse> HandleAsync(SearchPageAuditScoreHistoryQuery request, CancellationToken cancellationToken = default)
        {
            if (request.IncludeAuditDetails)
            {
                _pageAuditRepository.Include(x => x.PageAuditItems); // This is how we eager load items into the repository. 
            }
            var pageAudits = await _pageAuditRepository.FindAsync(x => x.PageUrl == request.SearchPageUrl, request.DeriveOrderByExpression<PageAudit>(), request.SortDirection.ToBoolean(),
                request.PageNumber, request.PageSize.Value, cancellationToken);

            return new PageAuditScoreHistoryListModelResponse(pageAudits, request, pageAudits.TotalCount);
        }
    }
}
