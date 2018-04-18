using System;
using System.Numerics;

namespace Mercury3D.Modifiers
{
    public unsafe class VortexModifier : IModifier
    {
        public Vector3 Offset { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }

        // Note: not the real-life one
        private const float GravConst = 100000f;

        public void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var pos = particle->TriggerPos + Offset;

                var diff = particle->Position - pos;
                var distSq = Vector3.Dot(diff, diff);
                var speedGain = Math.Min(GravConst * Mass * elapsedSeconds / distSq, MaxSpeed);

                // normalize distances and multiply by speedGain
                particle->Velocity += Vector3.Normalize(diff) * speedGain;
            }
        }
    }
}
