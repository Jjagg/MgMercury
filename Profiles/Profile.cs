using System;
using System.Numerics;

namespace Mercury3D.Profiles
{
    public abstract class Profile : ICloneable
    {
        public abstract void GetOffsetAndHeading(out Vector3 offset, out Axis heading);

        public object Clone()
        {
            return MemberwiseClone();
        }

        public enum CircleRadiation
        {
            None,
            In,
            Out
        }

        public static Profile Point()
        {
            return PointProfile.Instance;
        }

        public static Profile Line(Axis axis, float length)
        {
            return new LineProfile
            {
                Axis = axis,
                Length = length
            };
        }

        public static Profile Ring(float radius, CircleRadiation radiate)
        {
            return new SphereProfile
            {
                Radius = radius,
                Radiate = radiate
            };
        }

        public static Profile Box(float width, float height, float depth)
        {
            return new BoxProfile
            {
                Width = width,
                Height = height,
                Depth = depth
            };
        }

        public static Profile BoxFill(float width, float height, float depth)
        {
            return new BoxFillProfile
            {
                Width = width,
                Height = height,
                Depth = depth
            };
        }

        public static Profile BoxUniform(float width, float height, float depth)
        {
            return new BoxProfile
            {
                Width = width,
                Height = height,
                Depth = depth
            };
        }

        public static Profile Circle(float radius, CircleRadiation radiate)
        {
            return new SphereFillProfile
            {
                Radius = radius,
                Radiate = radiate
            };
        }

        public static Profile Spray(Axis direction, float spread)
        {
            return new SprayProfile
            {
                Direction = direction,
                Spread = spread
            };
        }

        public override string ToString()
        {
            return GetType().ToString();
        }
    }
}
