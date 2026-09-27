using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using VenusParticleEngine.Core;
using VenusParticleEngine.Core.Modifiers;
using VenusParticleEngine.Core.Modifiers.Container;
using VenusParticleEngine.Core.Profiles;

namespace Demo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private ParticleEffect _particleEffectImage;
        private ParticleEffect _particleEffectJson;
        private ParticleEffect _particleEffectJsonString;
        private ParticleEffect _particleEffectSquare;
        private Vector _particleJsonPosition = new Vector(700, 0);
        private SpriteBatch _spriteBatch;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            {
                _spriteBatch.Draw(_particleEffectSquare);
                _spriteBatch.Draw(_particleEffectImage);
                _spriteBatch.Draw(_particleEffectJson);
                _spriteBatch.Draw(_particleEffectJsonString);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ParticleInitSquare();
            ParticleInitImage();
            ParticleInitJson();
            ParticleInitJsonString();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //Square
            _particleEffectSquare.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectSquare.Trigger(new Vector(150, 350));

            //Image
            _particleEffectImage.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectImage.Trigger(new Vector(400, 250));

            //Json
            _particleEffectJson.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleJsonPosition.Y += 10;
            _particleEffectJson.Trigger(_particleJsonPosition);

            _particleEffectJsonString.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectJsonString.Trigger(new Vector(150, 150));

            if (_particleJsonPosition.Y > 1000)
            {
                _particleJsonPosition.Y = 0;
            }

            base.Update(gameTime);
        }

        private void ParticleInitImage()
        {
            _particleEffectImage = new ParticleEffect();
            Texture2D imageTexture = Content.Load<Texture2D>("heart");

            Dictionary<string, IModifier> Modifiers = new Dictionary<string, IModifier>
            {
                {
                     "effect1", new ScaleInterpolator2
                     {
                          InitialScale = new Vector(10,10),
                          FinalScale =  new Vector(20,20)
                     }
                },
                {
                     "effect2", new RotationModifier
                     {
                          RotationRate = 1f
                     }
                },
                {
                    "effect3", new LinearGravityModifier
                    {
                      Direction = new Axis(0,-10),
                      Strength = 100
                    }
                }
            };

            _particleEffectImage.Emitters.Add("test", new Emitter(200, TimeSpan.FromSeconds(2), Profile.Circle(50, Profile.CircleRadiation.Out))
            {
                Texture = imageTexture,
                BlendMode = BlendMode.Alpha,
                Parameters = new ReleaseParameters
                {
                    Speed = new RangeF(20f, 50f),
                    Quantity = 3,
                },
                Modifiers = Modifiers
            });
        }

        private void ParticleInitJson()
        {
            _particleEffectJson = ParticleEffect.ReadFromJsonFile(Path.Combine(Content.RootDirectory, "testParticle.ptc"), GraphicsDevice, Content);
            if (_particleEffectJson == null)
            {
                throw new InvalidOperationException("Unable to load the particle effect from testParticle.ptc.");
            }

            foreach (var item in _particleEffectJson.Emitters)
            {
                if (string.IsNullOrEmpty(item.Value.TexturePath))
                {
                    item.Value.Texture = new Texture2D(GraphicsDevice, 1, 1);
                    item.Value.Texture.SetData(new[] { Color.White });
                }
            }
        }

        private void ParticleInitJsonString()
        {
            var fileContents = File.ReadAllText(Path.Combine(Content.RootDirectory, "testParticle2.ptc"));

            if (!string.IsNullOrWhiteSpace(fileContents))
            {
                _particleEffectJsonString = ParticleEffect.ReadFromJson(
                    Content.RootDirectory,
                    fileContents,
                    GraphicsDevice, Content);

                if (_particleEffectJsonString == null)
                {
                    throw new InvalidOperationException("Unable to load the particle effect from testParticle2.ptc.");
                }

                foreach (var item in _particleEffectJsonString.Emitters)
                {
                    if (string.IsNullOrEmpty(item.Value.TexturePath))
                    {
                        item.Value.Texture = new Texture2D(GraphicsDevice, 1, 1);
                        item.Value.Texture.SetData(new[] { Color.White });
                    }
                }
            }
        }

        private void ParticleInitSquare()
        {
            _particleEffectSquare = new ParticleEffect();
            Texture2D blankTexture = new Texture2D(GraphicsDevice, 1, 1);
            blankTexture.SetData(new[] { Color.White });

            Dictionary<string, IModifier> Modifiers = new Dictionary<string, IModifier>
            {
                {
                    "effect1", new ColourInterpolator2
                    {
                        InitialColour = new Colour(0.33f, 0.5f, 0.5f),
                        FinalColour = new Colour(0f, 0.5f, 0.5f)
                    }
                },
                {
                     "effect2", new RotationModifier
                     {
                          RotationRate = 1f
                     }
                },
                {
                    "effect3", new RectContainerModifier
                    {
                        Height = 100,
                        Width = 100,
                    }
                }
            };

            _particleEffectSquare.Emitters.Add("test", new Emitter(2000, TimeSpan.FromSeconds(2), Profile.Point())
            {
                Texture = blankTexture,
                BlendMode = BlendMode.Alpha,
                Parameters = new ReleaseParameters
                {
                    Speed = new RangeF(20f, 50f),
                    Quantity = 3,
                },
                Modifiers = Modifiers
            });
        }
    }
}
