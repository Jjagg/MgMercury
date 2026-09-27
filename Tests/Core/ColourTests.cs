using VenusParticleEngine.Core;
using Xunit;

namespace VenusParticleEngine.Tests.Core;

public sealed class ColourTests
{
    [Fact]
    public void Lerp_InterpolatesLightness()
    {
        var result = Colour.Lerp(new Colour(0f, 0f, 0f), new Colour(0f, 0f, 1f), 0.5f);

        Assert.Equal(0.5f, result.L, 3);
    }
}
