using System.Numerics;

namespace Mercury3D.Modifiers
{
    public class ScaleInterpolator2 : IModifier
    {
        public Vector3 InitialScale { get; set; }
        public Vector3 FinalScale { get; set; }

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
