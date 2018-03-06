using System;
using OpenWheels;

namespace Mercury3D.Modifiers
{
    public class HueInterpolator : IModifier
    {
        public float InitialHue { get; set; }
        public float FinalHue { get; set; }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var d = FinalHue - InitialHue;
            var delta = d + (Math.Abs(d) > 180 ? (d < 0 ? 360 : -360) : 0);

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Color = new HsvColor(
                    delta * particle->Age + InitialHue,
                    particle->Color.S,
                    particle->Color.V);
            }
        }
    }

    public class ColorMapInterpolator : IModifier
    {
        public ColorMap ColorMap { get; set; }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Color = ColorMap.Map(particle->Age);
            }
        }
    }
}
