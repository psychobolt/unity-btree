# TakeDamageBehaviour

## Required Components

- [EnemyActor](EnemyActor.md)
- Custom Animation Controller (AbstractAnimationController)

## Parameters

- Damage Time - Duration in seconds
- Revive Time - Duration in seconds
- Cooldown Time - Behavior is triggered after cooldown timer is 0
- Hit Type 
  - HP_PERCENTAGE - Percentage of health missing (greater or equal)
  - DMG_PERCENTAGE - Percentage of health removed from last health (greater or equal)
  - POINTS - Amount of damage points taken
- Hit Value - Specified damage threshold based on Hit Type.
- Revive Percentage - Amount of health to recover after being damaged