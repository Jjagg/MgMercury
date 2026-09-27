using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VenusParticleEngine.Core.Modifiers;
using Xunit;

namespace VenusParticleEngine.Tests.Core.Modifiers;

public sealed class IModifierJsonConverterTests
{
    [Fact]
    public void WriteJson_SerializesModifiers()
    {
        var converter = new IModifierJsonConverter();
        using var text = new StringWriter();
        using var writer = new JsonTextWriter(text);

        converter.WriteJson(writer, new OpacityInterpolator2(0.2f, 0.8f), JsonSerializer.CreateDefault());
        writer.Flush();

        var modifier = JObject.Parse(text.ToString());

        Assert.Equal("OpacityInterpolator2", modifier!["Name"]!.Value<string>());
        Assert.Equal(0.2f, modifier["InitialOpacity"]!.Value<float>());
        Assert.Equal(0.8f, modifier["FinalOpacity"]!.Value<float>());
    }
}
