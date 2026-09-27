namespace VenusParticleEngine.Core.Modifiers
{
    public class ScaleInterpolator2 : IModifier
    {
        public string Name = "ScaleInterpolator2";

        public Vector InitialScale { get; set; }
        public Vector FinalScale { get; set; }

        public ScaleInterpolator2() { }

        public ScaleInterpolator2(Vector initialScale, Vector finalScale)
        {
            InitialScale = initialScale;
            FinalScale = finalScale;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var delta = FinalScale - InitialScale;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Scale = delta * particle->Age + InitialScale;
            }
        }
    }
}