using AutoMapper;
using RCommon.Mediator.Subscribers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RCommon.Persistence.Crud;
using MotherStar.Platform.Domain;
using MotherStar.Platform.Application.SEO.Lighthouse.Jobs;
using MotherStar.Platform.Application.SEO.Lighthouse.Services;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using MotherStar.Platform.Application.SEO.Extensions;

namespace MotherStar.Platform.Application.SEO.Lighthouse.CommandHandling
{
    /// <summary>
    /// A command handler that implements <see cref="IRequestHandler{CreateLighthouseCustomerCommand, LighthouseCustomerResponse}"/>. 
    /// The DI registration is handled in the <see cref="ServiceCollectionExtensions.AddLightHouseServices"/> extension method.
    /// </summary>
    /// <remarks>Command Handlers interact with domain objects and repositories which will raise domain events as part of their 
    /// propety changes and peristence. Logging, validation, and transactions may be implemented as part of the <see cref="Mediator"/>
    /// pipeline but may also be handled here as well. Business rules and business logic should not be encapsulated in this layer.</remarks>
    public class CreateLighthouseProfileCommandHandler : IAppRequestHandler<CreateLighthouseProfileCommand, LighthouseProfileResponse>
    {
        private readonly IWriteOnlyRepository<LighthouseProfile> _profileRepository;
        private readonly ISystemTime _clock;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IMapper _mapper;
        private readonly ILighthouseAppService _lighthouseAppService;
        private readonly IGraphRepository<PageAuditRequest> _pageAuditRequestRepository;
        private readonly IGraphRepository<PageAudit> _pageAuditRepository;
        private readonly ISystemTime _systemTime;
        private readonly IConfiguration _configuration;

        public CreateLighthouseProfileCommandHandler(ILogger<CreatePageAuditRequestCommandHandler> logger, IWriteOnlyRepository<LighthouseProfile> profileRepository,
            IGraphRepository<PageAudit> pageAuditRepository, ISystemTime systemTime,
            ISystemTime clock, IGuidGenerator guidGenerator, IMapper mapper, ILighthouseAppService lighthouseAppService,
            IGraphRepository<PageAuditRequest> pageAuditRequestRepository, IConfiguration configuration)
        {
            _profileRepository = profileRepository;
            _profileRepository.DataStoreName = DataStoreNamesConst.SeoDb;
            _clock = clock;
            _guidGenerator = guidGenerator;
            _mapper = mapper;
            _lighthouseAppService = lighthouseAppService;
            _pageAuditRequestRepository = pageAuditRequestRepository;
            _pageAuditRequestRepository.DataStoreName = DataStoreNamesConst.SeoDb;
            _pageAuditRepository = pageAuditRepository;
            _pageAuditRepository.DataStoreName = DataStoreNamesConst.SeoDb;
            _systemTime = systemTime;
            _configuration = configuration;
        }
        public async Task<LighthouseProfileResponse> HandleAsync(CreateLighthouseProfileCommand request, CancellationToken cancellationToken)
        {
            // Create the profile
            LighthouseProfile profile = null;
            profile = new LighthouseProfile(_guidGenerator.Create(), _clock.Now, request.WebsiteUrl, request.CreatedByEmail);
            await _profileRepository.AddAsync(profile, cancellationToken);
            var responseDto = _mapper.Map<LighthouseProfileResponse>(profile);

            // Queue the inital scan
            var desktopPageAuditRequest = new PageAuditRequest(_guidGenerator.Create(), profile.Id, request.WebsiteUrl, _clock.Now,
                PageAuditRequestStatusConst.Created);
            await _pageAuditRequestRepository.AddAsync(desktopPageAuditRequest);

            var desktopPageAuditResponseDto = _mapper.Map<PageAuditRequestedResponse>(desktopPageAuditRequest);

            var desktopPageAudit = new PageAudit(_guidGenerator.Create());
            desktopPageAudit.StatusId = PageAuditStatusConst.Created;
            desktopPageAudit.CreatedDate = _systemTime.Now;
            desktopPageAudit.PageAuditRequestId = desktopPageAuditResponseDto.PageAuditRequestId;
            desktopPageAudit.PageUrl = request.WebsiteUrl;
            desktopPageAudit.Score = 0.0;
            desktopPageAudit.AuditReport = "";
            desktopPageAudit.Device = (int)BrowserOptions.Desktop;
            await _pageAuditRepository.AddAsync(desktopPageAudit);

            var mobilePageAuditRequest = new PageAuditRequest(_guidGenerator.Create(), profile.Id, request.WebsiteUrl, _clock.Now,
                PageAuditRequestStatusConst.Created);
            await _pageAuditRequestRepository.AddAsync(mobilePageAuditRequest);

            var mobilePageAuditResponseDto = _mapper.Map<PageAuditRequestedResponse>(mobilePageAuditRequest);

            var mobilePageAudit = new PageAudit(_guidGenerator.Create());
            mobilePageAudit.StatusId = PageAuditStatusConst.Created;
            mobilePageAudit.CreatedDate = _systemTime.Now;
            mobilePageAudit.PageAuditRequestId = mobilePageAuditResponseDto.PageAuditRequestId;
            mobilePageAudit.PageUrl = request.WebsiteUrl;
            mobilePageAudit.Score = 0.0;
            mobilePageAudit.AuditReport = "";
            mobilePageAudit.Device = (int)BrowserOptions.Mobile;
            await _pageAuditRepository.AddAsync(mobilePageAudit);

            // Queue the audit as a background job.
            var lighthouseApiKey = _configuration.GetSection("Lighthouse:ApiKey").Value;
            BackgroundJobHelper.Enqueue(() => _lighthouseAppService.RunLighthousePageAudits(desktopPageAuditRequest.Id, request.PackagePageAuditTypeList(),
                request.WebsiteUrl, BrowserOptions.Desktop, lighthouseApiKey, cancellationToken));
            BackgroundJobHelper.Enqueue(() => _lighthouseAppService.RunLighthousePageAudits(mobilePageAuditRequest.Id, request.PackagePageAuditTypeList(),
                request.WebsiteUrl, BrowserOptions.Mobile, lighthouseApiKey, cancellationToken));

            responseDto.StatusId = PageAuditStatusConst.Created;
            responseDto.PageAuditRequestId = desktopPageAuditResponseDto.PageAuditRequestId;
            return responseDto;
        }
    }
}
