using AutoMapper;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
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
using RCommon.Persistence.Crud;
using RCommon.ApplicationServices.Commands;
using RCommon.ApplicationServices.ExecutionResults;
using MotherStar.Platform.Domain;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands;

namespace MotherStar.Platform.Application.SEO.Lighthouse.CommandHandling
{
    public class CreatePageAuditCommandHandler : ICommandHandler<IExecutionResult, CreatePageAuditCommand>
    {
        private readonly ISystemTime _systemTime;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IGraphRepository<PageAudit> _pageAuditRepository;
        private readonly IMapper _mapper;

        public CreatePageAuditCommandHandler(ILogger<CreatePageAuditCommandHandler> logger, ISystemTime systemTime, IGuidGenerator guidGenerator,
            IGraphRepository<PageAudit> pageAuditRepository, IMapper mapper)
        {
            _systemTime = systemTime;
            _guidGenerator = guidGenerator;
            _pageAuditRepository = pageAuditRepository;
            _pageAuditRepository.DataStoreName = DataStoreNamesConst.LighthouseDb;
            _mapper = mapper;
        }

        public async Task<IExecutionResult> HandleAsync(CreatePageAuditCommand request, CancellationToken cancellationToken)
        {
            //Persist the Page Audits for each category
            var audit = request.LighthouseResults.LighthouseResult;
            var report = JsonConvert.SerializeObject(audit, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var seoAudit = audit.Categories.Seo;
            var performanceAudit = audit.Categories.Performance;
            var accessbilityAudit = audit.Categories.Accessibility;
            var bpAudit = audit.Categories.BestPractices;

            double seoAuditScore = 0;
            double performanceAuditScore = 0;
            double accessbilityAuditScore = 0;
            double bpAuditScore = 0;

            if (seoAudit != null)
            {
                if (seoAudit.Score != null && !seoAudit.Score.ToString().IsNullOrEmpty())
                {
                    seoAuditScore = double.Parse(seoAudit.Score.ToString());
                }
            }

            if (performanceAudit != null)
            {
                if (performanceAudit.Score != null && !performanceAudit.Score.ToString().IsNullOrEmpty())
                {
                    performanceAuditScore = double.Parse(performanceAudit.Score.ToString());
                }
            }

            if (accessbilityAudit != null)
            {
                if (accessbilityAudit.Score != null && !accessbilityAudit.Score.ToString().IsNullOrEmpty())
                {
                    accessbilityAuditScore = double.Parse(accessbilityAudit.Score.ToString());
                }
            }

            if (bpAudit != null)
            {
                if (bpAudit.Score != null && !bpAudit.Score.ToString().IsNullOrEmpty())
                {
                    bpAuditScore = double.Parse(bpAudit.Score.ToString());
                }
            }

            var averagePageAuditScore = (seoAuditScore + performanceAuditScore + accessbilityAuditScore + bpAuditScore) / 4;

            var pageAudit = _pageAuditRepository.FirstOrDefault(p => p.PageAuditRequestId == request.PageAuditRequestId);

            if (pageAudit != null)
            {
                pageAudit.StatusId = PageAuditStatusConst.Completed;
                pageAudit.CreatedDate = _systemTime.Now;
                pageAudit.PageAuditRequestId = request.PageAuditRequestId;
                pageAudit.Score = averagePageAuditScore;
                pageAudit.AuditReport = report;
                pageAudit.Device = audit.ConfigSettings.EmulatedFormFactor == "mobile" ? (int)BrowserOptions.Mobile : (int)BrowserOptions.Desktop;

                await _pageAuditRepository.UpdateAsync(pageAudit);
            }

            //var auditResponse = _mapper.Map<PageAuditResponse>(pageAudit);
            //return auditResponse;
            return new SuccessExecutionResult();
        }
    }
}
