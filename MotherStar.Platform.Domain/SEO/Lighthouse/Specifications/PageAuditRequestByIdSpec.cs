using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using RCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Domain.SEO.Lighthouse.Specifications
{
    public class PageAuditRequestByIdSpec : PagedSpecification<PageAuditRequest>
    {
        public PageAuditRequestByIdSpec(Guid lighthouseProfileId, Guid pageAuditRequestId,
            Expression<Func<PageAuditRequest, object>> orderByExpression, bool orderByAscending, int pageNumber, int pageSize)
            : base(audit => audit.LighthouseProfileId == lighthouseProfileId && audit.Id == pageAuditRequestId, orderByExpression,
                  orderByAscending, pageNumber, pageSize)
        {
        }
    }
}
