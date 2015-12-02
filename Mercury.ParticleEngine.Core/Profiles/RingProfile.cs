namespace MonoGameMPE.Mercury.ParticleEngine.Core.Profiles {
    public class RingProfile : Profile
    {
        public float Radius;
        public CircleRadiation Radiate;

        public override unsafe void GetOffsetAndHeading(Coordinate* offset, Axis* heading) {
            FastRand.NextUnitVector((Vector*)heading);

            if (Radiate == CircleRadiation.In)
                *offset = new Coordinate(-heading->_x * Radius, -heading->_y * Radius);
            else
                *offset = new Coordinate(heading->_x * Radius, heading->_y * Radius);

            if (Radiate == CircleRadiation.None)
                FastRand.NextUnitVector((Vector*)heading);
        }
    }
}