using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.SEO.Lighthouse.Services
{
    public interface ILighthouseAppService
    {
        Task RunLighthousePageAudits(Guid pageAuditRequestId, IList<LighthouseAuditTypeEnum> auditTypes, string url,
            BrowserOptions browserOptions, string lighthouseApiKey, CancellationToken cancellationToken);
    }
}