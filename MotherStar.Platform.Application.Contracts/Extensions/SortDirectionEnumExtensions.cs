using RCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.Contracts.Extensions
{
    public static class SortDirectionEnumExtensions
    {
        /// <summary>
        /// Simple utility that returns true if <see cref="SortDirectionEnum"/> is ascending and false if descending
        /// </summary>
        /// <param name="source">Object we are extending.</param>
        /// <returns>True if ascending and false if not</returns>
        public static bool ToBoolean(this SortDirectionEnum source)
        {
            if (source == SortDirectionEnum.Descending)
            {
                return false;
            }
            return true;
        }
    }
}
