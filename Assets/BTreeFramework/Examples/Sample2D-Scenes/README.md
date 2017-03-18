# Sample 2D Scenes

## Scenes

### Death AI Pathfinding

<img width="500" src="Assets/Boss-AI/Death/preview.gif" />

Simple scene for testing pathfinding and mechanim states. AI will respawn upon death or low health (< 50%)

#### Controls:
| Key                       | Action                |
| --------------------------|:---------------------:|
| Left-Click on DeathBossAI | Hit AI for -10 pts    |
| Up Arrow                  | Camera move up        |
| Down Arrow                | Camera move down      |
| Left Arrow                | Camera move left      |
| Right Arrow               | Camera move right     |

#### Actors: 
- DeathBossAI

#### Behaviors:
- Death
- Take Damage
  - Teleport/Flee
- Attack
  - Melee Attack
  - Random Range Attacks

## Base Scripts

### Behaviors

New behaviors can be created by combining and mixing a variety of base behaviours

- [MeleeBehaviour](Assets/Boss-AI/Scripts/MeleeBehaviour.md)
- [RangeBehaviour](Assets/Boss-AI/Scripts/RangeBehaviour.md)
- [RelocateObjectBehaviour](Assets/Boss-AI/Scripts/RangeBehaviour.md)
- [TakeDamageBehaviour](Assets/Boss-AI/Scripts/TakeDamageBehaviour.md)

## 3rd Party Assets

- [Steer2D](https://www.assetstore.unity3d.com/en/#!/content/21381)
- [Simply A* Pathfinding](https://www.assetstore.unity3d.com/en/#!/content/6385)
