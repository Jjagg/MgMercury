using VenusParticleEngine.Core;
using VenusParticleEngine.Core.Profiles;
using Xunit;

namespace VenusParticleEngine.Tests.Core;

public sealed class EmitterTests
{
    [Fact]
    public void Trigger_ReleasesParticlesOnItsFirstCall()
    {
        using var emitter = new Emitter(4, TimeSpan.FromSeconds(1), Profile.Point())
        {
            Parameters = new ReleaseParameters { Quantity = 1 }
        };

        emitter.Trigger(Vector.Zero);

        Assert.Equal(1, emitter.ActiveParticles);
    }

    [Fact]
    public void Trigger_LineReleasesTheConfiguredQuantity()
    {
        using var emitter = new Emitter(4, TimeSpan.FromSeconds(1), Profile.Point())
        {
            Parameters = new ReleaseParameters { Quantity = 3 }
        };

        emitter.Trigger(LineSegment.FromPoints(Vector.Zero, new Vector(10f, 0f)));

        Assert.Equal(3, emitter.ActiveParticles);
    }

    [Fact]
    public void Update_ReclaimsParticlesAfterTheirTermWhenLooping()
    {
        using var emitter = new Emitter(1, TimeSpan.FromSeconds(1), Profile.Point())
        {
            Parameters = new ReleaseParameters { Quantity = 1 }
        };
        emitter.Trigger(Vector.Zero);

        emitter.Update(1f);

        Assert.True(emitter.HasExpired);
        Assert.Equal(0, emitter.ActiveParticles);
    }

    [Fact]
    public void Update_KeepsExpiredParticlesWhenLoopingIsDisabled()
    {
        using var emitter = new Emitter(1, TimeSpan.FromSeconds(1), Profile.Point())
        {
            Loop = false,
            Parameters = new ReleaseParameters { Quantity = 1 }
        };
        emitter.Trigger(Vector.Zero);

        emitter.Update(1f);

        Assert.True(emitter.HasExpired);
        Assert.Equal(1, emitter.ActiveParticles);
    }
}
