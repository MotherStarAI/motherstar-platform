using RCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse
{
    public class LighthouseProfileDetailResponse : IModel
    {
        public LighthouseProfileDetailResponse()
        {

        }

        public Guid LighthouseProfileId { get; set; }

        public DateTime CreatedDate { get; set; }
        public string WebsiteUrl { get; set; }
        public string CreatedByEmail { get; set; }

        public PageAuditListModelResponse WebPages { get; set; }

        public PageAuditResponse HomePageAudit { get; set; }
    }
}
