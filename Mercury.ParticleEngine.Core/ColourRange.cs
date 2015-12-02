namespace MonoGameMPE.Mercury.ParticleEngine.Core {
    public struct ColourRange {
        public ColourRange(Colour min, Colour max) {
            Min = min;
            Max = max;
        }

        public readonly Colour Min;
        public readonly Colour Max;

        public static implicit operator ColourRange(Colour value) {
            return new ColourRange(value, value);
        }
    }
}