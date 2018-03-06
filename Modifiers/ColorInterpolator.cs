using System;
using OpenWheels;

namespace Mercury3D.Modifiers
{
    /// <summary>
    /// Defines a modifier which interpolates the colour of a particle over the course of its lifetime.
    /// </summary>
    public sealed class ColorInterpolator : IModifier
    {
        /// <summary>
        /// Gets or sets the initial colour of particles when they are released.
        /// </summary>
        public HsvColor InitialColour { get; set; }

        /// <summary>
        /// Gets or sets the final colour of particles when they are retired.
        /// </summary>
        public HsvColor FinalColour { get; set; }

        public unsafe void Update(float elapsedseconds, ParticleBuffer.ParticleIterator iterator)
        {
            var d = FinalColour.H - InitialColour.H;
            var deltaH = d + (Math.Abs(d) > 180 ? (d < 0 ? 360 : -360) : 0);
            var deltaS = FinalColour.S - InitialColour.S;
            var deltaV = FinalColour.V - InitialColour.V;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Color = new HsvColor(
                    InitialColour.H + deltaH * particle->Age,
                    InitialColour.S + deltaS * particle->Age,
                    InitialColour.V + deltaV * particle->Age);
            }
        }
    }
}
