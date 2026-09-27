This repository is a version of the [MgMercury](https://github.com/Jjagg/MgMercury) usable with [MonoGame](https://github.com/mono/MonoGame). <br />
MgMercury is a particle engine based on  [Mercury Particle Engine](https://github.com/Matthew-Davey/mercury-particle-engine)

![](https://media.giphy.com/media/bbSYumsbuysZEpqpgN/giphy.gif)

# News with Venus:
 - You can choose custom camera do draw particle
 - New Modifier OpacityInterpolator3 with 3 states (initial,medium,final)
 - ParticleEffect now have play/pause
 - Emmiter have public bool "Loop", when false, the particle will show only one time. But if you want to show again, set "ForceLoop = true" at any time.
 - Json support to Load particles from file

## Depedencies
 - [Newtonsoft.Json 13.0.4](https://www.nuget.org/packages/Newtonsoft.Json/13.0.4)
 - [MonoGame 3.8.5.1](https://www.nuget.org/profiles/MonoGame)
 - .NET SDK 9.0.200 or newer (required for `.slnx` support)

Venus uses the common MonoGame APIs and does not impose a runtime platform on consuming applications. Use MonoGame 3.8.5.1 and choose the platform package that matches your application (for example, `MonoGame.Framework.DesktopGL`, `MonoGame.Framework.Android`, or `MonoGame.Framework.iOS`). The included Demo uses `MonoGame.Framework.DesktopGL` for Windows, macOS, and Linux.

For mobile and console builds, prefer `ReadFromJson` with JSON supplied by the application's asset system and textures processed by MGCB (`.xnb`). `ReadFromJsonFile` and raw image paths remain available for externally readable files.

## Build

```bash
dotnet tool restore
dotnet build "Venus Particle Engine.slnx" --configuration Release
dotnet run --project Demo/Demo.csproj
dotnet pack VenusParticleEngine.csproj --configuration Release
```

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
A ParticleEffect has one or more **Emitters**. An emitter shoots out particles whenever the parent effect is triggered. The initial properties of a particle are determined by the Profile of the emitter and the ReleaseParameters of the emitter. The *Texture* that the particles use is specified in their Emitter.

## Profiles
The **Profile** can be seen as the shape of the emitter. It determines where particles are released and what direction they go to. For example a Point profile sends out particles with a random direction from the center off the emitter, while a ring profile creates particles at a certain distance from the center and makes them go either towards the center or away from it.

## ReleaseParameters
The **ReleaseParameters** of an emitter determine the initial properties of the particles. Most of these are ranges from which a value gets picked randomly on releasing a particle. The number of particles released when the emitter is triggered is also set in here. Parameters specify initial *Velocity* (note: not direction, that's handled by the profile), *Colour* (which are HSL colors, I stuck with the original naming), *Opacity*, *Scale*, *Rotation* and *Mass* (for force modifiers).

## Modifiers
Of course that's not very flexible, so to actually do something with the particles we use **Modifiers**. An Emitter has a list of modifiers. When an effect is updated, it updates all it's emitters which then move all their particles and update all Modifiers. When updated a Modifier can change particle properties. The Modifier names pretty much speak for themselves.

# Example
You can see example inside de [Demo folder](https://github.com/mmbelkiman/Venus-Particle-Engine/tree/master/Demo)

# Changes to MgMercury
 - Emmiter (Inside ParticleEffect) now is Dictionary (string, Emitter)
 - Modifiers (Inside Emmiter) now is Dictionary (string, IModifier)

# Changes to original Mercury particle editor

## Modifications by mgMercury
 - Made the particle buffer circular. (it copied over particles at every reclaim)
 - Made an iterator for the particle buffer.
 - Changed particle to use structs rather than using arrays of floats (so you can say particle->Position.X instead of particle->Position[0])
 - Made scale a vector for separate x and y scaling.
 - A ColorHelper class to convert colors.
 - New container modifiers

[![kofi](https://az743702.vo.msecnd.net/cdn/kofi2.png)](https://ko-fi.com/B0B2KE8I)
