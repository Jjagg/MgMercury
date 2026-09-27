This repository is a version of the [MgMercury](https://github.com/Jjagg/MgMercury) usable
with [MonoGame](https://github.com/mono/MonoGame). <br />
MgMercury is a particle engine based
on  [Mercury Particle Engine](https://github.com/Matthew-Davey/mercury-particle-engine)

![](https://media.giphy.com/media/bbSYumsbuysZEpqpgN/giphy.gif)

# News with Venus:

- You can choose custom camera do draw particle
- New Modifier OpacityInterpolator3 with 3 states (initial,medium,final)
- ParticleEffect now have play/pause
- Emmiter have public bool "Loop", when false, the particle will show only one time. But if you want to show again, set
  "ForceLoop = true" at any time.
- Json support to Load particles from file
- Particle triggers now emit immediately and honor the configured quantity; previously the first trigger emitted nothing
  and changing the quantity delayed emission by 100 triggers.
- `ColorHelper.ToHsl(float, float, float)` now uses the conventional `R, G, B` channel order; previously it interpreted
  its arguments as `R, B, G`.
- `Colour.Lerp` now interpolates lightness from the initial colour to the final colour; previously lightness always
  remained at its initial value.
- Effects marked as `Parallel` now run modifiers deterministically with independent iterators; previously concurrent
  modifiers shared one iterator and could race while writing particles.
- `VortexModifier` now leaves a particle unchanged when it is exactly at the vortex center; previously this division by
  zero could produce `NaN` velocity values.
- `ParticleBuffer` now rejects invalid release and reclaim quantities instead of moving unmanaged pointers outside valid
  bounds, and `LineSegment.CopyTo` now writes its two vectors contiguously.

## Depedencies

- [Newtonsoft.Json 13.0.4](https://www.nuget.org/packages/Newtonsoft.Json/13.0.4)
- [MonoGame 3.8.5.1](https://www.nuget.org/profiles/MonoGame)
- .NET SDK 9.0.200 or newer (required for `.slnx` support)

Venus uses the common MonoGame APIs and does not impose a runtime platform on consuming applications. Use MonoGame
3.8.5.1 and choose the platform package that matches your application (for example, `MonoGame.Framework.DesktopGL`,
`MonoGame.Framework.Android`, or `MonoGame.Framework.iOS`). The included Demo uses `MonoGame.Framework.DesktopGL` for
Windows, macOS, and Linux.

For mobile and console builds, prefer `ReadFromJson` with JSON supplied by the application's asset system and textures
processed by MGCB (`.xnb`). `ReadFromJsonFile` and raw image paths remain available for externally readable files.

## Build

Install the .NET SDK 9.0.200 or newer, then restore the repository-local tools once after cloning:

```bash
dotnet tool restore
```

To validate and build every project, including the Demo and unit tests:

```bash
dotnet build "Venus Particle Engine.slnx" --configuration Release
```

To build only the library and produce its assembly files:

```bash
dotnet build VenusParticleEngine.csproj --configuration Release
```

This produces the following files:

- `bin/Release/net9.0/VenusParticleEngine.dll`: the library used at runtime.
- `bin/Release/net9.0/VenusParticleEngine.pdb`: optional debug symbols, which enable source-level debugging when placed
  next to the DLL.

To run the DesktopGL Demo:

```bash
dotnet run --project Demo/Demo.csproj
```

To create the NuGet package:

```bash
dotnet pack VenusParticleEngine.csproj --configuration Release
```

### Use the DLL in another project

The recommended distribution method is the NuGet package. If you need a direct file reference, copy
`VenusParticleEngine.dll` to your consuming project and optionally copy `VenusParticleEngine.pdb` next to it for
debugging. Add a reference to the DLL and the MonoGame package appropriate for the target platform:

```xml
<ItemGroup>
  <Reference Include="VenusParticleEngine">
    <HintPath>lib/VenusParticleEngine.dll</HintPath>
  </Reference>
  <PackageReference Include="MonoGame.Framework.DesktopGL" Version="3.8.5.1" />
  <PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
</ItemGroup>
```

Replace `MonoGame.Framework.DesktopGL` with the MonoGame package for your application platform when necessary. Pre-built
DLL and PDB files are also available from
the [GitHub Releases](https://github.com/mbelkiman/venus-particle-engine/releases) page.

## Quality checks

GitHub Actions runs the following checks for every pull request. You can also run any individual check locally when it
is relevant to your change:

To verify C# whitespace formatting:

```bash
dotnet format whitespace "Venus Particle Engine.slnx" --verify-no-changes
```

To build the solution in Release:

```bash
dotnet build "Venus Particle Engine.slnx" --configuration Release
```

To run the unit tests and measure line coverage:

```bash
dotnet test Tests/VenusParticleEngine.Tests.csproj --configuration Release \
  /p:CollectCoverage=true /p:CoverletOutput=coverage.xml /p:CoverletOutputFormat=opencover \
  /p:Threshold=1 /p:ThresholdType=line
```

To generate a human-readable HTML report from an existing `Tests/coverage.xml` file:

```bash
dotnet tool run reportgenerator -reports:Tests/coverage.xml -targetdir:Tests/coverage \
  -reporttypes:Html
```

Run `dotnet tool restore` once after cloning the repository to install the local tools used by the Demo and the HTML
report. The HTML report is generated locally at `Tests/coverage/index.html` and is not uploaded by the CI job.

## Load from Json

You can load a json particle file using

```
ParticleEffect _particleEffectJson = ParticleEffect.ReadFromJsonFile(string filePath, GraphicsDevice graphicsDevice, ContentManager content)
```

If your particle don't have a image annexed, you need to create a texture, like this

```
foreach (var item in _particleEffectJson.Emitters)
{
    if (string.IsNullOrEmpty(item.Value.TexturePath))
    {
        item.Value.Texture = new Texture2D(GraphicsDevice, 1, 1);
        item.Value.Texture.SetData(new[] { Color.White });
    }
}
```

## Particle Editor

Venus has an editor that can upload and save json files.  <br />
[![Itch.io](https://jessemillar.com/available-on-itchio-badge/badge-bw.png)](https://mmbelkiman.itch.io/venus-particle-editor) <br />

[Repository](https://github.com/mmbelkiman/Venus-Particle-Editor)

# Structure

## ParticleEffect

A ParticleEffect has one or more **Emitters**. An emitter shoots out particles whenever the parent effect is triggered.
The initial properties of a particle are determined by the Profile of the emitter and the ReleaseParameters of the
emitter. The *Texture* that the particles use is specified in their Emitter.

## Profiles

The **Profile** can be seen as the shape of the emitter. It determines where particles are released and what direction
they go to. For example a Point profile sends out particles with a random direction from the center off the emitter,
while a ring profile creates particles at a certain distance from the center and makes them go either towards the center
or away from it.

## ReleaseParameters

The **ReleaseParameters** of an emitter determine the initial properties of the particles. Most of these are ranges from
which a value gets picked randomly on releasing a particle. The number of particles released when the emitter is
triggered is also set in here. Parameters specify initial *Velocity* (note: not direction, that's handled by the
profile), *Colour* (which are HSL colors, I stuck with the original naming), *Opacity*, *Scale*, *Rotation* and *Mass*
(for force modifiers).

## Modifiers

Of course that's not very flexible, so to actually do something with the particles we use **Modifiers**. An Emitter has
a list of modifiers. When an effect is updated, it updates all it's emitters which then move all their particles and
update all Modifiers. When updated a Modifier can change particle properties. The Modifier names pretty much speak for
themselves.

# Example

You can see example inside de [Demo folder](https://github.com/mmbelkiman/Venus-Particle-Engine/tree/master/Demo)

# Changes to MgMercury

- Emmiter (Inside ParticleEffect) now is Dictionary (string, Emitter)
- Modifiers (Inside Emmiter) now is Dictionary (string, IModifier)

# Changes to original Mercury particle editor

## Modifications by mgMercury

- Made the particle buffer circular. (it copied over particles at every reclaim)
- Made an iterator for the particle buffer.
- Changed particle to use structs rather than using arrays of floats (so you can say particle->Position.X instead of
  particle->Position[0])
- Made scale a vector for separate x and y scaling.
- A ColorHelper class to convert colors.
- New container modifiers