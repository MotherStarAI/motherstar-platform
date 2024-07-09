using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RCommon.Mediator.MediatR;
using RCommon.Persistence.Transactions;
using RCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using RCommon.MediatR;
using RCommon.EventHandling;
using RCommon.EventHandling.Producers;
using RCommon.ApplicationServices;
using RCommon.FluentValidation;
using MediatR;
using RCommon.ApplicationServices.ExecutionResults;
using MotherStar.Platform.Domain;
using MotherStar.Platform.Application;
using MotherStar.Platform.Application.SEO.Lighthouse.CommandHandling;
using MotherStar.Platform.Application.SEO.Lighthouse.DomainEventHandling;
using MotherStar.Platform.Application.SEO.Lighthouse.QueryHandling;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Queries;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands;
using MotherStar.Platform.Domain.SEO.Lighthouse.Events;
using MotherStar.Platform.Data;

namespace MotherStar.Platform.Bootstrapper.SEO.Extensions
{
    public static class WebApplicationBuilderExtensions
    {

        public static IRCommonBuilder AddRCommonServices(this WebApplicationBuilder builder)
        {
            return AddRCommonServices(builder.Configuration, builder.Services, builder.Environment);
        }
        public static IRCommonBuilder AddRCommonServices(IConfigurationRoot configuration, IServiceCollection services, IHostEnvironment environment)
        {
            return services.AddRCommon()
               .WithDateTimeSystem(x => x.Kind = DateTimeKind.Utc)
               .WithSequentialGuidGenerator(x => x.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString)
               .WithPersistence<EFCorePerisistenceBuilder, DefaultUnitOfWorkBuilder>(objectAccessActions: ef => // Repository/ORM configuration. We could easily swap out to NHibernate without impact to domain service up through the stack
               {
                   // Add all the DbContexts here
                   ef.AddDbContext<LighthouseDbContext>(DataStoreNamesConst.LighthouseDb, options =>
                   {
                       options.UseNpgsql(configuration.GetConnectionString(DataStoreNamesConst.LighthouseDb));
                   });
                   ef.SetDefaultDataStore(dataStore =>
                   {
                       dataStore.DefaultDataStoreName = DataStoreNamesConst.LighthouseDb;
                   });
               }, unitOfWorkActions: unitOfWork =>
               {
                   unitOfWork.SetOptions(options =>
                   {
                       options.AutoCompleteScope = true;
                       options.DefaultIsolation = IsolationLevel.ReadCommitted;
                   });
               })
               .WithEventHandling<InMemoryEventBusBuilder>(eventHandling => // In memory events typically produced/subscribed for domain events and domain event handlers
               {
                   // Producers
                   eventHandling.AddProducer<PublishWithEventBusEventProducer>();

                   // Subscribers
                   eventHandling.AddSubscriber<PageAuditCompletedDomainEvent, PageAuditCompletedDomainEventHandler>();
                   eventHandling.AddSubscriber<PageAuditRequestStatusChangedDomainEvent, PageAuditRequestStatusChangedDomainEventHandler>();
               })
               .WithMediator<MediatRBuilder>(mediator => // Mediator request handlers
               {
                   // MediatR Requests/Commands
                   mediator.AddRequest<BulkCreateLighthouseProfileCommand, BulkLighthouseProfileResponse, BulkCreateLighthouseProfileCommandHandler>();
                   mediator.AddRequest<CreateLighthouseProfileCommand, LighthouseProfileResponse, CreateLighthouseProfileCommandHandler>();
                   mediator.AddRequest<CreatePageAuditRequestCommand, PageAuditRequestedResponse, CreatePageAuditRequestCommandHandler>();

                   // MediatR Requests/Queries
                   mediator.AddRequest<GetLighthouseProfileQuery, LighthouseProfileResponse, GetLighthouseProfileQueryHandler>();
                   mediator.AddRequest<GetLighthouseProfileDetailsByQuery, LighthouseProfileDetailResponse, GetLighthouseProfileDetailsQueryHandler>();
                   mediator.AddRequest<GetPageAuditListQuery, PageAuditListModelResponse, GetPageAuditListQueryHandler>();
                   mediator.AddRequest<SearchPageAuditScoreHistoryQuery, PageAuditScoreHistoryListModelResponse, SearchPageAuditScoreHistoryQueryHandler>();

                   // MediatR Pipeline behaviors
                   mediator.AddLoggingToRequestPipeline();
                   mediator.AddValidationToRequestPipeline();
               }).
               WithCQRS<CqrsBuilder>(cqrs =>
               {
                   cqrs.AddCommand<CreatePageAuditCommand, CreatePageAuditCommandHandler, IExecutionResult>();

               })
               .WithValidation<FluentValidationBuilder>(validation =>
               {
                   validation.AddValidatorsFromAssemblyContaining(typeof(ApplicationLayerMappingProfile));
                   validation.UseWithCqrs(validation =>
                   {
                       validation.ValidateCommands = true;
                       validation.ValidateQueries = true;
                   });
               });
        }
    }
}
