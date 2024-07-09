using MotherStar.Platform.Domain.SEO.Lighthouse.Models;
using RCommon.Collections;
using RCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.SEO.Lighthouse
{
    public record LighthouseProfileListModelResponse : PaginatedListModel<LighthouseProfile, LighthouseProfileResponse>
    {
        public LighthouseProfileListModelResponse(IPaginatedList<LighthouseProfile> source, PaginatedListRequest paginatedListRequest, int totalCount,
            bool skipSort = false)
            : base(source, paginatedListRequest, totalCount, skipSort)
        {
        }

        protected override IQueryable<LighthouseProfileResponse> CastItems(IQueryable<LighthouseProfile> source)
        {
            return source.Select(x => new LighthouseProfileResponse
            {

            });
        }
    }
}
