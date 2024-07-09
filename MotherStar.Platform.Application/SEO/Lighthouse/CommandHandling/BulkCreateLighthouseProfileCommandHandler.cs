using AutoMapper;
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
using MediatR;
using RCommon.Persistence.Crud;
using MotherStar.Platform.Domain;
using MotherStar.Platform.Application.Extensions;
using MotherStar.Platform.Application.SEO.Lighthouse.Jobs;
using MotherStar.Platform.Application.SEO.Lighthouse.Services;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;

namespace MotherStar.Platform.Application.SEO.Lighthouse.CommandHandling
{
    /// <summary>
    /// A command handler that implements <see cref="IRequestHandler{CreateLighthouseCustomerCommand, LighthouseCustomerResponse}"/>. 
    /// The DI registration is handled in the <see cref="ServiceCollectionExtensions.AddLightHouseServices"/> extension method.
    /// </summary>
    /// <remarks>Command Handlers interact with domain objects and repositories which will raise domain events as part of their 
    /// propetty changes and peristence. Logging, validation, and transactions may be implemented as part of the <see cref="Mediator"/>
    /// pipeline but may also be handled here as well. Business rules and business logic should not be encapsulated in this layer.</remarks>
    public class BulkCreateLighthouseProfileCommandHandler : IAppRequestHandler<BulkCreateLighthouseProfileCommand, BulkLighthouseProfileResponse>
    {
        private readonly IWriteOnlyRepository<LighthouseProfile> _customerRepository;
        private readonly ISystemTime _clock;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IMapper _mapper;
        private readonly ILighthouseAppService _lighthouseAppService;
        private readonly IWriteOnlyRepository<PageAuditRequest> _pageAuditRequestRepository;
        private readonly IConfiguration _configuration;

        public BulkCreateLighthouseProfileCommandHandler(ILogger<CreatePageAuditRequestCommandHandler> logger, IWriteOnlyRepository<LighthouseProfile> customerRepository,
            ISystemTime clock, IGuidGenerator guidGenerator, IMapper mapper, ILighthouseAppService lighthouseAppService,
            IWriteOnlyRepository<PageAuditRequest> pageAuditRequestRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _customerRepository.DataStoreName = DataStoreNamesConst.LighthouseDb;
            _clock = clock;
            _guidGenerator = guidGenerator;
            _mapper = mapper;
            _lighthouseAppService = lighthouseAppService;
            _pageAuditRequestRepository = pageAuditRequestRepository;
            _pageAuditRequestRepository.DataStoreName = DataStoreNamesConst.LighthouseDb;
            _configuration = configuration;
        }
        public async Task<BulkLighthouseProfileResponse> HandleAsync(BulkCreateLighthouseProfileCommand request, CancellationToken cancellationToken)
        {
            // Enumerate each item and add delay so we don't exceeed 4 requests per second (current limit for Pagespeed Insights: 400 per 100 seconds)
            BulkLighthouseProfileResponse bulkResponseDto = new BulkLighthouseProfileResponse();

            foreach (var url in request.WebsiteUrls)
            {
                // Remove any whitespace
                var cleanUrl = string.Concat(url.Where(c => !char.IsWhiteSpace(c)));

                // Add delay to handle limits
                double delay = 0.0;

                // Create the profile
                LighthouseProfile profile = null;
                profile = new LighthouseProfile(_guidGenerator.Create(), _clock.Now, cleanUrl, request.CreatedByEmail);
                await _customerRepository.AddAsync(profile, cancellationToken);
                var responseDto = _mapper.Map<LighthouseProfileResponse>(profile);

                // Queue the inital scan
                var pageAuditRequest = new PageAuditRequest(_guidGenerator.Create(), profile.Id, cleanUrl, _clock.Now,
                    PageAuditRequestStatusConst.Created);
                await _pageAuditRequestRepository.AddAsync(pageAuditRequest);
                responseDto = _mapper.Map<LighthouseProfileResponse>(pageAuditRequest);

                // Queue the audit as a background job. Keep in mind that we're creating 2 jobs
                var lighthouseApiKey = _configuration.GetSection("Lighthouse:ApiKey").Value;
                BackgroundJobHelper.Schedule(() => _lighthouseAppService.RunLighthousePageAudits(pageAuditRequest.Id, request.PackagePageAuditTypeList(),
                    cleanUrl, BrowserOptions.Desktop, lighthouseApiKey, cancellationToken), TimeSpan.FromSeconds(delay));
                BackgroundJobHelper.Schedule(() => _lighthouseAppService.RunLighthousePageAudits(pageAuditRequest.Id, request.PackagePageAuditTypeList(),
                    cleanUrl, BrowserOptions.Mobile, lighthouseApiKey, cancellationToken), TimeSpan.FromSeconds(delay));

                // Increase delay
                delay += 0.6; // Limit is 4/second so this should give us a little room

                bulkResponseDto.LighthouseProfileResponses.Add(responseDto);
            }



            return bulkResponseDto;
        }
    }
}
