﻿using System;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;
using MonoGameMPE.Core.Modifiers;
using MonoGameMPE.Core.Profiles;

namespace MonoGameMPE.Core
{

    public unsafe class Emitter : IDisposable
    {

        public Emitter(int capacity, TimeSpan term, Profile profile) {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            _term = (float)term.TotalSeconds;

            Buffer = new ParticleBuffer(capacity);
            Offset = new Vector();
            Profile = profile;
            Modifiers = new IModifier[0];
            ModifierExecutionStrategy = ModifierExecutionStrategy.Serial;
            Parameters = new ReleaseParameters();
        }

        [Description("How long each particle will exist. To change this value, reinitialize the emitter.")]
        private readonly float _term;

        [Browsable(false)] private float _totalSeconds;
        [Browsable(false)] internal readonly ParticleBuffer Buffer;

        [Browsable(false)]
        public int ActiveParticles => Buffer.Count;

        [Description("The name of this emitter.")]
        public string Name { get; set; }
        public Vector Offset { get; set; }
        [Browsable(false)]
        public IModifier[] Modifiers { get; set; }

        [Browsable(false)]
        public ModifierExecutionStrategy ModifierExecutionStrategy { get; set; }

        [Description("Determines initial particle position and direction. To change this value, reinitialize the emitter.")]
        public Profile Profile { get; }
        [Description("Release values for the particles.")]
        public ReleaseParameters Parameters { get; set; }
        [Description("Blend mode used when drawing particles on top of each other.")]
        public BlendMode BlendMode { get; set; }
        [Description("Name of the texture of the particles.")]
        public string TextureKey { get; set; }
        [Browsable(false)]
        public Texture2D Texture { get; set; }

        private void ReclaimExpiredParticles()
        {

            var iterator = Buffer.Iterator;

            var expired = 0;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();

                if (_totalSeconds - particle->Inception < _term)
                    break;

                expired++;
            }
            if (expired != 0)
                Buffer.Reclaim(expired);
        }

        public void Update(float elapsedSeconds) {
            _totalSeconds += elapsedSeconds;

            if (Buffer.Count == 0)
            {
                return;
            }

            ReclaimExpiredParticles();

            var iterator = Buffer.Iterator;

            while (iterator.HasNext)
            {
                var particle = iterator.Next();
                particle->Age = (_totalSeconds - particle->Inception) / _term;

                particle->Position = particle->Position + particle->Velocity * elapsedSeconds;
            }

            ModifierExecutionStrategy.ExecuteModifiers(Modifiers, elapsedSeconds, iterator);
        }

        public void Trigger(Vector position) {
            var numToRelease = FastRand.NextInteger(Parameters.Quantity);

            Release(position + Offset, numToRelease);
        }

        public void Trigger(LineSegment line) {
            var numToRelease = FastRand.NextInteger(Parameters.Quantity);
            var lineVector = line.ToVector();

            for (var i = 0; i < numToRelease; i++) {
                var offset = lineVector * FastRand.NextSingle();
                Release(line.Origin + offset, 1);
            }
        }

        private void Release(Vector position, int numToRelease) {
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

                particle->Opacity  = FastRand.NextSingle(Parameters.Opacity);
                var scale = FastRand.NextSingle(Parameters.Scale);
                particle->Scale    = new Vector(scale, scale);
                particle->Rotation = FastRand.NextSingle(Parameters.Rotation);
                particle->Mass     = FastRand.NextSingle(Parameters.Mass);
            }
        }

        public void Dispose() {
            Buffer.Dispose();
            GC.SuppressFinalize(this);
        }

        ~Emitter() {
            Dispose();
        }
    }
}
