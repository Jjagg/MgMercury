using System.ComponentModel;
using System.Linq;

namespace MonoGameMPE.Core
{

    [DefaultProperty("SaveOnClose")]
    public class ParticleEffect
    {
        [Description("The name of this particle effect.")]
        public string Name { get; set; }
        [Browsable(false)]
        public Emitter[] Emitters { get; set; }

        public ParticleEffect() {
            Emitters = new Emitter[0];
        }

        [Browsable(false)]
        public int ActiveParticles => Emitters.Sum(t => t.ActiveParticles);

        public void FastForward(Vector position, float seconds, float triggerPeriod)
        {
            var time = 0f;
            while (time < seconds)
            {
                Update(triggerPeriod);
                Trigger(position);
                time += triggerPeriod;
            }
        }

        public void Update(float elapsedSeconds)
        {
            foreach (var e in Emitters)
                e.Update(elapsedSeconds);
        }

        public void Trigger(Vector position)
        {
            foreach (var e in Emitters)
                e.Trigger(position);
        }

        public void Trigger(LineSegment line)
        {
            foreach (var e in Emitters)
                e.Trigger(line);
        }
    }
}