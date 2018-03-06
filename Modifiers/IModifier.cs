namespace Mercury3D.Modifiers {
    public interface IModifier {
        
        void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator);
    }
}