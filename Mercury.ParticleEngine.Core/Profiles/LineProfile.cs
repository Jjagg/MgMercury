namespace MonoGameMPE.Mercury.ParticleEngine.Core.Profiles
{
    public class LineProfile : Profile
    {
        public Axis Axis;
        public float Length;


        public override unsafe void GetOffsetAndHeading(Coordinate* offset, Axis* heading)
        {
            var vect = Axis * FastRand.NextSingle(Length*-0.5f, Length*0.5f);
            *offset = new Coordinate(vect._x, vect._y);
            FastRand.NextUnitVector((Vector*)heading);
        }
    }
}