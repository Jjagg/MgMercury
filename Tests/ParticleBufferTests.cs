using VenusParticleEngine.Core;
using Xunit;

namespace VenusParticleEngine.Tests;

public sealed class ParticleBufferTests
{
    [Fact]
    public void ReleaseAndReclaim_TrackCapacity()
    {
        using var buffer = new ParticleBuffer(3);

        buffer.Release(2);
        Assert.Equal(2, buffer.Count);
        Assert.Equal(1, buffer.Available);

        buffer.Reclaim(1);
        buffer.Release(5);

        Assert.Equal(3, buffer.Count);
        Assert.Equal(0, buffer.Available);
    }
}
