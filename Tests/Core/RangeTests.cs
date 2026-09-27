using VenusParticleEngine.Core;
using Xunit;
using ParticleRange = VenusParticleEngine.Core.Range;

namespace VenusParticleEngine.Tests.Core;

public sealed class RangeTests
{
    [Fact]
    public void Constructor_NormalizesBoundariesAndContainsEndpoints()
    {
        var range = new ParticleRange(8, 3);

        Assert.Equal(3, range.Min);
        Assert.Equal(8, range.Max);
        Assert.True(range.Contains(3));
        Assert.True(range.Contains(8));
        Assert.False(range.Contains(9));
    }

    [Fact]
    public void Union_EncompassesBothRanges()
    {
        var result = ParticleRange.Union(new ParticleRange(4, 10), new ParticleRange(-3, 6));

        Assert.Equal(-3, result.Min);
        Assert.Equal(10, result.Max);
    }
}
