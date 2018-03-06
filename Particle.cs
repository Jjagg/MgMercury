using System.Numerics;
using System.Runtime.InteropServices;
using OpenWheels;

namespace Mercury3D {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Particle {
        public float Inception;
        public float Age;
        public Vector3 Position;
        public Vector3 TriggerPos;
        public Vector3 Velocity;
        public HsvColor Color;
        public float Opacity;
        public Vector3 Scale;
        public float Rotation;
        public float Mass;

        public static readonly int SizeInBytes = Marshal.SizeOf<Particle>();
    }
}
