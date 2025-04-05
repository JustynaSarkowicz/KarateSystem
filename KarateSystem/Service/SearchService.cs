using KarateSystem.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Service
{
    public class SearchService : ISearchService
    {
        public List<T> SearchInCollection<T>(IEnumerable<T> collection,
                                          string searchText,
                                          params string[] propertyNames)
        {
            if (string.IsNullOrWhiteSpace(searchText) || collection == null || !collection.Any())
                return collection.ToList();

            var filtered = collection.Where(item =>
            {
                var type = typeof(T);
                foreach (var propertyName in propertyNames)
                {
                    var property = type.GetProperty(propertyName);
                    if (property == null) continue;

                    var value = property.GetValue(item)?.ToString();
                    if (!string.IsNullOrEmpty(value) &&
                        value.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            });

            return filtered.ToList();
        }
    }

}
