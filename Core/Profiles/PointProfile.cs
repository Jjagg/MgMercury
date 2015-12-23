﻿namespace MonoGameMPE.Core.Profiles {
    public class PointProfile : Profile {
        public override void GetOffsetAndHeading(out Vector offset, out Axis heading) {
            offset = Vector.Zero;

            FastRand.NextUnitVector(out heading);
        }
    }
}