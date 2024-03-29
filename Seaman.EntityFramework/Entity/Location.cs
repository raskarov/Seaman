﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core.Model;

namespace Seaman.EntityFramework.Entity
{
    public class Location : LocationBase
    {
        public virtual Sample Sample { get; set; }
        public virtual CollectionMethod CollectionMethod { get; set; }

        public virtual ExtractReason ExtractReason { get; set; }
    }
}
