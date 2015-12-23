﻿using System.ComponentModel;

namespace MonoGameMPE.Core.Modifiers {
    /// <summary>
    /// Defines a modifier which interpolates the colour of a particle over the course of its lifetime.
    /// </summary>
    public sealed class ColourInterpolator2 : IModifier {
        /// <summary>
        /// Gets or sets the initial colour of particles when they are released.
        /// </summary>
        [Description("The color a particle gets emitted at.")]
        public Colour InitialColour { get; set; }

        /// <summary>
        /// Gets or sets the final colour of particles when they are retired.
        /// </summary>
        public Colour FinalColour { get; set; }

        public unsafe void Update(float elapsedseconds, ParticleBuffer.ParticleIterator iterator) {
            var delta = new Colour(FinalColour.H - InitialColour.H,
                                   FinalColour.S - InitialColour.S,
                                   FinalColour.L - InitialColour.L);

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Colour = new Colour(
                    InitialColour.H + delta.H*particle->Age,
                    InitialColour.S + delta.S*particle->Age,
                    InitialColour.L + delta.L*particle->Age);
            }
        }
    }
}