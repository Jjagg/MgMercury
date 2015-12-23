using System;
using System.ComponentModel;
using MgMercury.Editor.TypeEditors;

namespace MonoGameMPE.Core
{
    [TypeConverter(typeof(ValueTypeTypeConverter))]
    public struct ColourRange
    {
        public ColourRange(Colour min, Colour max) {
            Min = min;
            Max = max;
        }

        public Colour Min { get; set; }
        public Colour Max { get; set; }

        public static ColourRange Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            try
            {
                var noBrackets = value.Substring(1, value.Length - 2);
                var colors = noBrackets.Split(';');
                var c1 = Colour.Parse(colors[0]);
                var c2 = Colour.Parse(colors[1]);
                return new ColourRange(c1, c2);
            }
            catch
            {
                throw new FormatException(
                    "ColorRange should be formatted like: [HUE°, SAT, LUM;HUE°, SAT, LUM], but was " +
                    value);
            }
        }

        public override string ToString()
        {
            return "[" + Min + ';' + Max + ']';
        }

        public static implicit operator ColourRange(Colour value) {
            return new ColourRange(value, value);
        }
    }
}