namespace MonoGameMPE.Mercury.ParticleEngine.Core.Modifiers {
    public sealed unsafe class OpacityFastFadeModifier : Modifier {
        protected internal override void Update(float elapsedSeconds, Particle* particle, int count) {
            while (count-- > 0) {
                particle->Opacity = 1.0f - particle->Age;
                particle++;
            }
        }
    }
}