namespace MonoGameMPE.Core.Modifiers {
    public interface IModifier {
        
        void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator);
    }
}