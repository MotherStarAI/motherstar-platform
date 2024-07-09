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
    public class CreatePageAuditRequestCommand : IAppRequest<PageAuditRequestedResponse>, IPageAuditRequest
    {
        public CreatePageAuditRequestCommand()
        {

        }

        public Guid LighthouseProfileId { get; set; }
        public string PageUrl { get; set; }
        public bool IncludeAccessibilityAudit { get; set; }
        public bool IncludePerformanceAudit { get; set; }
        public bool IncludeSeoAudit { get; set; }
        public bool IncludeBestPracticesAudit { get; set; }
        public BrowserOptions Device { get; set; }
        public string CreatedByEmail { get; set; }
    }
}
