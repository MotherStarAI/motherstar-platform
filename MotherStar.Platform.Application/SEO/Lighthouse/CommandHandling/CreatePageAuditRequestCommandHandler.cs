using AutoMapper;
using Hangfire;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using RCommon.Mediator.Subscribers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RCommon;
using RCommon.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RCommon.Persistence.Crud;
using MotherStar.Platform.Domain;
using MotherStar.Platform.Application.Extensions;
using MotherStar.Platform.Application.SEO.Lighthouse.Jobs;
using MotherStar.Platform.Application.SEO.Lighthouse.Services;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands;
using MotherStar.Platform.Domain.SEO.Lighthouse.Exceptions;

namespace MotherStar.Platform.Application.SEO.Lighthouse.CommandHandling
{
    /// <summary>
    /// A command handler that implements <see cref="IRequestHandler{CreatePageAuditRequestCommand, PageAuditRequestedResponse}"/>. 
    /// The DI registration is handled in the <see cref="ServiceCollectionExtensions.AddLightHouseServices"/> extension method.
    /// </summary>
    /// <remarks>Command Handlers interact with domain objects and repositories which will raise domain events as part of their 
    /// properly changes and peristence. Logging, validation, and transactions may be implemented as part of the <see cref="Mediator"/>
    /// pipeline but may also be handled here as well. Business rules and business logic should not be encapsulated in this layer.</remarks>
    public class CreatePageAuditRequestCommandHandler : IAppRequestHandler<CreatePageAuditRequestCommand, PageAuditRequestedResponse>
    {
        private readonly IWriteOnlyRepository<PageAuditRequest> _pageAuditRequestRepository;
        private readonly IGraphRepository<PageAudit> _pageAuditRepository;
        private readonly ISystemTime _clock;
        private readonly IMapper _mapper;
        private readonly ILighthouseAppService _lighthouseAppService;
        private readonly IGraphRepository<LighthouseProfile> _lighthouseProfileRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IConfiguration _configuration;
        private readonly ISystemTime _systemTime;

        public CreatePageAuditRequestCommandHandler(ILogger<CreatePageAuditRequestCommandHandler> logger,
            IWriteOnlyRepository<PageAuditRequest> pageAuditRequestRepository,
            IGraphRepository<PageAudit> pageAuditRepository, ISystemTime systemTime,
            ISystemTime clock, IGuidGenerator guidGenerator, IMapper mapper, ILighthouseAppService lighthouseAppService,
            IGraphRepository<LighthouseProfile> lighthouseProfileRepository, IConfiguration configuration)
        {
            _pageAuditRequestRepository = pageAuditRequestRepository;
            _pageAuditRequestRepository.DataStoreName = DataStoreNamesConst.LighthouseDb;
            _pageAuditRepository = pageAuditRepository;
            _pageAuditRepository.DataStoreName = DataStoreNamesConst.LighthouseDb;
            _clock = clock;
            _guidGenerator = guidGenerator;
            _mapper = mapper;
            _lighthouseAppService = lighthouseAppService;
            _lighthouseProfileRepository = lighthouseProfileRepository;
            _lighthouseProfileRepository.DataStoreName = DataStoreNamesConst.LighthouseDb;
            _configuration = configuration;
            _systemTime = systemTime;
        }


        public async Task<PageAuditRequestedResponse> HandleAsync(CreatePageAuditRequestCommand request, CancellationToken cancellationToken)
        {
            // Validate that profile exists
            var profileCount = await _lighthouseProfileRepository.GetCountAsync(x => x.Id == request.LighthouseProfileId);
            Guard.Against<LighthouseDomainException>(profileCount == 0, string.Format("Lighthouse Profile does not exist with Id:{0}", request.LighthouseProfileId));

            // Validate Url
            var hompageUrl = _lighthouseProfileRepository.FindQuery(x => x.Id == request.LighthouseProfileId).Select(x => x.WebsiteUrl).First();
            Guard.Against<LighthouseDomainException>(!request.PageUrl.Contains(hompageUrl),
                        string.Format("Page Url: {0} must include hompage: {1}", request.PageUrl, hompageUrl));

            var pageAuditRequest = new PageAuditRequest(_guidGenerator.Create(), request.LighthouseProfileId, request.PageUrl, _clock.Now,
                PageAuditRequestStatusConst.Created);
            await _pageAuditRequestRepository.AddAsync(pageAuditRequest);

            var responseDto = _mapper.Map<PageAuditRequestedResponse>(pageAuditRequest);

            var pageAudit = new PageAudit(_guidGenerator.Create());
            pageAudit.StatusId = PageAuditStatusConst.Created;
            pageAudit.CreatedDate = _systemTime.Now;
            pageAudit.PageAuditRequestId = responseDto.PageAuditRequestId;
            pageAudit.PageUrl = request.PageUrl;
            pageAudit.Score = 0.0;
            pageAudit.AuditReport = "";
            pageAudit.Device = (int)request.Device;
            await _pageAuditRepository.AddAsync(pageAudit);

            // Queue the audit as a background job.
            var lighthouseApiKey = _configuration.GetSection("Lighthouse:ApiKey").Value;
            BackgroundJobHelper.Enqueue(() => _lighthouseAppService.RunLighthousePageAudits(pageAuditRequest.Id, request.PackagePageAuditTypeList(),
                request.PageUrl, request.Device, lighthouseApiKey, cancellationToken));
            return responseDto;
        }
    }
}
