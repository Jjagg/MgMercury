﻿namespace MonoGameMPE.Mercury.ParticleEngine.Core.Profiles {
    public abstract class Profile {
        public abstract unsafe void GetOffsetAndHeading(Coordinate* offset, Axis* heading);

        public enum CircleRadiation
        {
            None,
            In,
            Out
        }

        public static Profile Point() {
            return new PointProfile();
        }

        public static Profile Line(Axis axis, float length)
        {
            return new LineProfile
            {
                Axis = axis,
                Length = length
            };
        }

        public static Profile Ring(float radius, CircleRadiation radiate) {
            return new RingProfile {
                Radius = radius,
                Radiate = radiate
            };
        }

        public static Profile Box(float width, float height) {
            return new BoxProfile {
                Width = width,
                Height = height
            };
        }

        public static Profile BoxFill(float width, float height) {
            return new BoxFillProfile {
                Width = width,
                Height = height
            };
        }

        public static Profile BoxUniform(float width, float height)
        {
            return new BoxUniformProfile {
                Width = width,
                Height = height
            };
        }

        public static Profile Circle(float radius, CircleRadiation radiate) {
            return new CircleProfile {
                Radius = radius,
                Radiate = radiate
            };
        }

        public static Profile Spray(Axis direction, float spread) {
            return new SprayProfile {
                Direction = direction,
                Spread = spread
            };
        }
    }
}
