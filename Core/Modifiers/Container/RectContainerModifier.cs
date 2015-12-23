namespace MonoGameMPE.Core.Modifiers.Container
{
    public sealed class RectContainerModifier : IModifier
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float RestitutionCoefficient { get; set; } = 1;

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator) {
            
            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                var left =   particle->TriggerPos.X + Width * -0.5f;
                var right =  particle->TriggerPos.X + Width * 0.5f;
                var top =    particle->TriggerPos.Y + Height * -0.5f;
                var bottom = particle->TriggerPos.Y + Height * 0.5f;

                if ((int)particle->Position.X < left) {
                    particle->Position.X = left + (left - particle->Position.X);
                    particle->Velocity.X = -particle->Velocity.X * RestitutionCoefficient;
                }
                else if (particle->Position.X > right) {
                    particle->Position.X = right - (particle->Position.X - right);
                    particle->Velocity.X = -particle->Velocity.X * RestitutionCoefficient;
                }

                if (particle->Position.Y < top) {
                    particle->Position.Y = top + (top - particle->Position.Y);
                    particle->Velocity.Y = -particle->Velocity.Y * RestitutionCoefficient;
                }
                else if ((int)particle->Position.Y > bottom) {
                    particle->Position.Y = bottom - (particle->Position.Y - bottom);
                    particle->Velocity.Y = -particle->Velocity.Y * RestitutionCoefficient;
                }
            }
        }
    }
}