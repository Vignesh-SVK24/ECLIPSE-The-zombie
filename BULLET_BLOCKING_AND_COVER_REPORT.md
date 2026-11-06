# BULLET BLOCKING AND COVER REPORT — ECLIPSE: THE LAST CIVILIZATION

## 1. Tactical Cover Classifications
1. **Heavy Cover** (`Trucks`, `Ambulances`, `Concrete Barriers`, `Shipping Containers`, `Closed Steel Doors`):
   - 100% blocks character movement, explosive blast fragments, and all ballistic projectiles.
   - Bullet damage reduction: 100%.
2. **Medium Cover** (`Cars`, `SUVs`, `Metal Barricades`, `Dumpsters`):
   - Blocks character movement.
   - Blocks bullets striking solid body panels/engine blocks. Clear window paths allow bullet passthrough.
3. **Light Cover** (`Wooden Supply Crates`, `Light Fences`):
   - Blocks character movement.
   - High-velocity rifle rounds penetrate with 45% damage passthrough to targets directly behind.

## 2. Raycast Evaluation Pipeline
```
Player Fire Ray -> Intersect Cover Obstacles & Zombies
               |-> Cover Distance < Zombie Distance:
                     - Bullet impacts cover mesh
                     - Spawns impact sparks & ricochet dust
                     - Damage Text: "[BLOCKED]"
                     - Zombie takes 0 damage (fully shielded!)
               |-> Zombie Distance < Cover Distance:
                     - Direct zombie hit: full damage & blood VFX
```

## 3. Automated Test Verification
- Tested raycast against military truck with zombie behind it:
  - `preHealth: 100`, `postHealth: 100`, `bulletWasBlocked: true`.
  - Zombie was 100% protected by tactical cover.
