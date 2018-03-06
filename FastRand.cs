using System;
using System.Numerics;
using OpenWheels;

namespace Mercury3D {
    /// <summary>
    /// Defines a random number generator which uses the FastRand algorithm to generate random values.
    /// </summary>
    internal static class FastRand {
        static int _state = 1;

        public static void Seed(int seed) {
            if (seed < 1)
                throw new ArgumentOutOfRangeException("seed", "seed must be greater than zero");
            
            _state = seed;
        }
        
        /// <summary>
        /// Gets the next random integer value.
        /// </summary>
        /// <returns>A random positive integer.</returns>
        public static int NextInteger() {
            _state = 214013 * _state + 2531011;
            return (_state >> 16) & 0x7FFF;
        }

        /// <summary>
        /// Gets the next random integer value which is greater than zero and less than or equal to
        /// the specified maxmimum value.
        /// </summary>
        /// <param name="max">The maximum random integer value to return.</param>
        /// <returns>A random integer value between zero and the specified maximum value.</returns>
        public static int NextInteger(int max) {
            return (int)(max * NextSingle() + 0.5f);
        }

        /// <summary>
        /// Gets the next random integer between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The inclusive minimum value.</param>
        /// <param name="max">The inclusive maximum value.</param>
        public static int NextInteger(int min, int max) {
            return (int)((max - min) * NextSingle() + 0.5f) + min;
        }

        /// <summary>
        /// Gets the next random integer between the specified range of values.
        /// </summary>
        /// <param name="range">A range representing the inclusive minimum and maximum values.</param>
        /// <returns>A random integer between the specified minumum and maximum values.</returns>
        public static int NextInteger(Range range) {
            return NextInteger(range.Min, range.Max);
        }

        /// <summary>
        /// Gets the next random single value.
        /// </summary>
        /// <returns>A random single value between 0 and 1.</returns>
        public static float NextSingle() {
            return NextInteger() / (float)short.MaxValue;
        }

        /// <summary>
        /// Gets the next random single value which is greater than zero and less than or equal to
        /// the specified maxmimum value.
        /// </summary>
        /// <param name="max">The maximum random single value to return.</param>
        /// <returns>A random single value between zero and the specified maximum value.</returns>
        public static float NextSingle(float max) {
            return max * NextSingle();
        }

        /// <summary>
        /// Gets the next random single value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The inclusive minimum value.</param>
        /// <param name="max">The inclusive maximum value.</param>
        /// <returns>A random single value between the specified minimum and maximum values.</returns>
        public static float NextSingle(float min, float max) {
            return (max - min) * NextSingle() + min;
        }

        /// <summary>
        /// Gets the next random single value between the specified range of values.
        /// </summary>
        /// <param name="range">A range representing the inclusive minimum and maximum values.</param>
        /// <returns>A random single value between the specified minimum and maximum values.</returns>
        public static float NextSingle(RangeF range) {
            return NextSingle(range.Min, range.Max);
        }

        public static void NextUnitVector(out Axis axis)
        {
            NextUnitVector(out Vector3 v);
            axis = new Axis(v);
        }

        public static void NextUnitVector(out Vector3 vector) {
            var u1 = NextSingle();
            var u2 = NextSingle();
            var z = 1 - 2 * u1;
            var r = (float) Math.Sqrt(1 - z * z);
            var phi = 2 * Math.PI * u2;
            var x = r * (float) Math.Cos(phi);
            var y = r * (float) Math.Sin(phi);
            vector = new Vector3(x, y, z);
        }

        public static void NextColor(out HsvColor colour, ColourRange range)
        {
            var maxH = range.Max.H >= range.Min.H
                ? range.Max.H
                : range.Max.H + 360;
            colour = new HsvColor(NextSingle(range.Min.H, maxH),
                                 NextSingle(range.Min.S, range.Max.S),
                                 NextSingle(range.Min.V, range.Max.V));
        }
    }
}
