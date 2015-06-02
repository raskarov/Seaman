using System;
using System.Collections.Generic;

namespace Seaman.Core
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
            Data = new List<T>();
        }

        public List<T> Data { get; set; }
        public PagedQuery Query { get; set; }
        public Int32 Total { get; set; }

    }
}