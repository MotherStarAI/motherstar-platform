using AutoMapper;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.SEO
{
    public class SeoAutoMapperProfile : Profile
    {
        public SeoAutoMapperProfile()
        {
            CreateMap<LighthouseProfile, LighthouseProfileResponse>()
                .ForMember(x => x.LighthouseProfileId, m => m.MapFrom(f => f.Id));
            CreateMap<LighthouseProfile, LighthouseProfileDetailResponse>()
                .ForMember(x => x.LighthouseProfileId, m => m.MapFrom(f => f.Id));
            CreateMap<PageAuditRequest, LighthouseProfileResponse>()
                .ForMember(x => x.PageAuditRequestId, m => m.MapFrom(f => f.Id))
                .ForMember(x => x.LighthouseProfileId, m => m.MapFrom(f => f.LighthouseProfileId));
            CreateMap<PageAuditRequest, PageAuditRequestedResponse>()
               .ForMember(x => x.PageAuditRequestId, m => m.MapFrom(f => f.Id))
               .ForMember(x => x.LighthouseProfileId, m => m.MapFrom(f => f.LighthouseProfileId));
            CreateMap<PageAudit, PageAuditResponse>()
                .ForMember(x => x.PageAuditId, m => m.MapFrom(f => f.Id))
                .ForMember(x => x.LighthouseProfileId, m => m.MapFrom(f => f.PageAuditRequest.LighthouseProfileId));

        }
    }
}
