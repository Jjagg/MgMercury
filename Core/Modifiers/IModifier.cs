using VenusParticleEngine.Core.Modifiers.Container;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace VenusParticleEngine.Core.Modifiers
{
    public interface IModifier
    {
        void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator);
    }

    public class IModifierJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        { return (objectType == typeof(IModifier)); }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var item = JObject.Load(reader);

            switch (item["Name"].Value<string>())
            {
                case "CircleContainerModifier":
                    return new CircleContainerModifier((float)item["Radius"], (bool)item["Inside"], (float)item["RestitutionCoefficient"]);
                case "RectContainerModifier":
                    return new RectContainerModifier((int)item["Width"], (int)item["Height"], (float)item["RestitutionCoefficient"]);
                case "RectLoopContainerModifier":
                    return new RectLoopContainerModifier((int)item["Width"], (int)item["Height"]);
                case "ColourInterpolator2":
                    return new ColourInterpolator2(
                        new Colour((float)item["InitialColour"]["H"], (float)item["InitialColour"]["S"], (float)item["InitialColour"]["L"]),
                        new Colour((float)item["FinalColour"]["H"], (float)item["FinalColour"]["S"], (float)item["FinalColour"]["L"]));
                case "DragModifier":
                    return new DragModifier((float)item["DragCoefficient"], (float)item["Density"]);
                case "HueInterpolator2":
                    return new HueInterpolator2((float)item["InitialHue"], (float)item["FinalHue"]);
                case "LinearGravityModifier":
                    return new LinearGravityModifier(
                        new Axis((float)item["Direction"]["X"], (float)item["Direction"]["Y"]),
                        (float)item["Strength"]);
                case "OpacityFastFadeModifier":
                    return new OpacityFastFadeModifier();
                case "OpacityInterpolator2":
                    return new OpacityInterpolator2((float)item["InitialOpacity"], (float)item["FinalOpacity"]);
                case "OpacityInterpolator3":
                    return new OpacityInterpolator3((float)item["InitialOpacity"], (float)item["MediumOpacity"], (float)item["FinalOpacity"]);
                case "RotationModifier":
                    return new RotationModifier((float)item["RotationRate"]);
                case "ScaleInterpolator2":
                    return new ScaleInterpolator2(
                        new Vector((float)item["InitialScale"]["X"], (float)item["InitialScale"]["Y"]),
                        new Vector((float)item["FinalScale"]["X"], (float)item["FinalScale"]["Y"]));
                case "VelocityColourInfiniteModifier":
                    return new VelocityColourInfiniteModifier(
                          new Colour((float)item["Colour1"]["H"], (float)item["Colour1"]["S"], (float)item["Colour1"]["L"]),
                          new Colour((float)item["Colour2"]["H"], (float)item["Colour2"]["S"], (float)item["Colour2"]["L"]),
                          (float)item["VelocityChangeColor"]);
                case "VelocityColourModifier":
                    return new VelocityColourModifier(
                          new Colour((float)item["StationaryColour"]["H"], (float)item["StationaryColour"]["S"], (float)item["StationaryColour"]["L"]),
                          new Colour((float)item["VelocityColour"]["H"], (float)item["VelocityColour"]["S"], (float)item["VelocityColour"]["L"]),
                          (float)item["VelocityThreshold"]);
                case "VelocityHueModifier":
                    return new VelocityHueModifier(
                              (float)item["StationaryHue"],
                              (float)item["VelocityHue"],
                              (float)item["VelocityThreshold"]);
                case "VortexModifier":
                    return new VortexModifier(
                        new Vector((float)item["Position"]["X"], (float)item["Position"]["Y"]),
                        (float)item["Mass"],
                        (float)item["MaxSpeed"]);
                default:
                    throw new NotImplementedException();
            }

            //  return serializer.Deserialize(reader, typeof(ColourInterpolator2));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            JObject.FromObject(value, JsonSerializer.CreateDefault()).WriteTo(writer);
        }
    }
}
