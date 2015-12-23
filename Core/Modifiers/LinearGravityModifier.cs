﻿namespace MonoGameMPE.Core.Modifiers
{
    public class LinearGravityModifier : IModifier
    {
        public Axis Direction { get; set; }
        public float Strength { get; set; }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator) {
            var vector = Direction * (Strength * elapsedSeconds);

            while (iterator.HasNext) {
                var particle = iterator.Next();
                particle->Velocity = new Vector(
                    particle->Velocity.X + vector.X * particle->Mass,
                    particle->Velocity.Y + vector.Y * particle->Mass);
            }
        }
    }
}