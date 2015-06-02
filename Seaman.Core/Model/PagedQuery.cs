using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Seaman.Core
{
    public class PagedQuery
    {
        public PagedQuery()
        {
            OrderBy = new Dictionary<string, string>();
            Filter = new NameValueCollection();
        }

        public Int32 Skip { get; set; }
        public Int32 Take { get; set; }
        public IDictionary<String, String> OrderBy { get; set; }
        public NameValueCollection Filter { get; set; }

        public Int32 SkipTakeCount<T>(ref IQueryable<T> queryable)
        {
            var count = queryable.Count();
            if (Skip > 0)
                queryable = queryable.Skip(Skip);
            if (Take > 0)
                queryable = queryable.Take(Take);
            return count;
        }


        public PagedQuery(NameValueCollection collection) : this()
        {
            Skip = GetInt(collection, "skip");
            Take = GetInt(collection, "take");
            Filter = new NameValueCollection();

            collection.GetValues("filter").IfNotNull(c =>
            {
                foreach (var s in c)
                {
                    if (String.IsNullOrWhiteSpace(s))
                        continue;
                    var parts = s.Split(new[] {' '}, 2);
                    Filter.Add(parts[0], parts.Length > 1 ? parts[1] : String.Empty);
                }
                return Filter;
            });
            OrderBy = new Dictionary<string, string>();

            collection.GetValues("order").IfNotNull(c =>
            {
                var splittedByComma = c.SelectMany(s => s.Split(','));
                foreach (var s in splittedByComma)
                {
                    if (String.IsNullOrWhiteSpace(s))
                        continue;

                    var parts = s.Split(new[] { ' ' }, 2);
                    OrderBy[parts[0]] = parts.Length > 1 ? parts[1] : String.Empty;
                }
                return Filter;
            });
                
        }

        private Int32 GetInt(NameValueCollection collection, String key)
        {
            return collection.GetValues(key).IfNotNull(a => a.FirstOrDefault()).IfNotNull(s =>
            {
                int i;
                Int32.TryParse(s, out i);
                return i;
            });
        }
    }
}