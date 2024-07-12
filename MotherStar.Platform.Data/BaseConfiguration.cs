﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Data
{
    public static class BaseConfiguration
    {
        public static void Configure<T>(EntityTypeBuilder<T> entity)
            where T : BusinessEntity
        {
            entity.Ignore(x => x.AllowEventTracking);
            //entity.Ignore(x => x.IsChanged);
            entity.Ignore(x => x.LocalEvents);

        }
    }
}
