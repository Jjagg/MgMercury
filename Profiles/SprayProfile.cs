using System;
using System.Numerics;

namespace Mercury3D.Profiles
{
    /// <summary>
    /// Spawn particles on a point with heading in a cone.
    /// </summary>
    public class SprayProfile : Profile
    {
        private float _halfSpread;
        private Quaternion _rotation;
        private Axis _direction;

        public Axis Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                var axis = Vector3.Cross(value.Vector, Vector3.UnitZ);
                var angle = Vector3.Dot(value.Vector, Vector3.UnitZ);
                _rotation = Quaternion.CreateFromAxisAngle(axis, angle);
            }
        }

        public float Spread
        {
            get => _halfSpread * 2;
            set => _halfSpread = value * .5f;
        }

        public override void GetOffsetAndHeading(out Vector3 offset, out Axis heading)
        {
            offset = Vector3.Zero;

            var cos = (float) Math.Cos(_halfSpread);
            var z = FastRand.NextSingle(cos, 1);
            var d = (float) Math.Sqrt(1 - z * z);
            var x = d * cos;
            var y = d * (float) Math.Sin(_halfSpread);

            heading = new Axis(Vector3.Transform(new Vector3(x, y, z), _rotation));
        }
    }
}
