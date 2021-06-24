using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PortalModels;

namespace PortalAsp.Controllers.Helpers
{
    public class CaseInsensitiveNameComparer<T> : IEqualityComparer<T> where T: INameAware
    {
        public bool Equals([AllowNull] T x, [AllowNull] T y)
        {
            if (x is null || y is null) return false;
            return x.Name.ToLower() == y.Name.ToLower();
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.Name.ToLower().GetHashCode();
        }
    }
}
