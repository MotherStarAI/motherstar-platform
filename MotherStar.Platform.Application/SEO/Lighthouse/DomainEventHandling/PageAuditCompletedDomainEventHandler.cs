using RCommon.Mediator.Subscribers;
using Microsoft.Extensions.Logging;
using RCommon;
using RCommon.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RCommon.EventHandling.Subscribers;
using RCommon.Persistence.Crud;
using MediatR;
using RCommon.EventHandling.Producers;
using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using MotherStar.Platform.Domain.SEO.Lighthouse.Events;
using MotherStar.Platform.Application.SEO.Lighthouse.DistributedEventHandling;

namespace MotherStar.Platform.Application.SEO.Lighthouse.DomainEventHandling
{
    /// <summary>
    /// This is a domain event handler that handles events raised from the domain. This event handler implements 
    /// <see cref="ISubscriber{PageAuditRequestCompletedDomainEvent}"/>.
    /// </summary>
    /// /// <remarks>Domain Handlers interact with domain objects and repositories which will raise domain events as part of their 
    /// propety changes and peristence. Logging, and validation, and transactions may be implemented as part of the <see cref="Mediator"/>
    /// pipeline but are not usually handled directly in this layer. This layer will also add events that implement <see cref="IDistributedEvent"/> 
    /// to the <see cref="IDistributedEventBroker"/>. Business logic should be encapsulated here.</remarks>
    public class PageAuditCompletedDomainEventHandler : ISubscriber<PageAuditCompletedDomainEvent>
    {
        private readonly IGraphRepository<PageAudit> _pageAuditRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ISystemTime _systemTime;
        private readonly IEventRouter _eventRouter;

        public PageAuditCompletedDomainEventHandler(ILogger<PageAuditCompletedDomainEventHandler> logger,
            IGraphRepository<PageAudit> pageAuditRepository, IGuidGenerator guidGenerator,
            ISystemTime systemTime, IEventRouter eventRouter)
        {
            _pageAuditRepository = pageAuditRepository;
            _guidGenerator = guidGenerator;
            _systemTime = systemTime;
            _eventRouter = eventRouter;
        }

        public async Task HandleAsync(PageAuditCompletedDomainEvent @event, CancellationToken cancellationToken = default)
        {
            // Communicate the audit results to anyone that may be listening
            _eventRouter.AddTransactionalEvent(new PageAuditRequestStatusChangedEvent(@event.PageAuditRequestId, PageAuditRequestStatusConst.Completed));
            await Task.CompletedTask;
        }
    }
}
