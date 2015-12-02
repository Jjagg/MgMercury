﻿using System;
using System.Runtime.InteropServices;

namespace MonoGameMPE.Mercury.ParticleEngine.Core {
    /// <summary>
    /// Defines a data structure representing a Euclidean vector facing a particular direction,
    /// including a magnitude value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector {
        internal readonly float _x;
        internal readonly float _y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> structure.
        /// </summary>
        /// <param name="axis">The axis along which the vector points.</param>
        /// <param name="magnitude">The magnitude of the vector.</param>
        public Vector(Axis axis, float magnitude) {
            _x = axis._x * magnitude;
            _y = axis._y * magnitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> structure.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        public Vector(float x, float y) {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Gets the length or magnitude of the Euclidean vector.
        /// </summary>
        public float Magnitude {
            get { return (float)Math.Sqrt(_x * _x + _y * _y); }
        }

        /// <summary>
        /// Gets the axis in which the vector is facing.
        /// </summary>
        /// <returns>A <see cref="Axis"/> value representing the direction the vector is facing.</returns>
        public Axis Axis {
            get { return new Axis(_x, _y); }
        }

        public Vector Multiply(float factor) {
            return new Vector(_x * factor, _y * factor);
        }

        /// <summary>
        /// Copies the X and Y components of the vector to the specified memory location.
        /// </summary>
        /// <param name="destination">The memory location to copy the coordinate to.</param>
        public unsafe void CopyTo(float* destination) {
            destination[0] = _x;
            destination[1] = _y;
        }

        /// <summary>
        /// Destructures the vector, exposing the individual X and Y components.
        /// </summary>
        public void Destructure(out float x, out float y) {
            x = _x;
            y = _y;
        }

        /// <summary>
        /// Exposes the individual X and Y components of the vector to the specified matching function.
        /// </summary>
        /// <param name="callback">The function which matches the individual X and Y components.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown if the value passed to the <paramref name="callback"/> parameter is <c>null</c>.
        /// </exception>
        public void Match(Action<float, float> callback) {
            if (callback == null)
                throw new ArgumentNullException("callback");

            callback(_x, _y);
        }

        /// <summary>
        /// Exposes the individual X and Y components of the vector to the specified mapping function and returns the
        /// result;
        /// </summary>
        /// <typeparam name="T">The type being mapped to.</typeparam>
        /// <param name="map">
        /// A function which maps the X and Y values to an instance of <typeparamref name="T"/>.
        /// </param>
        /// <returns>
        /// The result of the <paramref name="map"/> function when passed the individual X and Y components.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown if the value passed to the <paramref name="map"/> parameter is <c>null</c>.
        /// </exception>
        public T Map<T>(Func<float, float, T> map) {
            if (map == null)
                throw new ArgumentNullException("map");

            return map(_x, _y);
        }

        public static Vector operator*(Vector value, float factor) {
            return value.Multiply(factor);
        }
    }
}