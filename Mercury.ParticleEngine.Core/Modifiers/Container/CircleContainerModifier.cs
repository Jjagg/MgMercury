using System;

namespace MonoGameMPE.Mercury.ParticleEngine.Core.Modifiers.Container
{
    public class CircleContainerModifier : Modifier
    {
        public float Radius;
        public bool Inside = true;
        public float RestitutionCoefficient = 1;

        protected internal override unsafe void Update(float elapsedSeconds, Particle* particle, int count)
        {
            var radiusSq = Radius*Radius;
            while (count-- > 0)
            {
                var x = particle->Position[0] - particle->TriggerPos[0];
                var y = particle->Position[1] - particle->TriggerPos[1];
                var distSq = x*x + y*y;

                if (Inside)
                {
                    if (distSq > radiusSq)
                    {
                        var dist = (float)Math.Sqrt(distSq);
                        var d = dist - Radius; // how far outside the circle is the particle

                        var normalX = x/dist;
                        var normalY = y/dist;
                        
                        var twoRestDot = 2*RestitutionCoefficient*
                                         (particle->Velocity[0]*normalX +
                                          particle->Velocity[1]*normalY);

                        particle->Velocity[0] -= twoRestDot*normalX;
                        particle->Velocity[1] -= twoRestDot*normalY;

                        var perpD = d; // exact computation requires sqrt or goniometrics
                        particle->Position[0] -= normalX*perpD;
                        particle->Position[1] -= normalY*perpD;
                    }
                }
                else
                {
                    if (distSq < radiusSq)
                    {
                        var dist = (float)Math.Sqrt(distSq);
                        var d = (float)Math.Ceiling(Radius - dist); // how far outside the circle is the particle

                        var normalX = -x / dist;
                        var normalY = -y / dist;

                        var twoRestDot = 2 * RestitutionCoefficient *
                                         (particle->Velocity[0] * normalX +
                                          particle->Velocity[1] * normalY);

                        particle->Velocity[0] -= twoRestDot * normalX;
                        particle->Velocity[1] -= twoRestDot * normalY;

                        // exact computation of d requires sqrt or goniometrics
                        particle->Position[0] -= normalX * d;
                        particle->Position[1] -= normalY * d;
                    }
                }

                particle++;
            }
        }

    }
}