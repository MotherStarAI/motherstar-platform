using RCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse
{
    public class PageAuditRequestedResponse : IModel
    {

        public PageAuditRequestedResponse()
        {

        }

        public Guid LighthouseProfileId { get; set; }
        public Guid PageAuditId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid StatusId { get; set; }
        public double? Score { get; set; }
        public string PageUrl { get; set; }
        public Guid PageAuditRequestId { get; set; }
        public string AuditReport { get; set; }
        public int? Device { get; set; }
    }
}
