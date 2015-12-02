namespace MonoGameMPE.Mercury.ParticleEngine.Core.Modifiers.Container
{
    public class LoopContainerModifier : Modifier
    {
        public int Width;
        public int Height;

        protected internal override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
        {
            while (count-- > 0)
            {
                var left = particle->TriggerPos[0] + Width * -0.5f;
                var right = particle->TriggerPos[0] + Width * 0.5f;
                var top = particle->TriggerPos[1] + Height * -0.5f;
                var bottom = particle->TriggerPos[1] + Height * 0.5f;

                if ((int)particle->Position[0] < left)
                {
                    particle->Position[0] = particle->Position[0] + Width;
                }
                else if ((int)particle->Position[0] > right)
                {
                    particle->Position[0] = particle->Position[0] - Width;
                }

                if ((int)particle->Position[1] < top)
                {
                    particle->Position[1] = particle->Position[1] + Height;
                }
                else if ((int)particle->Position[1] > bottom)
                {
                    particle->Position[1] = particle->Position[1] - Height;
                }

                particle++;
            }
        }
    }
}