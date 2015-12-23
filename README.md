This is a version of the [Mercury Particle Engine](https://github.com/Matthew-Davey/mercury-particle-engine) usable with Monogame. Basically to use MPE all that's needed is a renderer that uses the MonoGame SpriteBatch and functions to get RGB from HSL and vice versa. I made some other changes however, that made the engine more convenient to use (for my specific use case at least).

## Modifications
 - Made the particle buffer circular. (it copied over particles at every reclaim)
 - Made an iterator for the particle buffer.
 - Changed particle to use structs rather than using arrays of floats (so you can say particle->Position.X instead of particle->Position[0])

## Additions
 - As mentioned before I added a ColorHelper class to convert colors.
 - New container modifiers
 