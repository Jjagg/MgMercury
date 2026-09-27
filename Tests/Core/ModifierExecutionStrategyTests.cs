using System.Collections.Concurrent;
using VenusParticleEngine.Core;
using VenusParticleEngine.Core.Modifiers;
using Xunit;

namespace VenusParticleEngine.Tests.Core;

public sealed class ModifierExecutionStrategyTests
{
    [Fact]
    public void ParallelExecution_UsesAnIndependentIteratorForEachModifier()
    {
        using var buffer = new ParticleBuffer(1);
        buffer.Release(1);

        var first = new RecordingModifier();
        var second = new RecordingModifier();
        var modifiers = new Dictionary<string, IModifier>
        {
            ["first"] = first,
            ["second"] = second
        };

        ModifierExecutionStrategy.Parallel().ExecuteModifiers(modifiers, 0f, buffer.Iterator);

        Assert.NotSame(first.Iterators.Single(), second.Iterators.Single());
    }

    private sealed class RecordingModifier : IModifier
    {
        public ConcurrentBag<ParticleBuffer.ParticleIterator> Iterators { get; } = new();

        public void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator)
        {
            Iterators.Add(iterator);
        }
    }
}
