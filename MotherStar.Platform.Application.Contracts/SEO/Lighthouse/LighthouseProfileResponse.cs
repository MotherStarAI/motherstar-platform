using RCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse
{
    public class LighthouseProfileResponse : IModel
    {
        public LighthouseProfileResponse()
        {

        }

        public Guid LighthouseProfileId { get; set; }

        public Guid PageAuditRequestId { get; set; }

        public Guid StatusId { get; set; }
    }
}
