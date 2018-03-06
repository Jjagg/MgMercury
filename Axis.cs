using System;
using System.Numerics;

namespace Mercury3D {
    /// <summary>
    /// An immutable data structure representing a directed fixed axis.
    /// </summary>
    public struct Axis : IEquatable<Axis>
    {
        public Vector3 Vector { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Axis"/> structure.
        /// </summary>
        /// <param name="x">The X component of the unit vector representing the axis.</param>
        /// <param name="y">The Y component of the unit vector representing the axis.</param>
        /// <param name="z">The Z component of the unit vector representing the axis.</param>
        public Axis(float x, float y, float z)
            : this(new Vector3(x, y, z))
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="Axis"/> structure.
        /// </summary>
        /// <param name="vector">Vector representing the axis. Will be normalized.</param>
        public Axis(Vector3 vector)
        {
            Vector = Vector3.Normalize(vector);
        }

        /// <summary>
        /// Gets a directed axis which points to the left.
        /// </summary>
        public static readonly Axis Left = new Axis(-Vector3.UnitX);

        /// <summary>
        /// Gets a directed axis which points up.
        /// </summary>
        public static readonly Axis Up = new Axis(-Vector3.UnitY);

        /// <summary>
        /// Gets a directed axis which points to the right.
        /// </summary>
        public static readonly Axis Right = new Axis(Vector3.UnitX);

        /// <summary>
        /// Gets a directed axis which points down.
        /// </summary>
        public static readonly Axis Down = new Axis(Vector3.UnitY);

        /// <summary>
        /// Gets a directed axis which points forward.
        /// </summary>
        public static readonly Axis Forward = new Axis(-Vector3.UnitZ);

        /// <summary>
        /// Gets a directed axis which points backward.
        /// </summary>
        public static readonly Axis Back = new Axis(Vector3.UnitZ);

        /// <summary>
        /// Multiplies the fixed axis by a magnitude value resulting in a directed vector.
        /// </summary>
        /// <param name="magnitude">The magnitude of the vector.</param>
        /// <returns>A directed vector.</returns>
        public Vector3 Multiply(float magnitude)
        {
            return Vector * magnitude;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Axis other)
        {
            return Vector.Equals(other.Vector);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to.</param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            
            return obj is Axis && Equals((Axis)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return Vector.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return Vector.ToString();
        }

        public static bool operator ==(Axis x, Axis y) {
            return x.Equals(y);
        }

        public static bool operator !=(Axis x, Axis y) {
            return !x.Equals(y);
        }

        public static Vector3 operator *(Axis axis, float magnitude) {
            return axis.Vector * magnitude;
        }
    }
}
