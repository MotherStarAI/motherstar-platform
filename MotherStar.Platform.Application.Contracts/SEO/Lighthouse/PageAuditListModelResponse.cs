using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using RCommon.Collections;
using RCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse
{
    public record PageAuditListModelResponse : PaginatedListModel<PageAudit, PageAuditRequestedResponse>
    {
        public PageAuditListModelResponse(IPaginatedList<PageAudit> source, PaginatedListRequest paginatedListRequest, int totalCount,
            bool skipSort = false)
            : base(source, paginatedListRequest, totalCount, skipSort)
        {
        }

        protected override IQueryable<PageAuditRequestedResponse> CastItems(IQueryable<PageAudit> source)
        {
            return source.Select(x => new PageAuditRequestedResponse
            {
                LighthouseProfileId = x.Id,
                PageAuditRequestId = x.PageAuditRequestId,
                StatusId = x.StatusId,
                PageAuditId = x.Id,
                CreatedDate = x.CreatedDate,
                Score = x.Score,
                PageUrl = x.PageUrl,
                AuditReport = x.AuditReport,
                Device = x.Device
            });
        }
    }
}
