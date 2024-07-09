using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse
{
    public interface IPageAuditRequest
    {

        public bool IncludeAccessibilityAudit { get; set; }
        public bool IncludePerformanceAudit { get; set; }
        public bool IncludeSeoAudit { get; set; }
        public bool IncludeBestPracticesAudit { get; set; }
    }
}
