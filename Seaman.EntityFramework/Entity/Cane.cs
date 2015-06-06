﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core.Model;

namespace Seaman.EntityFramework.Entity
{
    public class Cane: CaneBase
    {
        public virtual Canister Canister { get; set; }

        public virtual ICollection<Position> Positions
        {
            get { return _positions; }
            set { _positions = value; }
        }

        private ICollection<Position> _positions = new HashSet<Position>();

        //public virtual ICollection<Location> Locations
        //{
        //    get { return _locations; }
        //    set { _locations = value; }
        //}

        //private ICollection<Location> _locations = new HashSet<Location>();
    }
}
