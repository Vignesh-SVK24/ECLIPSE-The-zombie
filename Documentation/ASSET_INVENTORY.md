# ECLIPSE: THE LAST CIVILIZATION
## Asset Inventory & Management Registry

### 1. Classification Definitions
- **[PROD]**: Verified production-ready asset.
- **[PLACEHOLDER]**: Geometric primitive / structural prototype proxy to be replaced in art passes.
- **[PROCEDURAL]**: Code-generated runtime visual / audio element.
- **[PENDING]**: External asset required for future stages.

---

### 2. Character & Locomotion Assets
| Asset Name | Target Path | Classification | Current Status | Notes |
|---|---|---|---|---|
| Ethan Cross Mesh | `Assets/_Project/Models/Characters/` | [PLACEHOLDER] | Capsule / Rigged Proxy | Full custom survivor mesh scheduled for Stage 05/Part 2. |
| Locomotion Animations | `Assets/_Project/Animations/Player/` | [PLACEHOLDER] | Procedural / Base Controller | Configured with Animator parameters for smooth blending. |
| Character Material | `Assets/_Project/Materials/Character/` | [PROD] | URP Lit PBR | Weathered survivor palette (charcoal / olive / leather). |

---

### 3. Environment & Props Assets (Greyfall County)
| Asset Name | Target Path | Classification | Current Status | Notes |
|---|---|---|---|---|
| Road & Sidewalk Kit | `Assets/_Project/Models/Buildings/` | [PLACEHOLDER] | Modular Prefabs | Includes standard mesh colliders for Stage 10. |
| Damaged Storefronts | `Assets/_Project/Models/Buildings/` | [PLACEHOLDER] | Box Prefabs with URP materials | Evacuation signage and boarded doorways. |
| Streetlights & Utility Poles | `Assets/_Project/Models/Props/` | [PLACEHOLDER] | Modular Prefabs | Integrated collision & point-light anchor points. |
| Abandoned Evacuation Vehicle | `Assets/_Project/Models/Vehicles/` | [PLACEHOLDER] | Box Colliders & Hull Proxy | Storytelling prop with collision barrier. |

---

### 4. Audio & VFX Assets
| Asset Name | Target Path | Classification | Current Status | Notes |
|---|---|---|---|---|
| Rain Particle Prefab | `Assets/_Project/VFX/` | [PROD] | Scalable URP Particle System | Configurable emission for Light & Heavy Rain. |
| Wind / Ambient Drone | `Assets/_Project/Audio/Ambient/` | [PENDING] | Audio bus hooks prepared | Royalty-free audio clips to be imported. |
| Footsteps & UI Feedback | `Assets/_Project/Audio/UI/` | [PENDING] | Audio bus hooks prepared | Low-latency audio trigger system ready in Stage 17. |
