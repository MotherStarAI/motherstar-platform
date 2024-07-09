using RCommon.Mediator.Subscribers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCommon.EventHandling.Subscribers;
using RCommon.Mediator;
using System.Threading;
using RCommon;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands;

namespace MotherStar.Platform.Application.SEO.Lighthouse.DistributedEventHandling
{
    /// <summary>
    /// This is a distributed event handler that implements <see cref="ISubscriber{TEvent}{PageAuditCompletedEvent}"/>.
    /// </summary>
    /// <remarks>Anything that fires this handler typically comes from a configured MassTransit queue. </remarks>
    public class PageAuditCompletedEventHandler : ISubscriber<PageAuditCompletedEvent>
    {
        private readonly ILogger<PageAuditCompletedEventHandler> _logger;
        private readonly IMediatorService _mediator;

        public PageAuditCompletedEventHandler(ILogger<PageAuditCompletedEventHandler> logger, IMediatorService mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task HandleAsync(PageAuditCompletedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("----- Handling event: ({0})", @event);

            var command = new CreatePageAuditCommand(@event.PageAuditRequestId, @event.LighthouseResults);

            _logger.LogInformation("----- Sending command: {CommandName} - {IdProperty}: {PageAuditRequestId} ({@Command})",
                command.GetGenericTypeName(),
                nameof(command),
                command.PageAuditRequestId,
                command);

            await _mediator.Send(command);
        }
    }
}
