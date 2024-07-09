using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Domain.SEO.Lighthouse.Models
{
    public static class PageAuditStatusConst
    {
        public static readonly Guid NotInitiated = Guid.Empty;
        public static readonly Guid Completed = new Guid("67f8bdc0-04ca-448f-a5b1-46aea7c569c3");
        public static readonly Guid Created = new Guid("c7625069-2055-4669-a0f2-5169b20ebc6e");
    }
}
