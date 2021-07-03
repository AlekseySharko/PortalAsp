using System.Linq;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;

namespace PortalAsp.Tests
{
    class TestHelper
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            return sourceList.AsQueryable().BuildMockDbSet().Object;
        }
    }
}
