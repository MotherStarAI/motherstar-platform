using MediatR;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using RCommon.Mediator.Subscribers;
using RCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Queries
{
    public class SearchPageAuditScoreHistoryQuery : PaginatedListRequest, IAppRequest<PageAuditScoreHistoryListModelResponse>
    {
        public SearchPageAuditScoreHistoryQuery()
        {

        }

        public Guid? LighthouseProfileId { get; set; }

        public string SearchPageUrl { get; set; }

        public bool IncludeAuditDetails { get; set; }
    }
}
