using System;
using OpenWheels;

namespace Mercury3D
{
    public struct ColourRange
    {
        public readonly HsvColor Min;
        public readonly HsvColor Max;

        public ColourRange(Color min, Color max) {
            Min = min;
            Max = max;
        }

        public static ColourRange Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            try
            {
                var noBrackets = value.Substring(1, value.Length - 2);
                var colors = noBrackets.Split(';');
                var c1 = Color.Parse(colors[0]);
                var c2 = Color.Parse(colors[1]);
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

        public static implicit operator ColourRange(Color value) {
            return new ColourRange(value, value);
        }
    }
}
