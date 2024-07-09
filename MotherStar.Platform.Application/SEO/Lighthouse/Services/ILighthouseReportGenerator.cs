using Google.Apis.PagespeedInsights.v5.Data;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.SEO.Lighthouse.Services
{
    public interface ILighthouseReportGenerator
    {
        Task<PagespeedApiPagespeedResponseV5> RunLighthousePageAudits(Guid pageAuditRequestId, IList<LighthouseAuditTypeEnum> auditTypes,
            string url, BrowserOptions browserOptions, string lighthouseApiKey, CancellationToken cancellationToken);
    }
}
