using System.Numerics;

namespace Mercury3D.Profiles
{
    /// <summary>
    /// Particles spawn on a line with a random heading.
    /// </summary>
    public class LineProfile : Profile
    {
        public Axis Axis { get; set; }
        public float Length { get; set; }

        public override void GetOffsetAndHeading(out Vector3 offset, out Axis heading)
        {
            offset = Axis * FastRand.NextSingle(Length*-0.5f, Length*0.5f);
            FastRand.NextUnitVector(out heading);
        }
    }
}
