using VenusParticleEngine;
using Xunit;

namespace VenusParticleEngine.Tests.Core;

public sealed class ColorHelperTests
{
    [Fact]
    public void ToHsl_UsesConventionalRedGreenBlueArgumentOrder()
    {
        var colour = ColorHelper.ToHsl(255f, 0f, 128f);

        Assert.InRange(colour.H, 329f, 331f);
    }
}
