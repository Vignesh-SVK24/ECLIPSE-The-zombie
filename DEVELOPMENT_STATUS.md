# ECLIPSE: THE LAST CIVILIZATION
## Development Status & Master Milestone Tracker

- **Current Stage:** Environment Overhaul — Batch A Complete (Core Urban Environment, Road PBR, Storytelling Checkpoints & Wrecks)
- **Developer:** SVK Creationz
- **Target Engine:** Unity 6 (6000.0.32f1 LTS)
- **Render Pipeline:** Universal Render Pipeline (URP 17.0.3)
- **Primary Platforms:** Android (Vulkan primary, API 26-34), iOS (Metal primary), PC / Desktop development support
- **Local Playable Client:** Three.js / WebGL Active at `http://localhost:5173`

---

### Progress Summary

- [x] **Part 1: Foundation & Core Configuration**
  - Project architecture established under `Assets/_Project/`.
  - Automated PowerShell backup system (`Tools/Backup/backup_project.ps1`) tested and verified.
  - Package manifest (`Packages/manifest.json`) targeting Unity 6 LTS dependencies (URP, Input System, Cinemachine, Addressables, Netcode).
  - Serialized URP Assets & Project settings (`URP_HighQuality.asset`, `QualitySettings.asset`, `PlayerSettings.asset`).
  - Mobile Quality Presets (Low, Medium, High) with runtime device profile and frame rate regulator.

- [x] **Part 2: Combat, Damage & Zombie AI Systems**
  - Sub-collider hitbox routing with 2.5x headshot critical multiplier (`HitBox.cs`, `HealthComponent.cs`).
  - Modular AI State Machine with 7 behavioral states (Idle, Patrol, Investigate, Chase, Attack, Stunned, Dead).
  - 5 Distinct Infected Zombie Class definitions (Walker, Runner, Stalker, Brute, Alpha Boss).
  - Sensor simulation with obstacle line-of-sight raycasts and sound attenuation hearing.

- [x] **Part 3: Multiplayer Networking Architecture**
  - Unity Netcode for GameObjects (NGO) configuration.
  - Session lifecycle management (`NetworkSessionManager.cs`, `NetworkPlayerSpawner.cs`, `NetworkAuthorityManager.cs`).
  - Resilient reconnection handler and mobile-friendly room lobby UI.

- [x] **Mobile Deployment & On-Device Controls**
  - Dual virtual touch joystick with dynamic center reset and visual drag knob.
  - Touch action buttons (Fire, Aim ADS, Sprint, Reload, Switch Weapon, Camera 1st/3rd person toggle).
  - Safe area inset detection and notch avoidance.
  - Headless Android build automation (`Tools/Build/build_android_apk.ps1`) and Unity build pipeline (`EclipseBuildAutomation.cs`).

- [x] **Ultra Photorealistic Graphics Overhaul — Batch A Complete**
  - **Unity 6 C# Humanoid Architecture:**
    - `HumanoidBoneDef.cs`: Humanoid bone enum hierarchy and anatomical proportion constants (1.83m adult male).
    - `HumanoidRigBuilder.cs`: Constructs 65-bone humanoid skeletal rig with weapon and equipment sockets.
    - `AnatomicalHumanoidGenerator.cs`: Procedural anatomical mesh generator with cranial topology, facial features, muscular torso, tactical clothing, 5-finger hands, combat assault boots, and 4-bone vertex skin weighting.
    - `CharacterPBRMaterialSetup.cs`: High-fidelity PBR material configuration for skin, eyes, hair, tactical clothing, ballistic armor, and gear with texture binding.
    - `CharacterAnimationController.cs`: Procedural breathing, weapon aiming bone tracking, recoil kickback, and hit flinch.
    - `EthanCrossCharacter.cs`: Master coordinator binding mesh, rig, precision hitboxes, and weapon sockets.
    - `PlayerMovementController.cs`, `PlayerCombatController.cs`, `PlayerCameraController.cs`: Production gameplay integration.
    - `PlayerPrefabBuilder.cs`: Automates player hierarchy creation with `CharacterController`, `LODGroup`, and hitboxes.
    - Custom URP Shaders: `CharacterSkinPBR.shader` (subsurface scattering wrap) and `CharacterEyePBR.shader` (cornea refraction).

