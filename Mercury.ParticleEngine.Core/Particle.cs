using System.Runtime.InteropServices;

namespace MonoGameMPE.Mercury.ParticleEngine.Core {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Particle {
        public float Inception;
        public float Age;
        public fixed float Position[2];
        public fixed float TriggerPos [2];
        public fixed float Velocity[2];
        public fixed float Colour[3];
        public float Opacity;
        public float Scale;
        public float Rotation;
        public float Mass;

        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Particle));
    }
}