namespace MonoGameMPE.Mercury.ParticleEngine.Core.Modifiers.Container {
    public sealed unsafe class ContainerModifier : Modifier {
        public int Width;
        public int Height;
        public float RestitutionCoefficient = 1;

        protected internal override void Update(float elapsedSeconds, Particle* particle, int count) {
            
            while (count-- > 0) {
                var left = particle->TriggerPos[0] + Width * -0.5f;
                var right = particle->TriggerPos[0] + Width * 0.5f;
                var top = particle->TriggerPos[1] + Height * -0.5f;
                var bottom = particle->TriggerPos[1] + Height * 0.5f;

                if ((int)particle->Position[0] < left) {
                    particle->Position[0] = left + (left - particle->Position[0]);
                    particle->Velocity[0] = -particle->Velocity[0] * RestitutionCoefficient;
                }
                else if ((int)particle->Position[0] > right) {
                    particle->Position[0] = right - (particle->Position[0] - right);
                    particle->Velocity[0] = -particle->Velocity[0] * RestitutionCoefficient;
                }

                if ((int)particle->Position[1] < top) {
                    particle->Position[1] = top + (top - particle->Position[1]);
                    particle->Velocity[1] = -particle->Velocity[1] * RestitutionCoefficient;
                }
                else if ((int)particle->Position[1] > bottom) {
                    particle->Position[1] = bottom - (particle->Position[1] - bottom);
                    particle->Velocity[1] = -particle->Velocity[1] * RestitutionCoefficient;
                }

                particle++;
            }
        }
    }
}