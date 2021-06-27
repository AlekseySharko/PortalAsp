using System.Diagnostics.CodeAnalysis;
using PortalModels;

namespace PortalAsp.Controllers.Helpers
{
    public static class NameComparer
    {
        public static bool CaseInsensitive<T>([AllowNull] T x, [AllowNull] T y) where T : INameAware
        {
            if (x is null || y is null) return false;
            return x.Name.ToLower() == y.Name.ToLower();
        }
    }
}
