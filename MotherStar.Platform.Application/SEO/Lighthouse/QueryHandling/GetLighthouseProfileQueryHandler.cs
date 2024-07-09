using AutoMapper;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Queries;
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
    public class GetLighthouseProfileQueryHandler
        : IAppRequestHandler<GetLighthouseProfileQuery, LighthouseProfileResponse>
    {
        private readonly IMapper _mapper;
        private readonly IReadOnlyRepository<LighthouseProfile> _lighthouseProfileRepository;

        public GetLighthouseProfileQueryHandler(IMapper mapper, IReadOnlyRepository<LighthouseProfile> lighthouseProfileRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _lighthouseProfileRepository = lighthouseProfileRepository ?? throw new ArgumentNullException(nameof(lighthouseProfileRepository));
        }

        public async Task<LighthouseProfileResponse> HandleAsync(GetLighthouseProfileQuery request,
            CancellationToken cancellationToken = default)
        {
            var profile = await _lighthouseProfileRepository.FindAsync(request.Id, cancellationToken);

            return _mapper.Map<LighthouseProfileResponse>(profile);
        }
    }
}
