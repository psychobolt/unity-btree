# RelocateObjectBehaviour

Relocates a target to a random valid position in specified areas or in world space.

## Required

- Pathfinder2D (Simply A*)

## Parameters

- Target - Target to be relocated
- Areas - An array of Game Objects to relocate to. Allows relocating in random space within:
    - SphereCollider
    - CircleCollider2D
- Freeze x - Do not update x position
- Freeze y - Do not update y position
- Freeze z - Do not update z position
- Allow hidden - Unrestricted relocation. Having this option off means relocate within visible space (Camera's view frustrum)
- Fail if invalid - Return FAILURE node state if failed to relocate to valid location