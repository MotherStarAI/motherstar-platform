using RCommon.Mediator.Subscribers;
using Microsoft.Extensions.Logging;
using RCommon.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RCommon.Persistence.Crud;
using RCommon.EventHandling.Subscribers;
using MotherStar.Platform.Domain.SEO.Lighthouse.Events;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;

namespace MotherStar.Platform.Application.SEO.Lighthouse.DomainEventHandling
{
    public class PageAuditRequestStatusChangedDomainEventHandler : ISubscriber<PageAuditRequestStatusChangedDomainEvent>
    {
        private readonly IGraphRepository<PageAuditRequest> _pageAuditRequestRepository;

        public PageAuditRequestStatusChangedDomainEventHandler(ILogger<PageAuditRequestStatusChangedDomainEventHandler> logger,
            IGraphRepository<PageAuditRequest> _pageAuditRequestRepository)
        {
            this._pageAuditRequestRepository = _pageAuditRequestRepository;
        }


        public async Task HandleAsync(PageAuditRequestStatusChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            // Note: if we implement event sourcing, we would simply write/insert the object to a queue. No selects or updates here.
            var pageAuditRequest = await _pageAuditRequestRepository.FindAsync(notification.PageAuditRequestId, cancellationToken);
            await _pageAuditRequestRepository.UpdateAsync(pageAuditRequest, cancellationToken);


        }
    }
}
