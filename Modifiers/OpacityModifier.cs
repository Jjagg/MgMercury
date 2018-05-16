namespace Mercury3D.Modifiers
{
    public class OpacityModifier : IModifier
    {
        public float Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                _dirty = true;
            }
        }

        private bool _dirty;
        private float _opacity;

        public OpacityModifier(float opacity = 1.0f)
        {
            Opacity = opacity;
            _dirty = true;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            if (!_dirty)
                return;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Opacity = _opacity;
            }

            _dirty = false;
        }
    }
}