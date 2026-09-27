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
        private ParticleEffect _particleEffectOpacityContainer;
        private ParticleEffect _particleEffectSquare;
        private ParticleEffect _particleEffectSpray;
        private ParticleEffect _particleEffectVortex;
        private Vector _particleJsonPosition = new Vector(800, 80);
        private SpriteBatch _spriteBatch;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Venus Particle Engine Demo — press Escape to exit";
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
                _spriteBatch.Draw(_particleEffectOpacityContainer);
                _spriteBatch.Draw(_particleEffectVortex);
                _spriteBatch.Draw(_particleEffectSpray);
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
            ParticleInitDiagnostics();
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
            _particleEffectSquare.Trigger(new Vector(1100, 170));

            //Image
            _particleEffectImage.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectImage.Trigger(new Vector(480, 170));

            //Json
            _particleEffectJson.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleJsonPosition.Y += 3;
            _particleEffectJson.Trigger(_particleJsonPosition);

            _particleEffectJsonString.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectJsonString.Trigger(new Vector(160, 170));

            _particleEffectOpacityContainer.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectOpacityContainer.Trigger(new Vector(250, 530));

            _particleEffectVortex.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectVortex.Trigger(new Vector(640, 530));

            _particleEffectSpray.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _particleEffectSpray.Trigger(new Vector(1030, 640));

            if (_particleJsonPosition.Y > 270)
            {
                _particleJsonPosition.Y = 80;
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

        private void ParticleInitDiagnostics()
        {
            var blankTexture = new Texture2D(GraphicsDevice, 1, 1);
            blankTexture.SetData(new[] { Color.White });

            _particleEffectOpacityContainer = new ParticleEffect();
            _particleEffectOpacityContainer.Emitters.Add("opacity-container",
                new Emitter(180, TimeSpan.FromSeconds(2), Profile.Circle(55f, Profile.CircleRadiation.Out))
                {
                    Texture = blankTexture,
                    BlendMode = BlendMode.Alpha,
                    Parameters = new ReleaseParameters
                    {
                        Quantity = 2,
                        Speed = 80f,
                        Colour = new Colour(180f, 1f, 0.5f),
                        Opacity = 1f,
                        Scale = 12f
                    },
                    Modifiers = new Dictionary<string, IModifier>
                    {
                        ["Hue"] = new HueInterpolator2(180f, 300f),
                        ["Opacity"] = new OpacityInterpolator3(0f, 1f, 0f),
                        ["CircleContainer"] = new CircleContainerModifier(110f, true, 0.85f),
                        ["Rotation"] = new RotationModifier(4f)
                    }
                });

            _particleEffectVortex = new ParticleEffect();
            _particleEffectVortex.Emitters.Add("vortex",
                new Emitter(120, TimeSpan.FromSeconds(2.5), Profile.Ring(95f, Profile.CircleRadiation.None))
                {
                    Texture = blankTexture,
                    BlendMode = BlendMode.Add,
                    Parameters = new ReleaseParameters
                    {
                        Quantity = 1,
                        Speed = 45f,
                        Colour = new Colour(210f, 1f, 0.6f),
                        Opacity = 0.9f,
                        Scale = 9f,
                        Mass = 1f
                    },
                    Modifiers = new Dictionary<string, IModifier>
                    {
                        ["Vortex"] = new VortexModifier(Vector.Zero, 0.4f, 200f),
                        ["VelocityColour"] = new VelocityColourModifier(
                            new Colour(210f, 1f, 0.6f), new Colour(30f, 1f, 0.6f), 80f),
                        ["Fade"] = new OpacityFastFadeModifier()
                    }
                });

            _particleEffectSpray = new ParticleEffect();
            _particleEffectSpray.Emitters.Add("spray",
                new Emitter(180, TimeSpan.FromSeconds(1.8), Profile.Spray(Axis.Up, MathHelper.PiOver4))
                {
                    Texture = blankTexture,
                    BlendMode = BlendMode.Alpha,
                    Parameters = new ReleaseParameters
                    {
                        Quantity = 2,
                        Speed = new RangeF(100f, 160f),
                        Colour = new Colour(45f, 1f, 0.55f),
                        Opacity = 1f,
                        Scale = new RangeF(6f, 12f)
                    },
                    Modifiers = new Dictionary<string, IModifier>
                    {
                        ["Gravity"] = new LinearGravityModifier(Axis.Down, 70f),
                        ["VelocityHue"] = new VelocityHueModifier(15f, 55f, 160f),
                        ["Fade"] = new OpacityFastFadeModifier()
                    }
                });
        }
    }
}
