namespace MonoGameMPE.Mercury.ParticleEngine.Core.Profiles {
    public class CircleProfile : Profile {
        public float Radius;
        public CircleRadiation Radiate;

        public override unsafe void GetOffsetAndHeading(Coordinate* offset, Axis* heading) {
            var dist = FastRand.NextSingle(0f, Radius);

            FastRand.NextUnitVector((Vector*)heading);

            if (Radiate == CircleRadiation.In)
                *offset = new Coordinate(-heading->_x * dist, -heading->_y * dist);
            else
                *offset = new Coordinate(heading->_x * dist, heading->_y * dist);

            if (Radiate == CircleRadiation.None)
                FastRand.NextUnitVector((Vector*)heading);
        }
    }
}