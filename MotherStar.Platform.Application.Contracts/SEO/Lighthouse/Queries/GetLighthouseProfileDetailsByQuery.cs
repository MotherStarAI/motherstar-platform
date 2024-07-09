using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using RCommon.Mediator.Subscribers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Queries
{
    public class GetLighthouseProfileDetailsByQuery : IAppRequest<LighthouseProfileDetailResponse>
    {
        public GetLighthouseProfileDetailsByQuery(Guid id)
        {
            Id = id;
        }

        public GetLighthouseProfileDetailsByQuery()
        {

        }

        public Guid Id { get; set; }
    }
}
