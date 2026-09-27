namespace VenusParticleEngine.Core.Modifiers
{
    public unsafe class VortexModifier : IModifier
    {
        public string Name = "VortexModifier";

        public Vector Position { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        // Note: not the real-life one
        private const float GravConst = 100000f;

        public VortexModifier() { }

        public VortexModifier(Vector position, float mass, float maxSpeed)
        {
            Position = position;
            Mass = mass;
            MaxSpeed = maxSpeed;
        }

        public void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var diff = Position + particle->TriggerPos - particle->Position;

                var distance2 = diff.LengthSq;
                if (distance2 <= float.Epsilon)
                    continue;

                var speedGain = GravConst * Mass / distance2 * elapsedSeconds;
                // normalize distances and multiply by speedGain
                particle->Velocity += diff.Axis * speedGain;
            }
        }
    }
}
