using System.Numerics;

namespace Mercury3D.Profiles
{
    /// <summary>
    /// Particles spawn inside a box with a random direction.
    /// </summary>
    public class BoxFillProfile : Profile
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }

        public override void GetOffsetAndHeading(out Vector3 offset, out Axis heading) {
            offset = new Vector3(FastRand.NextSingle(Width * -0.5f, Width * 0.5f),
                                 FastRand.NextSingle(Height * -0.5f, Height * 0.5f),
                                 FastRand.NextSingle(Depth * -0.5f, Depth * 0.5f));

            FastRand.NextUnitVector(out heading);
        }
    }
}
