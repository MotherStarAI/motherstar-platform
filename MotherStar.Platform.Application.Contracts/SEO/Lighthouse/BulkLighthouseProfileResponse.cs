using RCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse
{
    public class BulkLighthouseProfileResponse : IModel
    {
        public BulkLighthouseProfileResponse()
        {
            LighthouseProfileResponses = new List<LighthouseProfileResponse>();
        }

        public List<LighthouseProfileResponse> LighthouseProfileResponses { get; set; }
    }
}
