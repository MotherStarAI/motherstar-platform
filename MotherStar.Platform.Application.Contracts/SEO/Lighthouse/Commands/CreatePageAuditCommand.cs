using Google.Apis.PagespeedInsights.v5.Data;
using RCommon.ApplicationServices.Commands;
using RCommon.ApplicationServices.ExecutionResults;
using RCommon.Mediator.Subscribers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands
{
    public class CreatePageAuditCommand : ICommand<IExecutionResult>
    {
        public CreatePageAuditCommand(Guid pageAuditRequestId, PagespeedApiPagespeedResponseV5 lighthouseResults)
        {
            PageAuditRequestId = pageAuditRequestId;
            LighthouseResults = lighthouseResults;
        }

        public Guid PageAuditRequestId { get; }
        public PagespeedApiPagespeedResponseV5 LighthouseResults { get; }
    }
}
