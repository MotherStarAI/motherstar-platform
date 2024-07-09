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
    public class LighthouseCustomerSearchSpec : PagedSpecification<LighthouseProfile>
    {
        public LighthouseCustomerSearchSpec(string websiteUrl,
            Expression<Func<LighthouseProfile, object>> orderByExpression, bool orderByAscending, int pageIndex, int pageSize)
            : base(customer => customer.WebsiteUrl.StartsWith(websiteUrl), orderByExpression, orderByAscending, pageIndex, pageSize)
        {
        }
    }
}
