using RCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Domain.SEO.Lighthouse.Exceptions
{
    public class LighthouseDomainException : GeneralException
    {
        public LighthouseDomainException()
        {

        }

        public LighthouseDomainException(string message) : base(message)
        {

        }
    }
}
