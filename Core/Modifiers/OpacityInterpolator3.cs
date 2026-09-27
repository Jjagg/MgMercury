using System;

namespace VenusParticleEngine.Core.Modifiers
{
    public class OpacityInterpolator3 : IModifier
    {
        public string Name = "OpacityInterpolator3";

        public float InitialOpacity { get; set; }
        public float MediumOpacity { get; set; }
        public float FinalOpacity { get; set; }

        public OpacityInterpolator3() { }

        public OpacityInterpolator3(float initialOpacity, float mediumOpacity, float finalOpacity)
        {
            InitialOpacity = initialOpacity;
            MediumOpacity = mediumOpacity;
            FinalOpacity = finalOpacity;
        }

        public unsafe void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                var age = Math.Clamp(particle->Age, 0f, 1f);

                if (age <= 0.5f)
                {
                    particle->Opacity = InitialOpacity + (MediumOpacity - InitialOpacity) * (age * 2f);
                }
                else
                {
                    particle->Opacity = MediumOpacity + (FinalOpacity - MediumOpacity) * ((age - 0.5f) * 2f);
                }
            }
        }
    }
}
