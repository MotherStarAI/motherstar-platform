using RCommon.Mediator.Subscribers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using RCommon.Mediator;
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
    public class PageAuditsController : ControllerBase
    {
        private readonly IMediatorService _mediator;

        public PageAuditsController(IMediatorService mediator)
        {
            _mediator = mediator;
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(PageAuditRequestedResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<PageAuditRequestedResponse>> CreatePageAuditAsync([FromBody] CreatePageAuditRequestCommand createPageAuditCommand)
        {
            return Ok(await _mediator.Send<CreatePageAuditRequestCommand, PageAuditRequestedResponse>(createPageAuditCommand));
        }

        [Route("search")]
        [HttpPost]
        [ProducesResponseType(typeof(PageAuditListModelResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PageAuditListModelResponse>> SearchPageAuditsAsync(SearchPageAuditsQuery request)
        {
            return Ok(await _mediator.Send<SearchPageAuditsQuery, PageAuditListModelResponse>(request));
        }

        [Route("score-history")]
        [HttpPost]
        [ProducesResponseType(typeof(PageAuditScoreHistoryListModelResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PageAuditScoreHistoryListModelResponse>> GetPageAuditScoreHistory([FromBody] SearchPageAuditScoreHistoryQuery request)
        {
            return Ok(await _mediator.Send<SearchPageAuditScoreHistoryQuery, PageAuditScoreHistoryListModelResponse>(request));
        }

        [Route("get")]
        [HttpPost]
        [ProducesResponseType(typeof(PageAuditListModelResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PageAuditListModelResponse>> GetPageAudits(GetPageAuditListQuery request)
        {
            return Ok(await _mediator.Send<GetPageAuditListQuery, PageAuditListModelResponse>(request));
        }
    }
}
