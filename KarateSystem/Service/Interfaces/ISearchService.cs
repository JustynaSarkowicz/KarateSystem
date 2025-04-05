using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Service.Interfaces
{
    public interface ISearchService
    {
        public List<T> SearchInCollection<T>(IEnumerable<T> collection, string searchText, params string[] propertyNames);
    }
}
