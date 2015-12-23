﻿using System;

namespace MonoGameMPE.Core.Modifiers
{
    public class VelocityColourModifier : IModifier
    {
        public Colour StationaryColour { get; set; }
        public Colour VelocityColour { get; set; }
        public float VelocityThreshold { get; set; }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator) {
            var velocityThreshold2 = VelocityThreshold * VelocityThreshold;

            while (iterator.HasNext) {
                var particle = iterator.Next();
                var velocity2 = particle->Velocity.X * particle->Velocity.X +
                                particle->Velocity.Y * particle->Velocity.Y;
                var deltaColour = VelocityColour - StationaryColour;

                if (velocity2 >= velocityThreshold2) {
                    VelocityColour.CopyTo(out particle->Colour);
                }
                else {
                    var t = (float)Math.Sqrt(velocity2) / VelocityThreshold;

                    particle->Colour = new Colour(
                        deltaColour.H * t + StationaryColour.H,
                        deltaColour.S * t + StationaryColour.S,
                        deltaColour.L * t + StationaryColour.L);
                }
            }
        }
    }
}