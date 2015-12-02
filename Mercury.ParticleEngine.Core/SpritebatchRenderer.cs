using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Svend.Util;

namespace MonoGameMPE.Mercury.ParticleEngine.Core
{
    public class SpriteBatchRenderer
    {

        public SpriteBatchRenderer()
        {
            
        }

        /// <summary>
        /// Draw a particle effect. This draw function calls spritebatch.Begin() and .End()
        /// </summary>
        public void Draw(ParticleEffect effect, SpriteBatch s)
        {
            foreach (var emitter in effect.Emitters)
            {
                Draw(emitter, s);
            }
        }

        private unsafe void Draw(Emitter emitter, SpriteBatch s)
        {
            var texture = emitter.Texture;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            var blendState = emitter.BlendMode == BlendMode.Add
                ? BlendState.Additive
                : BlendState.AlphaBlend;

            s.Begin(SpriteSortMode.Deferred, blendState);

            Particle* particle = (Particle*) emitter.Buffer.NativePointer;
            int count = emitter.Buffer.Count;

            while (count-- > 0)
            {
                var pos = new Vector2(particle->Position[0], particle->Position[1]);
                var color = ColorHelper.FromHsl(particle->Colour[0],
                    particle->Colour[1], particle->Colour[2]);
                if (blendState == BlendState.AlphaBlend)
                    color *= particle->Opacity;
                else
                    color.A = (byte) (particle->Opacity * 255);
                

                float a = particle->Opacity;
                s.Draw(texture, pos, null, null, origin, particle->Rotation,
                    new Vector2(particle->Scale),
                    new Color(color, a));

                particle++;
            }

            s.End();

        }
    }
}