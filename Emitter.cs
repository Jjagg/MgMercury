﻿using System;
using System.Numerics;
using Mercury3D.Modifiers;
using Mercury3D.Profiles;

namespace Mercury3D
{

    public unsafe class Emitter : IDisposable
    {

        public Emitter(int capacity, TimeSpan term, Profile profile) {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            _term = (float)term.TotalSeconds;

            Buffer = new ParticleBuffer(capacity);
            Offset = new Vector3();
            Profile = profile;
            Modifiers = new IModifier[0];
            ModifierExecutionStrategy = ModifierExecutionStrategy.Serial;
            Parameters = new ReleaseParameters();
            BlendMode = BlendMode.Alpha;
        }
        
        private readonly float _term;

        private float _totalSeconds;
        public ParticleBuffer Buffer { get; }
        
        public int ActiveParticles => Buffer.Count;
        public int Capacity => Buffer.Size;
        
        public Vector3 Offset { get; set; }
        
        public IModifier[] Modifiers { get; set; }
        
        public ModifierExecutionStrategy ModifierExecutionStrategy { get; set; }
        
        public Profile Profile { get; }
        public ReleaseParameters Parameters { get; set; }
        public BlendMode BlendMode { get; set; }

        public object UserData { get; set; }

        public void ReclaimParticles(int count)
        {
            Buffer.Reclaim(count);
        }

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

        public void Trigger(Vector3 position) {
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

        private void Release(Vector3 position, int numToRelease) {
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

                FastRand.NextColor(out particle->Color, Parameters.Colour);

                particle->Opacity  = FastRand.NextSingle(Parameters.Opacity);
                var scale = FastRand.NextSingle(Parameters.Scale);
                particle->Scale    = new Vector2(scale);
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
