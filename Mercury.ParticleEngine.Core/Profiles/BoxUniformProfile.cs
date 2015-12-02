namespace MonoGameMPE.Mercury.ParticleEngine.Core.Profiles
{
    public class BoxUniformProfile : Profile
    {
        public float Width;
        public float Height;
        public float Rotation;

        public override unsafe void GetOffsetAndHeading(Coordinate* offset, Axis* heading)
        {
            var rand = FastRand.NextInteger((int) (2*Width + 2*Height));
            if (rand < Width) // Top
                *offset = new Coordinate(FastRand.NextSingle(Width * -0.5f, Width * 0.5f), Height * -0.5f);
            else if (rand < 2*Width) // Bottom
                *offset = new Coordinate(FastRand.NextSingle(Width * -0.5f, Width * 0.5f), Height * 0.5f);
            else if (rand < 2*Width + Height) // Left
                *offset = new Coordinate(Width * -0.5f, FastRand.NextSingle(Height * -0.5f, Height * 0.5f));
            else // Right
                *offset = new Coordinate(Width * 0.5f, FastRand.NextSingle(Height * -0.5f, Height * 0.5f));

            FastRand.NextUnitVector((Vector*) heading);
        }
    }
}