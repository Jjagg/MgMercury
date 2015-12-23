This is a version of the [Mercury Particle Engine](https://github.com/Matthew-Davey/mercury-particle-engine) usable with [MonoGame](https://github.com/mono/MonoGame). Basically to use the MPE all that's needed is a renderer that uses the MonoGame SpriteBatch and functions to convert HSL color to RGB. I made some other changes however, that made the engine more convenient to use IMO.

# Structure
Let's see how this all works! :)
We start out with a **ParticleEffect**. A ParticleEffect has one or more **Emitters**. An emitter shoots out particles whenever the parent effect is triggered. The initial properties of a particle are determined by the Profile of the emitter and the ReleaseParameters of the emitter. The *Texture* that the particles use is specified in their Emitter.

### Profiles
The **Profile** can be seen as the shape of the emitter. It determines where particles are released and what direction they go to. For example a Point profile sends out particles with a random direction from the center off the emitter, while a ring profile creates particles at a certain distance from the center and makes them go either towards the center or away from it.

### ReleaseParameters
The **ReleaseParameters** of an emitter determine the initial properties of the particles. Most of these are ranges from which a value gets picked randomly on releasing a particle. The number of particles released when the emitter is triggered is also set in here. Parameters specify initial *Velocity* (note: not direction, that's handled by the profile), *Colour* (which are HSL colors, I stuck with the original naming), *Opacity*, *Scale*, *Rotation* and *Mass* (for force modifiers).

### Modifiers
Of course that's not very flexible, so to actually do something with the particles we use **Modifiers**. An Emitter has a list of modifiers. When an effect is updated, it updates all it's emitters which then move all their particles and update all Modifiers. When updated a Modifier can change particle properties. The Modifier names pretty much speak for themselves.

## Example
A Game wich draws a simple particle effect to the screen can be found [here](https://gist.github.com/Jjagg/716924e108a84f0ace19).

# Changes to original MPE

### Modifications
 - Made the particle buffer circular. (it copied over particles at every reclaim)
 - Made an iterator for the particle buffer.
 - Changed particle to use structs rather than using arrays of floats (so you can say particle->Position.X instead of particle->Position[0])
 - Made scale a vector for separate x and y scaling.

### Additions
 - As mentioned before I added a ColorHelper class to convert colors.
 - New container modifiers
