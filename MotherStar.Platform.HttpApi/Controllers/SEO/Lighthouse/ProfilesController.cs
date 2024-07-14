using RCommon.Mediator.Subscribers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using RCommon.Mediator;
using RCommon.Persistence.Transactions;
using RCommon.Entities;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Queries;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse.Commands;

namespace MotherStar.Platform.HttpApi.Controllers.SEO.Lighthouse
{
    [ApiVersion("1.0")]
    [EnableCors(HttpApiDefaults.CorsPolicyDefault)]
    [Authorize(AuthenticationSchemes = HttpApiDefaults.AuthenticationSchemesAllValid)]
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IMediatorService _mediator;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityEventTracker _entityEventTracker;

        public ProfilesController(IMediatorService mediator, IUnitOfWorkFactory unitOfWorkFactory, IEntityEventTracker entityEventTracker)
        {
            _mediator = mediator;
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityEventTracker = entityEventTracker;
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(LighthouseProfileResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<LighthouseProfileResponse>> CreateLighthouseProfileAsync([FromBody] CreateLighthouseProfileCommand createLighthouseProfileCommand)
        {
            return Ok(await _mediator.Send<CreateLighthouseProfileCommand, LighthouseProfileResponse>(createLighthouseProfileCommand));
        }

        [Route("bulkCreate")]
        [HttpPost]
        [ProducesResponseType(typeof(BulkLighthouseProfileResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<BulkLighthouseProfileResponse>> BulkCreateLighthouseProfileAsync([FromBody] BulkCreateLighthouseProfileCommand bulkLighthouseProfileCommand)
        {
            return Ok(await _mediator.Send<BulkCreateLighthouseProfileCommand, BulkLighthouseProfileResponse>(bulkLighthouseProfileCommand));
        }

        [HttpGet]
        [Route("get/{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(LighthouseProfileResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LighthouseProfileResponse>> GetProfileById(Guid id)
        {
            return Ok(await _mediator.Send<GetLighthouseProfileQuery, LighthouseProfileResponse>(new GetLighthouseProfileQuery(id)));
        }

        [HttpGet]
        [Route("detail/{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(LighthouseProfileDetailResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LighthouseProfileDetailResponse>> GetProfileDetailsById(Guid id)
        {
            return Ok(await _mediator.Send<GetLighthouseProfileDetailsByQuery, LighthouseProfileDetailResponse>(new GetLighthouseProfileDetailsByQuery(id)));
        }
    }
}
