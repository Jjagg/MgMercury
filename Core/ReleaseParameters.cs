using System.ComponentModel;

namespace MonoGameMPE.Core
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ReleaseParameters
    {
        public ReleaseParameters() {
            Quantity = 1;
            Speed    = RangeF.Parse("[-1.0,1.0]");
            Colour   = new ColourRange(new Colour(0f, 0.5f, 0.5f), new Colour(360f, 0.5f, 0.5f));
            Opacity  = RangeF.Parse("[0.0,1.0]");
            Scale    = RangeF.Parse("[1.0,10.0]");
            Rotation = RangeF.Parse("[-3.14159,3.14159]");
            Mass     = 1f;
        }

        [Description("Amount of particles to release each time the emitter is triggered.")]
        public Range Quantity { get; set; }
        [Description("Speed at which the particles are released " +
                     "(Note that direction is determined by the Profile of the emitter).")]
        public RangeF Speed { get; set; }
        [Description("Initial (British) HSL color of the particles.")]
        public ColourRange Colour { get; set; }
        [Description("Opacity of the colors on release.")]
        public RangeF Opacity { get; set; }
        [Description("The initial scale of the particle. (Use a modifier to set x and y scale separately")]
        public RangeF Scale { get; set; }
        [Description("Initial rotation of the particles in radians.")]
        public RangeF Rotation { get; set; }
        [Description("The mass of the particles. This is used in various modifiers.")]
        public RangeF Mass { get; set; }

    }
}