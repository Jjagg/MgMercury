using Microsoft.Xna.Framework;

namespace MonoGameMPE
{
    public class ColorHelper
    {
        public static Color FromHsl(float h, float s, float l)
        {
            var r = l;
            var g = l;
            var b = l;
            var v = l <= 0.5 ? l * (1.0f + s) : l + s - l * s;

            if (!(v > 0)) return new Color(r, g, b);

            float m = l + l - v;
            float sv = (v - m) / v;
            h *= 6.0f;
            var sextant = (int)h;
            float fract = h - sextant;
            var vsf = v * sv * fract;
            var mid1 = m + vsf;
            var mid2 = v - vsf;
            switch (sextant)
            {
                case 0:
                    r = v;
                    g = mid1;
                    b = m;
                    break;
                case 1:
                    r = mid2;
                    g = v;
                    b = m;
                    break;
                case 2:
                    r = m;
                    g = v;
                    b = mid1;
                    break;
                case 3:
                    r = m;
                    g = mid2;
                    b = v;
                    break;
                case 4:
                    r = mid1;
                    g = m;
                    b = v;
                    break;
                case 5:
                    r = v;
                    g = m;
                    b = mid2;
                    break;
            }
            return new Color(r, g, b);
        }
    }
}