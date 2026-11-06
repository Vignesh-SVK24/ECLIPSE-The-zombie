# ECLIPSE: THE LAST CIVILIZATION
# ZOMBIE AI & SENSORY SUBSYSTEM SPECIFICATION
**Developer:** SVK Creationz  
**Target Engine:** Unity 6 (URP 17.0.3)  
**Architecture:** Finite State Machine (FSM) + NavMeshAgent + Sensory Detection Modules  

---

## 1. Sensory Subsystem Architecture

The infected AI operates through modular sensory perception components in `Eclipse.AI`:

```
                    ┌─────────────────────────┐
                    │   Target (Player/Noise) │
                    └───────────┬─────────────┘
                                │
        ┌───────────────────────┴───────────────────────┐
        ▼                                               ▼
┌─────────────────────────┐                 ┌─────────────────────────┐
│     ZombieDetection     │                 │      ZombieHearing      │
│  Vision Cone & Raycasts │                 │ Acoustic Sound Events   │
│  - Vision Range (m)     │                 │ - Gunshot Radius (40m)  │
│  - Field of View (deg)  │                 │ - Footstep Radius (10m) │
└───────────┬─────────────┘                 └───────────┬─────────────┘
            │                                           │
            └───────────────────┬───────────────────────┘
                                ▼
                    ┌─────────────────────────┐
                    │    ZombieController     │
                    │   Finite State Machine  │
                    └───────────┬─────────────┘
                                ▼
            ┌───────────────────┴───────────────────┐
            ▼                                       ▼
┌─────────────────────────┐             ┌─────────────────────────┐
│     ZombieMovement      │             │ ZombieAnimationControl  │
│  NavMeshAgent Pathing   │             │   Mecanim Parameters    │
└─────────────────────────┘             └─────────────────────────┘
```

---

## 2. Class Sensory Parameters Configuration

| Parameter | Walker | Runner | Stalker | Brute |
|---|---|---|---|---|
| **Vision Range** | 16.0 meters | 22.0 meters | 20.0 meters | 15.0 meters |
| **Field of View** | 100 degrees | 120 degrees | 130 degrees | 90 degrees |
| **Hearing Sensitivity**| 1.0x (Standard) | 1.4x (High Alert) | 1.3x (Acute) | 0.8x (Dull) |
| **Investigate Duration**| 5.0 seconds | 4.0 seconds | 8.0 seconds | 6.0 seconds |
| **Stopping Distance** | 1.2 meters | 1.4 meters | 1.3 meters | 1.8 meters |
| **Turn Rate (Angular)** | 180 deg/s | 320 deg/s | 280 deg/s | 120 deg/s |
| **Can Break Barricades**| False | False | False | True (3.5x dmg) |
| **Weak Point Multiplier**| Head: 2.5x | Head: 2.5x / Knee: 1.5x | Spine: 3.0x | Back Gaps: 2.0x |

---

## 3. Class-Specific Tactical Logic

### 3.1 Walker Tactical Logic (Group Swarm)
- Does not flank or seek cover.
- Calculates avoidance vectors with peer Walkers using `ZombieGroupManager` to form wide encircling semi-circles rather than single-file conga lines.
- Slow movement gives players time to acquire headshots or reposition.

### 3.2 Runner Tactical Logic (Agile Interceptor)
- Predicts player movement trajectory by sampling player velocity vector and aiming path 2 meters ahead of player's current position.
- Executes lunges when distance drops below 3.5 meters with line of sight.
- Failsafe: If a lunge misses and impacts a wall or ground, enters `ZombieStunnedState` for 1.0 second.

### 3.3 Stalker Tactical Logic (Shadow Ambush)
- When in `InvestigateState` or `PatrolState`, checks light levels or seeks cover behind the closest high-density occlusion collider.
- Maintains silence until within 4 meters; suppresses groans until attack initiation.
- If player aims crosshair directly at Stalker for > 1.2 seconds before the pounce, Stalker cancels attack, hops backward, and repositions.

### 3.4 Brute Tactical Logic (Juggernaut)
- Pathfinding ignores small destructible props and barricades, applying continuous physics damage.
- When distance to player is 6 to 14 meters in an open corridor, initiates a telegraph roar (`1.2s windup`), followed by an accelerating charge.
- Ground slam inflicts area-of-effect stagger and 25 physical damage to any survivors within 4.0 meters.
