# ECLIPSE: THE LAST CIVILIZATION
## Part 2 — Master Development Plan (Stages 01 – 25)
**Developer:** SVK Creationz  
**Target Engine:** Unity 6 (6000.0.x LTS) | Universal Render Pipeline (URP)  
**Primary Platforms:** Android (Vulkan), iOS (Metal)

---

### Part 2 Scope Overview
Part 2 expands the foundational architecture with full survival-horror mechanics: intelligent zombie AI (5 distinct infected types), a multi-weapon combat system with FP/TP aiming and mobile firing controls, damage/health mechanics, survival needs (hunger, thirst, stamina, injuries), inventory and crafting, narrative story missions, world exploration, survivor recruitment, base/settlement building, and vehicle mechanics.

---

### Implementation Roadmap

| Stage | Title | Core Deliverables | Status |
|---|---|---|---|
| **01** | **Project Inspection & Part 1 Integration** | Audit Part 1 assets, establish assembly references, create pre-Part 2 backups, map dependency tree. | **COMPLETE** |
| **02** | **Common Damage & Health Architecture** | `IDamageable`, `DamageInfo`, `DamageType`, `DamageSystem`, `HealthComponent`, `HitBox`, `HitReactionController`, `DestructibleObject`. | **COMPLETE** |
| **03** | **Zombie AI Foundation & State Machine** | Modular AI state machine (`ZombieStateMachine`, `ZombieController`), NavMesh navigation, boundary clamps. | Pending |
| **04** | **Five Infected Enemy Types** | Walker, Runner, Stalker, Brute, Alpha (boss with phase mechanics) data assets and specialized controllers. | Pending |
| **05** | **Zombie Perception & Sound Detection** | Vision cone, sound event listener (gunshots, footsteps, vehicle engines), horde grouping logic. | Pending |
| **06** | **Zombie Animations & Hit Reactions** | Blend trees, hit staggers, directional impact reactions, death states, Ragdoll/cleanup triggers. | Pending |
| **07** | **Weapon Data & Handling** | ScriptableObject `WeaponData` (Pistols, Rifles, Shotguns, Snipers, SMGs, Melee), ammo types, attachments. | Pending |
| **08** | **Shooting, Aiming, Recoil & Reload** | Hitscan & ballistic firing, FP & TP camera aim alignment, procedural recoil, safe reload cycles. | Pending |
| **09** | **Melee Combat & Weapon Switching** | Light/heavy melee swings, stamina consumption, attack sweeps, multi-slot equipment cycle. | Pending |
| **10** | **Mobile Combat Controls** | Contextual fire/aim buttons, reload gesture, weapon wheel, multitouch priority routing. | Pending |
| **11** | **Health, Stamina, Hunger, Thirst & Injuries** | Survival manager, resource depletion rates, injury status effects (bleeding, limp), medical consumables. | Pending |
| **12** | **Inventory & Equipment** | Grid inventory, item stacking, weight/capacity limits, equipment slots, touch drag-and-drop UI. | Pending |
| **13** | **Loot & Resource Gathering** | Loot tables, container interactions (backpacks, lockers, car trunks), item spawner pools. | Pending |
| **14** | **Crafting System** | Recipe registry, material validation, workbench stations, medical & ammo crafting. | Pending |
| **15** | **Mission & Dialogue Systems** | MissionManager, objective tracking, branching dialogue UI with choices and sound cues. | Pending |
| **16** | **Three Initial Story Missions** | Mission 01 (The Last Signal), Mission 02 (Silent Streets), Mission 03 (The Hidden Truth). | Pending |
| **17** | **World Exploration & Dynamic Events** | Discovered POIs, emergency radio calls, supply drops, trapped civilian encounters. | Pending |
| **18** | **Settlement & Building Upgrades** | Greyfall Safehouse, Command Centre, Workshop, Medical Centre, Generator, defense barricades. | Pending |
| **19** | **Survivor Recruitment & Assignments** | Survivor profiles (Medic, Engineer, Hunter, Soldier), role tasks, lightweight mobile simulation. | Pending |
| **20** | **Vehicle System** | Pickup truck & Van physics, entry/exit, steering, fuel consumption, engine noise attractant. | Pending |
| **21** | **HUD & UI Integration** | Comprehensive mobile survival HUD, compass markers, weapon status, quick consumable slots. | Pending |
| **22** | **Save System Expansion** | Serialized save slots capturing player stats, inventory, settlement level, vehicles, and missions. | Pending |
| **23** | **Multiplayer Preparation** | Client-server decoupling, state validation interfaces, network-ready authority hooks. | Pending |
| **24** | **Mobile Optimization** | AI LODs, staggered perception checks, combat VFX object pools, draw call reduction. | Pending |
| **25** | **Testing, Debugging & Final Validation** | Full acceptance testing against Section 25 criteria across Android & iOS target profiles. | Pending |
