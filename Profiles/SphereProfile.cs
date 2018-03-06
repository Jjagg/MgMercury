using System.Numerics;

namespace Mercury3D.Profiles
{
    /// <summary>
    /// Spawn particles on a sphere. Heading is either towards the center, away from the center or random.
    /// </summary>
    public class SphereProfile : Profile
    {
        public float Radius { get; set; }
        public CircleRadiation Radiate { get; set; }

        public override void GetOffsetAndHeading(out Vector3 offset, out Axis heading) {
            var dist = Radius;

            FastRand.NextUnitVector(out Vector3 dir);
            offset = dir * dist;

            if (Radiate == CircleRadiation.In)
                heading = new Axis(-dir);
            else if (Radiate == CircleRadiation.Out)
                heading = new Axis(dir);
            else
                FastRand.NextUnitVector(out heading);
        }
    }
}
