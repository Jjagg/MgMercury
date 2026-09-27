using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using VenusParticleEngine.Core.Modifiers;
using VenusParticleEngine.Core.Profiles;
using Newtonsoft.Json;

namespace VenusParticleEngine.Core
{
    [Serializable]
    public unsafe class Emitter : IDisposable
    {
        public Emitter(int capacity, TimeSpan term, Profile profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            _term = (float)term.TotalSeconds;

            Buffer = new ParticleBuffer(capacity);
            Offset = new Vector();

            switch (profile.Name)
            {
                case nameof(Profile.EnumEmitterProfiles.BoxFillProfile):
                    Profile = Profile.BoxFill(profile.Width, profile.Height);
                    break;
                case nameof(Profile.EnumEmitterProfiles.BoxProfile):
                    Profile = Profile.Box(profile.Width, profile.Height);
                    break;
                case nameof(Profile.EnumEmitterProfiles.BoxUniformProfile):
                    Profile = Profile.BoxUniform(profile.Width, profile.Height);
                    break;
                case nameof(Profile.EnumEmitterProfiles.CircleProfile):
                    Profile = Profile.Circle(profile.Radius, profile.Radiate);
                    break;
                case nameof(Profile.EnumEmitterProfiles.LineProfile):
                    Profile = Profile.Line(profile.Axis, profile.Length);
                    break;
                case nameof(Profile.EnumEmitterProfiles.PointProfile):
                    Profile = Profile.Point();
                    break;
                case nameof(Profile.EnumEmitterProfiles.RingProfile):
                    Profile = Profile.Ring(profile.Radius, profile.Radiate);
                    break;
                case nameof(Profile.EnumEmitterProfiles.SprayProfile):
                    Profile = Profile.Spray(profile.Direction, profile.Spread);
                    break;
            }

            if (Profile == null)
                Profile = profile;

            Modifiers = new Dictionary<string, IModifier>();
            ModifierExecutionStrategy = ModifierExecutionStrategy.Serial();

            Parameters = new ReleaseParameters();
        }

        public int Capacity
        {
            get { return Buffer.Size; }
            set { Buffer = new ParticleBuffer(value); }
        }

        public TimeSpan Term
        {
            set { _term = (float)value.TotalSeconds; }
            get { return TimeSpan.FromSeconds(_term); }
        }

        private float _term;
        private int _lastQuantity = -1;
        private int _countToChangeQuantity = 0;

        private float _totalSeconds;
        internal ParticleBuffer Buffer;

        public int ActiveParticles => Buffer.Count;

        public SpriteEffects SpriteEffects = SpriteEffects.None;
        public float LayerDepth = 0;

        public BlendMode BlendMode { get; set; }

        public string TextureKey { get; set; }

        public float ReclaimFrequency { get; set; }

        public Vector Offset { get; set; }

        public Dictionary<string, IModifier> Modifiers { get; set; }

        public ModifierExecutionStrategy ModifierExecutionStrategy { get; set; }

        public Profile Profile { get; set; }

        public ReleaseParameters Parameters { get; set; }

        //TODO
        [JsonIgnore]
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                if (value != null && !string.IsNullOrEmpty(value.Name))
                {
                    TexturePath = Path.GetFileName(value.Name.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar));
                }
                _texture = value;
            }
        }

        public string TexturePath = "";
        private Texture2D _texture;

        public bool HasExpired = false;
        public bool Loop = true;
        public bool ForceLoop = false;

        private float _secondsSinceLastReclaim;

        private void ReclaimExpiredParticles()
        {
            var iterator = Buffer.Iterator;

            var expired = 0;
            HasExpired = false;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                if (_totalSeconds - particle->Inception < _term)
                    break;

                expired++;
                particle->StartAge = 0f;
            }
            if (expired != 0)
            {
                if (Loop || ForceLoop) Buffer.Reclaim(expired);
                HasExpired = true;
            }
        }

        public void Update(float elapsedSeconds)
        {
            _totalSeconds += elapsedSeconds;
            _secondsSinceLastReclaim += elapsedSeconds;

            if (Buffer.Count == 0)
            {
                return;
            }

            if (_secondsSinceLastReclaim > (1f / ReclaimFrequency))
            {
                ReclaimExpiredParticles();
                _secondsSinceLastReclaim -= (1f / ReclaimFrequency);
            }

            ReclaimExpiredParticles();

            var iterator = Buffer.Iterator;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Age = (_totalSeconds - particle->Inception) / _term;

                if (particle->StartAge == 0)
                {
                    particle->StartAge = particle->Age;
                    particle->Term = _term;
                }


                particle->Position = particle->Position + particle->Velocity * elapsedSeconds;
            }

            ModifierExecutionStrategy.ExecuteModifiers(Modifiers, elapsedSeconds, iterator);
        }

        public void Trigger(Vector position)
        {
            var numToRelease = FastRand.NextInteger(Parameters.Quantity);

            if (_lastQuantity != Parameters.Quantity.Min)
            {
                if (_lastQuantity > 0)
                    _countToChangeQuantity = 100;

                _lastQuantity = Parameters.Quantity.Min;
                numToRelease = 0;
            }

            if (_countToChangeQuantity > 0)
            {
                _countToChangeQuantity--;
                numToRelease = 0;
            }

            Release(position + Offset, numToRelease);
        }

        public void Trigger(LineSegment line)
        {
            var numToRelease = FastRand.NextInteger(Parameters.Quantity);
            var lineVector = line.ToVector();

            for (var i = 0; i < numToRelease; i++)
            {
                var offset = lineVector * FastRand.NextSingle();
                Release(line.Origin + offset, 1);
            }
        }

        private void Release(Vector position, int numToRelease)
        {
            var iterator = Buffer.Release(numToRelease);

            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                Axis heading;
                Profile.GetOffsetAndHeading(out particle->Position, out heading);

                particle->Age = 0f;

                particle->Inception = _totalSeconds;

                particle->Position += position;

                particle->TriggerPos = position;

                var speed = FastRand.NextSingle(Parameters.Speed);

                particle->Velocity = heading * speed;

                FastRand.NextColour(out particle->Colour, Parameters.Colour);

                particle->Opacity = FastRand.NextSingle(Parameters.Opacity);
                var scale = FastRand.NextSingle(Parameters.Scale);
                particle->Scale = new Vector(scale, scale);
                particle->Rotation = FastRand.NextSingle(Parameters.Rotation);
                particle->Mass = FastRand.NextSingle(Parameters.Mass);
            }
        }

        public void Dispose()
        {
            Buffer.Dispose();
            GC.SuppressFinalize(this);
        }

        ~Emitter()
        {
            Dispose();
        }
    }
}
