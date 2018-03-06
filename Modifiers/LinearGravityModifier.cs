namespace Mercury3D.Modifiers
{
    public class LinearGravityModifier : IModifier
    {
        public Axis Direction { get; set; }
        public float Strength { get; set; }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            var vector = Direction * (Strength * elapsedSeconds);

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Velocity += vector * particle->Mass;
            }
        }
    }
}
