using System;
using Microsoft.Xna.Framework;

namespace MonoGameMPE.Core.Modifiers
{
    public class VelocityHueModifier : IModifier
    {
        public float StationaryHue { get; set; }
        public float VelocityHue { get; set; }
        public float VelocityThreshold { get; set; }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator) {
            var velocityThreshold2 = VelocityThreshold * VelocityThreshold;

            while (iterator.HasNext) {
                var particle = iterator.Next();
                var velocity2 = particle->Velocity.LengthSq;

                if (velocity2 >= velocityThreshold2) {
                    particle->Colour.H = VelocityHue;
                }
                else {
                    var t = (float)Math.Sqrt(velocity2) / VelocityThreshold;
                    particle->Colour.H = MathHelper.Lerp(StationaryHue, VelocityHue, t);
                }
            }
        }
    }
}