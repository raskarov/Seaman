﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core.Model;

namespace Seaman.EntityFramework.Entity
{
    public class CollectionMethod : CollectionMethodBase
    {
        public virtual ICollection<Location> Samples
        {
            get { return _samples; }
            set { _samples = value; }
        }

        private ICollection<Location> _samples = new HashSet<Location>();
    }
}
