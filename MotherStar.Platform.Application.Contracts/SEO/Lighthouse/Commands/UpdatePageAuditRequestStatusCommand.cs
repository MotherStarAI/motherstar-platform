using RCommon.Mediator.Subscribers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands
{
    [DataContract]
    public class UpdatePageAuditRequestStatusCommand : IAppRequest<PageAuditRequestedResponse>
    {
        public UpdatePageAuditRequestStatusCommand(Guid pageAuditRequestId, Guid statusId, Guid lighthouseProfileId)
        {
            PageAuditRequestId = pageAuditRequestId;
            StatusId = statusId;
            LighthouseProfileId = lighthouseProfileId;
        }

        public Guid PageAuditRequestId { get; }
        public Guid StatusId { get; }
        public Guid LighthouseProfileId { get; }
    }
}
