using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Mercury3D {
    /// <summary>
    /// Defines a part of a line that is bounded by two distinct end points.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LineSegment : IEquatable<LineSegment> {
        internal Vector3 _point1;
        internal Vector3 _point2;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSegment"/> structure.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        public LineSegment(Vector3 point1, Vector3 point2) {
            _point1 = point1;
            _point2 = point2;
        }

        public static LineSegment FromPoints(Vector3 p1, Vector3 p2)
            => new LineSegment(p1, p2);

        public static LineSegment FromOrigin(Vector3 origin, Vector3 vector)
            => new LineSegment(origin, origin + vector);

        public Vector3 Origin => _point1;

        public Axis Direction {
            get {
                var coord = _point2 - _point1;
                return new Axis(coord);
            }
        }

        public void Translate(Vector3 t)
        {
            _point1 += t;
            _point2 += t;
        }

        public Vector3 ToVector() {
            return _point2 - _point1;
        }

        public void Destructure(out Vector3 point1, out Vector3 point2) {
            point1 = _point1;
            point2 = _point2;
        }

        public void Match(Action<Vector3, Vector3> callback) {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
        }

        public T Map<T>(Func<Vector3, Vector3, T> map) {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            return map(_point1, _point2);
        }

        public bool Equals(LineSegment other) {
            return _point1.Equals(other._point1) &&
                   _point2.Equals(other._point2);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is LineSegment & Equals((LineSegment)obj);
        }

        public override int GetHashCode() {
            var hashCode = _point1.GetHashCode();

            hashCode = (hashCode * 397) ^ _point2.GetHashCode();

            return hashCode;
        }

        public override string ToString() {
            return string.Format("({0:x}:{0:y},{1:x}:{1:y})", _point1, _point2);
        }
    }
}
