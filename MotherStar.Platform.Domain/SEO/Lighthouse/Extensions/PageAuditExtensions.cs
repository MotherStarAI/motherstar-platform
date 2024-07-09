using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Domain.SEO.Lighthouse.Extensions
{
    public static class PageAuditExtensions
    {

        public static PageAudit GenerateDefaultValues(this PageAudit pageAudit, string url)
        {
            pageAudit.StatusId = PageAuditStatusConst.NotInitiated;
            pageAudit.PageUrl = url;
            pageAudit.Score = 0.0;
            return pageAudit;
        }
    }
}
