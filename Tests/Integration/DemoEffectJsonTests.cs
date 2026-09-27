using Newtonsoft.Json;
using VenusParticleEngine.Core;
using VenusParticleEngine.Core.Modifiers;
using VenusParticleEngine.Core.Modifiers.Container;
using Xunit;

namespace VenusParticleEngine.Tests.Integration;

public sealed class DemoEffectJsonTests
{
    [Fact]
    public void CloudEffect_DeserializesItsEmittersParametersAndModifiers()
    {
        using var effect = DeserializeEffect("testParticle.ptc");

        Assert.Equal(2, effect.Emitters.Count);

        var cloudEmitter = effect.Emitters["principal"];
        Assert.Equal("cloud.png", cloudEmitter.TexturePath);
        Assert.Equal(100, cloudEmitter.Capacity);
        Assert.Equal(1, cloudEmitter.Parameters.Quantity.Min);
        Assert.Equal(1.1f, cloudEmitter.Parameters.Speed.Min);
        Assert.IsType<LinearGravityModifier>(cloudEmitter.Modifiers["LinearGravity"]);

        var pointEmitter = effect.Emitters["points"];
        Assert.Empty(pointEmitter.TexturePath);
        Assert.IsType<ScaleInterpolator2>(pointEmitter.Modifiers["ScaleInterpolator2"]);
        Assert.IsType<VelocityColourInfiniteModifier>(pointEmitter.Modifiers["VelocityColourInfinite"]);
        Assert.IsType<RotationModifier>(pointEmitter.Modifiers["Rotation"]);
    }

    [Fact]
    public void StarEffect_DeserializesItsEmissionParametersAndModifiers()
    {
        using var effect = DeserializeEffect("testParticle2.ptc");

        var emitter = Assert.Single(effect.Emitters).Value;
        Assert.Equal("star.xnb", emitter.TexturePath);
        Assert.Equal(10, emitter.Capacity);
        Assert.Equal(1, emitter.Parameters.Quantity.Min);
        Assert.Equal(225f, emitter.Parameters.Scale.Min);
        Assert.Equal(1f, emitter.Parameters.Colour.Min.L);
        Assert.IsType<OpacityFastFadeModifier>(emitter.Modifiers["OpacityFastFade"]);
        Assert.IsType<RotationModifier>(emitter.Modifiers["Rotation"]);
        Assert.IsType<ScaleInterpolator2>(emitter.Modifiers["ScaleInterpolator2"]);
        Assert.IsType<DragModifier>(emitter.Modifiers["Drag"]);
        Assert.IsType<RectLoopContainerModifier>(emitter.Modifiers["RectLoopContainerModifier"]);
    }

    private static ParticleEffect DeserializeEffect(string fileName)
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new IModifierJsonConverter());

        var jsonPath = Path.Combine(AppContext.BaseDirectory, "Assets", fileName);
        var effect = JsonConvert.DeserializeObject<ParticleEffect>(File.ReadAllText(jsonPath), settings);

        return Assert.IsType<ParticleEffect>(effect);
    }
}
