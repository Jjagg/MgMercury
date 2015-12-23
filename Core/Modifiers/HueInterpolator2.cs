﻿namespace MonoGameMPE.Core.Modifiers {
    public class HueInterpolator2 : IModifier {
        public float InitialHue { get; set; }
        public float FinalHue { get; set; }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator) {
            var delta = FinalHue - InitialHue;

            while (iterator.HasNext) {
                var particle = iterator.Next();
                particle->Colour.H = delta * particle->Age + InitialHue;
            }
        }
    }
}