using System;
using OpenWheels;

namespace Mercury3D.Modifiers
{
    public class VelocityColourModifier : IModifier
    {
        private float _velocityThresholdSq;

        public HsvColor StationaryColour { get; set; }
        public HsvColor VelocityColour { get; set; }
        public float VelocityThreshold
        {
            get => (float) Math.Sqrt(_velocityThresholdSq);
            set => _velocityThresholdSq = value * value;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                var velocitySq = particle->Velocity.X * particle->Velocity.X +
                                 particle->Velocity.Y * particle->Velocity.Y;

                if (velocitySq >= _velocityThresholdSq)
                {
                    particle->Color = VelocityColour;
                }
                else
                {
                    var t = velocitySq / _velocityThresholdSq;
                    particle->Color = HsvColor.Lerp(StationaryColour, VelocityColour, t);
                }
            }
        }
    }
}
