using VenusParticleEngine.Core;
using Xunit;

namespace VenusParticleEngine.Tests.Core;

public sealed class RangeFTests
{
    [Fact]
    public void Contains_RejectsInfiniteValues()
    {
        var range = new RangeF(-1f, 1f);

        Assert.Throws<ArgumentException>(() => range.Contains(float.PositiveInfinity));
    }
}
