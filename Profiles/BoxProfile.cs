using System.Numerics;

namespace Mercury3D.Profiles
{
    /// <summary>
    /// Particles spawn on the faces of a box with a random heading.
    /// </summary>
    public class BoxProfile : Profile
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }

        public override void GetOffsetAndHeading(out Vector3 offset, out Axis heading)
        {
            var rand = FastRand.NextInteger((int) (2 * Width + 2 * Height + 2 * Depth));
            if (rand < Width) // Top
                offset = new Vector3(FastRand.NextSingle(Width * -0.5f, Width * 0.5f), Height * -0.5f, FastRand.NextSingle(Depth * -0.5f, Depth * 0.5f));
            else if (rand < 2 * Width) // Bottom
                offset = new Vector3(FastRand.NextSingle(Width * -0.5f, Width * 0.5f), Height * 0.5f, FastRand.NextSingle(Depth * -0.5f, Depth * 0.5f));
            else if (rand < 2 * Width + Height) // Left
                offset = new Vector3(Width * -0.5f, FastRand.NextSingle(Height * -0.5f, Height * 0.5f), FastRand.NextSingle(Depth * -0.5f, Depth * 0.5f));
            else if (rand < 2 * Width + 2 * Height)// Right
                offset = new Vector3(Width * 0.5f, FastRand.NextSingle(Height * -0.5f, Height * 0.5f), FastRand.NextSingle(Depth * -0.5f, Depth * 0.5f));
            else if (rand < 2 * Width + 2 * Height + Depth) // Back
                offset = new Vector3(FastRand.NextSingle(Width * -0.5f, Width * 0.5f), FastRand.NextSingle(Height * -0.5f, Height * 0.5f), Depth * -0.5f);
            else // Front
                offset = new Vector3(FastRand.NextSingle(Width * -0.5f, Width * 0.5f), FastRand.NextSingle(Height * -0.5f, Height * 0.5f), Depth * 0.5f);

            FastRand.NextUnitVector(out heading);
        }
    }
}
