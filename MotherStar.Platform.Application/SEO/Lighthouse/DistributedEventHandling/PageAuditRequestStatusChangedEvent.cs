using RCommon.EventHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.SEO.Lighthouse.DistributedEventHandling
{
    public record PageAuditRequestStatusChangedEvent : ISyncEvent
    {

        public PageAuditRequestStatusChangedEvent(Guid pageAuditRequestId, Guid statusId)
        {
            PageAuditRequestId = pageAuditRequestId;
            StatusId = statusId;
        }

        public Guid PageAuditRequestId { get; }
        public Guid StatusId { get; init; }
    }
}
