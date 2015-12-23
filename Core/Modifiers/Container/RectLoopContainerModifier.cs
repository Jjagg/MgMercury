namespace MonoGameMPE.Core.Modifiers.Container
{
    public class RectLoopContainerModifier : IModifier
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var left =   particle->TriggerPos.X + Width * -0.5f;
                var right =  particle->TriggerPos.X + Width * 0.5f;
                var top =    particle->TriggerPos.Y + Height * -0.5f;
                var bottom = particle->TriggerPos.Y + Height * 0.5f;

                if ((int)particle->Position.X < left)
                {
                    particle->Position.X = particle->Position.X + Width;
                }
                else if ((int)particle->Position.X > right)
                {
                    particle->Position.X = particle->Position.X - Width;
                }

                if ((int)particle->Position.Y < top)
                {
                    particle->Position.Y = particle->Position.Y + Height;
                }
                else if ((int)particle->Position.Y > bottom)
                {
                    particle->Position.Y = particle->Position.Y - Height;
                }
            }
        }
    }
}