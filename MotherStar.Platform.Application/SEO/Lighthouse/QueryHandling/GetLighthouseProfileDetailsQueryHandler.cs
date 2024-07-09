using AutoMapper;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Queries;
using MotherStar.Platform.Domain.SEO.Lighthouse.Exceptions;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using RCommon;
using RCommon.Mediator.Subscribers;
using RCommon.Persistence.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.SEO.Lighthouse.QueryHandling
{
    public class GetLighthouseProfileDetailsQueryHandler : IAppRequestHandler<GetLighthouseProfileDetailsByQuery, LighthouseProfileDetailResponse>
    {
        private readonly IMapper _mapper;
        private readonly IGraphRepository<PageAudit> _pageAuditRepository;
        private readonly IGraphRepository<LighthouseProfile> _lighthouseProfileRepository;

        public GetLighthouseProfileDetailsQueryHandler(IMapper mapper, IGraphRepository<PageAudit> pageAuditRepository, IGraphRepository<LighthouseProfile> _lighthouseProfileRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _pageAuditRepository = pageAuditRepository ?? throw new ArgumentNullException(nameof(pageAuditRepository));
            this._lighthouseProfileRepository = _lighthouseProfileRepository ?? throw new ArgumentNullException(nameof(_lighthouseProfileRepository));
        }


        public async Task<LighthouseProfileDetailResponse> HandleAsync(GetLighthouseProfileDetailsByQuery request,
            CancellationToken cancellationToken = default)
        {
            // Eager loading
            _pageAuditRepository.Include(x => x.PageAuditRequest);

            // Get the profile
            var profile = await _lighthouseProfileRepository.FindAsync(request.Id, cancellationToken);

            Guard.Against<LighthouseDomainException>(profile == null, string.Format("Lighthouse Profile does not exist with Id:{0}", request.Id));

            var profileDetail = _mapper.Map<LighthouseProfileDetailResponse>(profile);

            // Gets the home page audit
            var homePageAudit = _pageAuditRepository.FirstOrDefault(x => (x.StatusId == PageAuditStatusConst.Completed || x.StatusId == PageAuditStatusConst.Created)
                                                                && x.PageAuditRequest.LighthouseProfileId == request.Id);

            var homePageAuditResponse = _mapper.Map<PageAuditResponse>(homePageAudit);
            profileDetail.HomePageAudit = homePageAuditResponse;

            profileDetail.LighthouseProfileId = profile.Id;

            return profileDetail;
        }
    }
}
