using Newtonsoft.Json;
using VenusParticleEngine.Core;
using VenusParticleEngine.Core.Profiles;
using Xunit;

namespace VenusParticleEngine.Tests.Core;

public sealed class ParticleEffectTests
{
    [Fact]
    public void ReadFromJson_PropagatesInvalidJsonErrors()
    {
        Assert.Throws<JsonReaderException>(() =>
            ParticleEffect.ReadFromJson(string.Empty, "{ invalid json", null!, null!));
    }

    [Fact]
    public void ReadFromJsonFile_PropagatesFileErrors()
    {
        var missingFile = Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}.ptc");

        Assert.Throws<FileNotFoundException>(() =>
            ParticleEffect.ReadFromJsonFile(missingFile, null!, null!));
    }

    [Fact]
    public void ImplementsIDisposable()
    {
        Assert.IsAssignableFrom<IDisposable>(new ParticleEffect());
    }

    [Fact]
    public void UpdateEmittersTexture_PreservesJsonQuantityWhenTextureSetupFails()
    {
        using var emitter = new Emitter(4, TimeSpan.FromSeconds(1), Profile.Point())
        {
            Parameters = new ReleaseParameters { Quantity = 3 }
        };
        var effect = new ParticleEffect();
        effect.Emitters.Add("emitter", emitter);

        Assert.ThrowsAny<Exception>(() => effect.UpdateEmmitersTexture(string.Empty, null!, null!));

        Assert.Equal(3, emitter.Parameters.Quantity.Min);
        Assert.Equal(3, emitter.Parameters.Quantity.Max);
    }

    [Fact]
    public void JsonDeserialization_PreservesReleaseParameters()
    {
        var parameters = JsonConvert.DeserializeObject<ReleaseParameters>("""
            {
              "Quantity": { "Min": 1, "Max": 2 },
              "Speed": { "Min": 3.5, "Max": 4.5 },
              "Colour": {
                "Min": { "H": 10.0, "S": 0.2, "L": 0.3 },
                "Max": { "H": 20.0, "S": 0.4, "L": 0.5 }
              },
              "Opacity": { "Min": 0.6, "Max": 0.7 },
              "Scale": { "Min": 8.0, "Max": 9.0 },
              "Rotation": { "Min": 10.0, "Max": 11.0 },
              "Mass": { "Min": 12.0, "Max": 13.0 }
            }
            """);

        Assert.NotNull(parameters);
        Assert.Equal(1, parameters!.Quantity.Min);
        Assert.Equal(2, parameters.Quantity.Max);
        Assert.Equal(3.5f, parameters.Speed.Min);
        Assert.Equal(4.5f, parameters.Speed.Max);
        Assert.Equal(10f, parameters.Colour.Min.H);
        Assert.Equal(0.5f, parameters.Colour.Max.L);
        Assert.Equal(0.6f, parameters.Opacity.Min);
        Assert.Equal(9f, parameters.Scale.Max);
        Assert.Equal(10f, parameters.Rotation.Min);
        Assert.Equal(13f, parameters.Mass.Max);
    }

    [Fact]
    public void JsonDeserialization_UsesDefaultQuantityWhenQuantityIsMissing()
    {
        var parameters = JsonConvert.DeserializeObject<ReleaseParameters>("{}");

        Assert.NotNull(parameters);
        Assert.Equal(1, parameters!.Quantity.Min);
        Assert.Equal(1, parameters.Quantity.Max);
    }

    [Fact]
    public void Pause_PreventsUpdatesUntilPlayIsCalled()
    {
        using var effect = CreateEffectWithSingleParticleEmitter();
        effect.Trigger(Vector.Zero);
        effect.Pause();

        effect.Update(1f);

        Assert.Equal(1, effect.ActiveParticles);

        effect.Play();
        effect.Update(1f);

        Assert.Equal(0, effect.ActiveParticles);
    }

    [Fact]
    public void FastForward_TriggersTheEffectAtEachPeriod()
    {
        using var effect = CreateEffectWithSingleParticleEmitter(capacity: 4, termSeconds: 10f);

        effect.FastForward(Vector.Zero, seconds: 3f, triggerPeriod: 1f);

        Assert.Equal(3, effect.ActiveParticles);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-1f)]
    public void FastForward_RejectsNonPositiveTriggerPeriods(float triggerPeriod)
    {
        using var effect = CreateEffectWithSingleParticleEmitter();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            effect.FastForward(Vector.Zero, seconds: 1f, triggerPeriod));
    }

    private static ParticleEffect CreateEffectWithSingleParticleEmitter(int capacity = 1, float termSeconds = 1f)
    {
        var effect = new ParticleEffect();
        effect.Emitters.Add("emitter", new Emitter(capacity, TimeSpan.FromSeconds(termSeconds), Profile.Point())
        {
            Parameters = new ReleaseParameters { Quantity = 1 }
        });

        return effect;
    }
}
