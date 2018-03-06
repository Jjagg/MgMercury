using System;
using OpenWheels;

namespace Mercury3D.Modifiers
{
    public class VelocityHueModifier : IModifier
    {
        private float _velocityThresholdSq;

        public float StationaryHue { get; set; }
        public float VelocityHue { get; set; }
        public float VelocityThreshold
        {
            get => (float) Math.Sqrt(_velocityThresholdSq);
            set => _velocityThresholdSq = value * value;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var d = VelocityHue - StationaryHue;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                var velocitySq = particle->Velocity.X * particle->Velocity.X +
                                 particle->Velocity.Y * particle->Velocity.Y;

                float h;
                if (velocitySq >= _velocityThresholdSq)
                {
                    h = VelocityHue;
                }
                else
                {
                    var t = velocitySq / _velocityThresholdSq;
                    h = StationaryHue + t * d;
                }

                particle->Color = new HsvColor(h, particle->Color.S, particle->Color.V);
            }
        }

    }
}
