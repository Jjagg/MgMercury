using System;
using System.Collections.Generic;
using VenusParticleEngine.Core.Modifiers;

namespace VenusParticleEngine.Core
{
    [Serializable]
    public class ModifierExecutionStrategy
    {
        public string Name = "";
        public enum EnumModifierExecutionStrategy { Serial, Parallel };

        public ModifierExecutionStrategy()
        {
            Name = ToString();
        }

        public virtual void ExecuteModifiers(Dictionary<string, IModifier> modifiers, float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
        }

        public override string ToString()
        {
            int startNameProfile = GetType().ToString().LastIndexOf(".") + 1;
            string profileName = GetType().ToString().Substring(startNameProfile);
            int endNameProfile = profileName.IndexOf("Mo");
            profileName = profileName.Substring(0, endNameProfile);

            if (profileName.Equals(""))
            {
                profileName = "NOT_SET";
            }

            return profileName;
        }

        public static ModifierExecutionStrategy Serial() { return new SerialModifierExecutionStrategy(); }
        public static ModifierExecutionStrategy Parallel() { return new ParallelModifierExecutionStrategy(); }
    }


    [Serializable]
    internal class ParallelModifierExecutionStrategy : ModifierExecutionStrategy
    {
        public override void ExecuteModifiers(Dictionary<string, IModifier> modifiers, float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            // Modifiers can write to the same particle fields, so executing them concurrently
            // produces races and non-deterministic results. Keep the existing strategy name for
            // serialized effects, but use independent iterators and deterministic execution.
            foreach (var modifier in modifiers)
                modifier.Value.Update(elapsedSeconds, iterator.CreateIndependentIterator());
        }
    }

    [Serializable]
    internal class SerialModifierExecutionStrategy : ModifierExecutionStrategy
    {
        public override void ExecuteModifiers(Dictionary<string, IModifier> modifiers, float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            foreach (var modifier in modifiers)
                modifier.Value.Update(elapsedSeconds, iterator.Reset());
        }
    }
}
