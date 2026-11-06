# ECLIPSE: THE LAST CIVILIZATION
## VISUAL TRANSFORMATION AUDIT & ASSET DEPRECATION MATRIX

**Audit Date:** 2026-11-06  
**Auditor:** Technical Art & Graphics Engineering Team  
**Scope:** Complete Unity 6 Project & WebGL Playable Client  

---

### 1. EXECUTIVE AUDIT SUMMARY

The previous iteration of *ECLIPSE: THE LAST CIVILIZATION* was structured around a stylized 3D animated cartoon-realism aesthetic. While structurally sound and responsive, several components featured high-saturation colors, simplified cartoon proportions, exaggerated silhouettes, and bright glow effects that contradict the new cinematic survival-shooter direction inspired by *Left to Survive*.

This audit catalogs every visual asset, shader, material, and UI element requiring transformation, defines the deprecation strategy, and ensures zero breakage of gameplay, combat, and networking logic.

---

### 2. DETAILED ASSET AUDIT & TRANSFORMATION ROADMAP

| Asset Subsystem | Current Stylized State | Required Gritty Semi-Realistic State | Action / Deprecation Plan |
|:---|:---|:---|:---|
| **Ethan Cross Model** | Bright scarlet hoodie, oversized hands, stylized hair geometry, smooth skin shader. | Mature adult male, natural 1:7.8 proportions, weathered dark red tactical canvas jacket, tactical chest rig, cargo pants, combat boots, facial stubble, realistic skin SSS. | Replace procedural geometry & shader values with anatomical proportions and realistic PBR parameters. |
| **Zombie Walkers** | Cartoonish exaggerated mouths, saturated lime-green skin, bright glowing LED eyes. | Gritty decaying infected human, pale gray-green mottled flesh, dark purple thrombosed veins, broken teeth, torn dirty clothing, subtle internal ocular glow. | Reconfigure material properties, texture albedo maps, and eye emission intensity (reduce from 4.5 to 1.8). |
| **Zombie Runners / Brutes** | Oversized cartoon muscles, bright neon skin patches. | Sinewy athletic anatomy (Runner) and realistic hyper-mutated bulk (Brute) with damaged skin plates, exposed tendons, and realistic blood staining. | Schedule for Batch B transformation. |
| **Weapons (9mm / M4)** | Stylized blocky silhouettes, high saturation metals. | Realistic milled steel, polymer handguards, scratched receivers, dirt in crevices, realistic muzzle brake details. | Refactor 3D weapon meshes with realistic proportions and tactical matte gunmetal finish. |
| **Greyfall City Road** | Flat dark plane with cartoon stripe markings. | Wet cracked asphalt with realistic normal maps, potholes, weathered yellow centerlines, realistic puddles with fresnel reflections. | Completed in Batch B shader setup; upgrade procedural puddle and curb details in approval prototype. |
| **Lighting & Sky** | Bright golden hour saturated cartoon sun. | Filmic overcast daylight or moody sunset with desaturated shadows, natural volumetric fog, realistic directional shadows. | Update URP lighting profiles, adjust Directional Light color from warm yellow to cinematic dusk cool-warm balance. |
| **In-Game HUD** | Bright gold cartoon border frames, oversized badges. | Sleek tactical military HUD: gunmetal gray translucent panels (`rgba(18, 23, 26, 0.88)`), hazard amber gauges, crisp sans-serif typography, clean ammo readout. | Overhaul HUD styling in approval prototype. |
| **Main Menu** | Direct load into gameplay with overlay prompt. | Full cinematic 3D Main Menu: 3D survivor character showcase, animated backdrop, Left to Survive-style tactical navigation bar, modals for Armory, Inventory, Base. | Implement in Approval Prototype. |

---

### 3. GAMEPLAY CODEBASE INTEGRITY VERIFICATION

To ensure absolute stability, all transformations are purely visual and presentation-layer:
- **PlayerMovement.cs / player controller:** Movement speeds, stamina drain rates, sprint speeds, and collision bounds remain 100% identical.
- **Weapon System:** Damage per bullet (24 for 9mm, 42 for M4), clip sizes (14 and 30), reload timings (1.8s and 2.4s), and automatic fire rates are preserved without alteration.
- **Zombie AI System:** Detection ranges, state machine transitions (Idle -> Patrol -> Investigate -> Chase -> Attack -> Stun -> Death), hitboxes, and health values remain strictly intact.
- **Multiplayer / Networking:** Authority validation, snapshot serialization, and player sync hooks are untouched.

---

### 4. BATCH A APPROVAL PROTOTYPE SCOPE

The Batch A Approval Prototype isolates the core visual pillar changes into a single, cohesive, reviewable experience:
1. **Hero Character:** Ethan Cross redesigned in full cinematic semi-realistic attire.
2. **Infected Enemy:** Walker Zombie redesigned with decaying flesh, visible veins, and torn civilian clothing.
3. **Environment:** Greyfall City evacuation street with realistic asphalt, barricades, emergency vehicle, street debris, and atmospheric mist.
4. **User Interface:** Tactical AAA Main Menu with 3D character preview and tactical HUD.
