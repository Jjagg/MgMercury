﻿using System;
using Microsoft.Xna.Framework;
using MonoGameMPE.Core;

namespace MonoGameMPE
{
    public static class ColorHelper
    {

        public static Color ToRgb(this Colour c)
        {
            return ToRgb(c.H, c.S, c.L);
        }

        public static Color ToRgb(float h, float s, float l)
        {
            if (s == 0)
                return new Color(l, l, l);

            h = h/360f;
            var max = l < 0.5f ? l * (1 + s) : (l + s) - (l * s);
            var min = 2f * l - max;
            
            return new Color(
                ComponentFromHue(min,max,h + 1f/3f),
                ComponentFromHue(min, max, h),
                ComponentFromHue(min, max, h - 1f/3f));
        }

        private static float ComponentFromHue(float m1, float m2, float h)
        {
            h = (h + 1f) % 1f;
            if (h*6f < 1)
                return m1 + (m2 - m1)*6f*h;
            if (h*2 < 1)
                return m2;
            if (h*3 < 2)
                return m1 + (m2 - m1)*(2f/3f - h)*6f;
            return m1;
        }

        public static Colour ToHsl(this Color c)
        {
            return ToHsl(c.R, c.B, c.G);
        }

        public static Colour ToHsl(float r, float b, float g)
        {
            r = r/255f;
            b = b/255f;
            g = g/255f;

            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var chroma = max - min;
            var sum = max + min;

            var l = sum*0.5f;
            
            if (chroma == 0) return new Colour(0f, 0f, l);
            float h;
            if (r == max)
                h = (60 * (g - b)/chroma + 360) % 360;
            else if (g == max)
                h = 60 * (b - r)/chroma + 120f;
            else
                h = 60 * (r - g)/chroma + 240f;
            
            var s = l <= 0.5f ? chroma / sum : chroma / (2f - sum);

            return new Colour(h, s, l);
        }
    }
}
