using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Domain.SEO.Lighthouse.Models
{
    public static class PageAuditRequestStatusConst
    {
        public static readonly Guid Created = new Guid("c7625069-2055-4669-a0f2-5169b20ebc6e");
        public static readonly Guid Started = new Guid("c16ca33c-2949-4f88-82f2-36eb24bbf874");
        public static readonly Guid Failed = new Guid("48a62047-2f22-453c-ab16-590dafa383b1");
        public static readonly Guid Completed = new Guid("cccae8e9-350c-43c5-bf0c-5d0d7fe1409c");


    }
}
