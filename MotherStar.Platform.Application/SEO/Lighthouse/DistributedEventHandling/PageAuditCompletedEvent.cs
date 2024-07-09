using Google.Apis.PagespeedInsights.v5.Data;
using RCommon.EventHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.SEO.Lighthouse.DistributedEventHandling
{
    public record PageAuditCompletedEvent : ISyncEvent
    {
        public PageAuditCompletedEvent(Guid pageAuditRequestId, PagespeedApiPagespeedResponseV5 lighthouseResults)
        {
            PageAuditRequestId = pageAuditRequestId;
            LighthouseResults = lighthouseResults;
        }

        public Guid PageAuditRequestId { get; }
        public PagespeedApiPagespeedResponseV5 LighthouseResults { get; set; }
    }
}
