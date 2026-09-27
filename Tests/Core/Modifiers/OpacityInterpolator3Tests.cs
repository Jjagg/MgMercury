using VenusParticleEngine.Core;
using VenusParticleEngine.Core.Modifiers;
using Xunit;

namespace VenusParticleEngine.Tests.Core.Modifiers;

public sealed class OpacityInterpolator3Tests
{
    [Fact]
    public unsafe void Update_UsesTheMiddleOfParticleLifetime()
    {
        using var buffer = new ParticleBuffer(1);
        var particle = buffer.Release(1).Next();
        particle->Age = 0.25f;
        particle->StartAge = 0.01f;
        particle->Term = 2f;

        var modifier = new OpacityInterpolator3(0f, 1f, 0f);
        modifier.Update(1f / 60f, buffer.Iterator);

        Assert.Equal(0.5f, particle->Opacity, 3);
    }
}
