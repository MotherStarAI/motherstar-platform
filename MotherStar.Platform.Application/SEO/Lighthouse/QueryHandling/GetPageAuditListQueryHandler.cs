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
    public class GetPageAuditListQueryHandler : IAppRequestHandler<GetPageAuditListQuery, PageAuditListModelResponse>
    {
        private readonly IGraphRepository<PageAudit> _pageAuditRepository;

        public GetPageAuditListQueryHandler(IGraphRepository<PageAudit> pageAuditRepository)
        {
            _pageAuditRepository = pageAuditRepository ?? throw new ArgumentNullException(nameof(pageAuditRepository));
        }
        public async Task<PageAuditListModelResponse> HandleAsync(GetPageAuditListQuery request, CancellationToken cancellationToken = default)
        {
            _pageAuditRepository.Include(x => x.PageAuditRequest);

            var profilePages = await _pageAuditRepository.FindAsync(x => (x.StatusId == PageAuditStatusConst.Completed ||
                                                                          x.StatusId == PageAuditStatusConst.Created) &&
                                                                          x.PageAuditRequest.LighthouseProfileId == request.LighthouseProfileId,
                                                                   request.DeriveOrderByExpression<PageAudit>(), request.SortDirection.ToBoolean(),
                                                                      request.PageNumber, request.PageSize.Value, cancellationToken);

            return new PageAuditListModelResponse(profilePages, request, profilePages.TotalCount);
        }
    }
}
