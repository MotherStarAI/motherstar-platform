using Google.Apis.PagespeedInsights.v5.Data;
using RCommon.EventHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Domain.SEO.Lighthouse.Events
{
    public class PageAuditCompletedDomainEvent : ISyncEvent
    {

        public PageAuditCompletedDomainEvent(Guid pageAuditRequestId, LighthouseResultV5[] lighthouseResults)
        {
            PageAuditRequestId = pageAuditRequestId;
            LighthouseResults = lighthouseResults;
        }

        public Guid PageAuditRequestId { get; }
        public LighthouseResultV5[] LighthouseResults { get; }
    }
}
