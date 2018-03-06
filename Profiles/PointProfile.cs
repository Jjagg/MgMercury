using System.Numerics;

namespace Mercury3D.Profiles
{
    /// <summary>
    /// Particles all spawn in the same place with a random heading.
    /// </summary>
    public class PointProfile : Profile
    {
        public static PointProfile Instance = new PointProfile();

        public override void GetOffsetAndHeading(out Vector3 offset, out Axis heading)
        {
            offset = Vector3.Zero;
            FastRand.NextUnitVector(out heading);
        }
    }
}
