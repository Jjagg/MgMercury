using VenusParticleEngine.Core;
using Xunit;

namespace VenusParticleEngine.Tests.Core;

public sealed class LineSegmentTests
{
    [Fact]
    public unsafe void CopyTo_WritesTwoContiguousVectors()
    {
        var values = new float[10];
        var segment = LineSegment.FromPoints(new Vector(1, 2), new Vector(3, 4));

        fixed (float* destination = values)
        {
            segment.CopyTo(destination);
        }

        Assert.Equal(new[] { 1f, 2f, 3f, 4f }, values.Take(4));
        Assert.All(values.Skip(4), value => Assert.Equal(0f, value));
    }
}
