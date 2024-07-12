using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.SEO.Lighthouse.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.SEO.Extensions
{
    internal static class IPageAuditRequestExtensions
    {

        public static List<LighthouseAuditTypeEnum> PackagePageAuditTypeList(this IPageAuditRequest request)
        {
            // Queue the audit as a background job.
            var auditTypes = new List<LighthouseAuditTypeEnum>();
            if (request.IncludeSeoAudit)
            {
                auditTypes.Add(LighthouseAuditTypeEnum.SEO);
            }
            if (request.IncludeAccessibilityAudit)
            {
                auditTypes.Add(LighthouseAuditTypeEnum.Accessbility);
            }
            if (request.IncludeBestPracticesAudit)
            {
                auditTypes.Add(LighthouseAuditTypeEnum.BestPractices);
            }
            if (request.IncludePerformanceAudit)
            {
                auditTypes.Add(LighthouseAuditTypeEnum.Performance);
            }

            return auditTypes;
        }
    }
}
