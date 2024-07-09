using Google.Apis.PagespeedInsights.v5;
using Google.Apis.PagespeedInsights.v5.Data;
using Google.Apis.Services;
using RCommon.Mediator.Subscribers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RCommon;
using RCommon.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RCommon.Mediator;
using RCommon.EventHandling.Producers;
using RCommon.ApplicationServices.Commands;
using MotherStar.Platform.Application.Contracts;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands;
using MotherStar.Platform.Application.SEO.Lighthouse.DistributedEventHandling;

namespace MotherStar.Platform.Application.SEO.Lighthouse.Services
{
    public class LighthouseAppService : ILighthouseAppService
    {
        private readonly ILogger<LighthouseAppService> _logger;
        private readonly ICommonFactory<ILighthouseReportGenerator> _reportFactory;
        private readonly ICommandBus _commandBus;
        private readonly IEventRouter _eventRouter;

        public LighthouseAppService(ILogger<LighthouseAppService> logger,
            ICommonFactory<ILighthouseReportGenerator> reportFactory, ICommandBus commandBus, IEventRouter eventRouter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reportFactory = reportFactory ?? throw new ArgumentNullException(nameof(reportFactory));
            _commandBus = commandBus;
            _eventRouter = eventRouter ?? throw new ArgumentNullException(nameof(eventRouter));
        }

        public async Task RunLighthousePageAudits(Guid pageAuditRequestId, IList<LighthouseAuditTypeEnum> auditTypes, string url,
            BrowserOptions browserOptions, string lighthouseApiKey, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Beginning Lighthouse page audits for Url: {url}", url);

                var reportGenerator = _reportFactory.Create(); // Using a factory in case we need more control over generator options
                var report = await reportGenerator.RunLighthousePageAudits(pageAuditRequestId, auditTypes, url, browserOptions, lighthouseApiKey, cancellationToken);

                var command = new CreatePageAuditCommand(pageAuditRequestId, report);
                await _commandBus.DispatchCommandAsync(command);

                _eventRouter.AddTransactionalEvent(new PageAuditRequestStatusChangedEvent(pageAuditRequestId, PageAuditRequestStatusConst.Completed));

                _logger.LogInformation("Completed Lighthouse page audits for Url: {url}", url);
                _logger.LogDebug("Completed Lighthouse page audits for Url: {url} Results: {results}", url, JsonConvert.SerializeObject(report));
            }
            catch (ApplicationException ex)
            {
                _eventRouter.AddTransactionalEvent(new PageAuditRequestStatusChangedEvent(pageAuditRequestId, PageAuditRequestStatusConst.Failed));
                _logger.LogError("The queued Lighthouse page audits did not run through to completion. See details in Exception: {ex}", ex);
            }

        }
    }
}
