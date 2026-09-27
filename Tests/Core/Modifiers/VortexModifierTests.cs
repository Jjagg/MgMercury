using VenusParticleEngine.Core;
using VenusParticleEngine.Core.Modifiers;
using Xunit;

namespace VenusParticleEngine.Tests.Core.Modifiers;

public sealed class VortexModifierTests
{
    [Fact]
    public unsafe void Update_AtItsCenterKeepsParticleVelocityFinite()
    {
        using var buffer = new ParticleBuffer(1);
        var particle = buffer.Release(1).Next();
        particle->Position = Vector.Zero;
        particle->TriggerPos = Vector.Zero;
        particle->Velocity = Vector.Zero;

        var modifier = new VortexModifier(Vector.Zero, 1f, 10f);
        modifier.Update(1f / 60f, buffer.Iterator);

        Assert.True(float.IsFinite(particle->Velocity.X));
        Assert.True(float.IsFinite(particle->Velocity.Y));
    }
}