- [x] **Adobe Photoshop Asset Pipeline Integration**
  - `PHOTOSHOP_ASSET_PIPELINE.md`: Master specification for layer organization, color spaces (sRGB vs Linear), and channel packing.
  - Asset Repository established:
    - `Assets/ECLIPSE/Art/Photoshop/`: Master editable `.PSD` source directory.
    - `Assets/ECLIPSE/Textures/Characters/`: High-resolution Ethan Cross, Survivors, and 5 Zombie class textures.
    - `Assets/ECLIPSE/Textures/Environment/`: Tileable damaged concrete and rusted corrugated metal textures.
    - `Assets/ECLIPSE/Textures/Decals/`: Transparent blood splatters, concrete bullet holes, and biohazard warning signs.
    - `Assets/ECLIPSE/Textures/VFX/`: Muzzle flashes and smoke puffs.
  - Automation scripts:
    - `Tools/Photoshop/pack_orm_textures.py`: Automated ORM mask map packing (AO, Roughness, Metallic, SSS).
    - `Tools/Photoshop/export_psd_to_unity.jsx`: Photoshop ExtendScript batch exporter.
    - `Tools/Photoshop/generate_eclipse_texture_pack.py`: Texture library compiler.

- [x] **Professional Photorealistic Open-World Environment Overhaul — Phase 0 & Batch A Complete**
  - **Phase 0 Audits & Standards:**
    - `ENVIRONMENT_VISUAL_AUDIT.md`: Visual analysis of current environment, identification of missing narrative/visual layers, and aesthetic target definition for Greyfall County (2038).
    - `ENVIRONMENT_ASSET_INVENTORY.md`: Comprehensive breakdown of all environment asset requirements across architecture, roads, vehicles, vegetation, debris, and storytelling props.
    - `ENVIRONMENT_UPGRADE_PLAN.md`: 5-batch phased delivery roadmap establishing design rules and technical constraints.
    - `ENVIRONMENT_PERFORMANCE_BASELINE.md`: Target frame rates (30-60 FPS mobile, 60+ FPS desktop) and strict poly/draw call/texture memory budgets for Android/iOS.
  - **Asset Directory Structure Established:**
    - `Assets/ECLIPSE/Environment/`: Standardized hierarchy covering Architecture, Buildings, Roads, Sidewalks, Vehicles, Props, Debris, Destruction, Nature, Decals, Materials, Textures, Lighting, Weather, VFX, Audio, Prefabs, and Scenes.
  - **Unity 6 C# Environment Scripts (`Assets/_Project/Scripts/Environment/`):**
    - `Eclipse.Environment.asmdef`: Dedicated assembly definition referencing Core, Settings, Damage, and URP runtime.
    - `RoadMaterialController.cs`: Controls layered asphalt PBR, crack intensity, faded markings, and rain puddle thresholds.
    - `ModularBuildingPiece.cs`: Architectural modular elements with facade variations, destruction states, fire escapes, AC units, and LOD group support.
    - `AbandonedVehicleProp.cs`: Vehicle wreck controller handling collision meshes, open doors, flat tires, and `UnityEngine.AI.NavMeshObstacle` carving to ensure zombie AI cleanly pathfinds around obstacles.
    - `EnvironmentalStorytellingScene.cs`: Coordinates evacuation checkpoints, hospital ambulance scenes, and road barricades.
    - `EnvironmentAtmosphereController.cs`: Controls cinematic overcast sunlight, volumetric fog, flickering streetlamps, and weather presets.
  - **Custom URP Environment Shaders:**
    - `Assets/ECLIPSE/Environment/Materials/RoadAsphaltPBR.shader`: Custom URP Lit-compatible road shader with detail normal, crack blend map, and dynamic wet puddle mask with SRP Batcher compatibility.
    - `Assets/ECLIPSE/Environment/Materials/WeatheredBuildingPBR.shader`: Custom URP Lit-compatible building facade shader with vertex-color grime/moss blending.
  - **Local Playable WebGL Client Overhaul (`LocalPlayable/index.html` at `http://localhost:5173`):**
    - Real-time 3D environment overhaul featuring multi-lane cracked asphalt, painted faded lane dividers, crosswalks, raised curbs, and reflective wet puddles.
    - Multi-story brick and concrete commercial facades with glass window frames, awnings, fire escapes, AC units, and rooftop water towers.
    - Abandoned evacuation vehicle fleet: Crashed police cruiser with active flashing red siren beacon, flat front tire, and open door; civilian sedan with roof luggage; heavy military transport truck blocking lane; crashed motorcycle on curb.
    - Military quarantine evacuation checkpoint: Concrete Jersey highway barriers in a chicane, sandbag nests, biohazard warning signs, wooden supply crates, and tripod searchlight.
    - Municipal infrastructure: Curved cobra-head streetlights and red fire hydrants.
    - Overgrown nature: Mature roadside oak trees with tiered canopies, weeds and wild grass clumps bursting through asphalt cracks and curb seams.
    - Preserved explosive barrels and all combat/zombie AI interactions.

---

### Next Development Stage
- **Batch B: Architectural Modular Kit Expansion & Interior Exploration**
  - Implement full interior floor plans for key buildings (pharmacy, police substation, residential apartments).
  - Add procedural modular snapping tool for Unity Editor.
  - Integrate interactive loot containers (cash registers, medicine cabinets, weapon lockboxes).
