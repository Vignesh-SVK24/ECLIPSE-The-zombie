# ECLIPSE: THE LAST CIVILIZATION
## VISUAL AUDIT & GRAPHICAL DEFICIT REPORT

### 1. Executive Summary & Audit Context
- **Project:** ECLIPSE: THE LAST CIVILIZATION
- **Audit Date:** November 6, 2026
- **Reference Standard:** *Left to Survive* (gritty zombie survival shooter aesthetics, tactical gear, infected variation, weapon realism) & *The Last of Us* (grounded post-apocalyptic environment materials, atmospheric depth, cinematic lighting).
- **Core Problem Identified:** Previous iterations relied on flat-color `MeshStandardMaterial` parameters without procedural surface texture maps, lacked ACES filmic tonemapping, lacked rim lighting for character silhouette definition, used primitive box geometries for complex tactical gear, and featured homogeneous zombie meshes with basic color tints.

---

### 2. Comprehensive Visual Component Audit

| Subsystem | Baseline State (Deficit) | Target Standard (*Left to Survive* Inspired) | Action Required |
|---|---|---|---|
| **Tonemapping & Color Grading** | Linear clamp (no tonemapping). Colors blow out in highlights; shadows lack rich contrast. | `ACESFilmicToneMapping` with exposure 1.05 and calibrated filmic curve. | Configure WebGL renderer ACES tonemapping and filmic exposure. |
| **Scene Lighting** | Single hemisphere + single directional light without rim or ambient fill. | 3-point cinematic lighting: warm sunset key light (`0xff9e54`), cool ambient sky dome (`0x3d2840`), and sharp blue-gray rim light (`0x5c7a94`). | Add directional rim backlight and secondary bounce lighting. |
| **Atmospheric Depth** | Static exponential fog. No dynamic environmental particulate matter. | Multi-layered atmospheric fog + dynamic floating dust motes and airborne ash particles drifting across light beams. | Implement GPU particle system for atmospheric dust motes. |
| **Material Textures** | 100% flat untextured PBR materials (`color`, `roughness`). Surfaces look synthetic and plastic-like. | Procedural canvas-generated PBR texture maps: micro-roughness noise, fabric weave, pore details, asphalt grit, rust oxidization, and concrete cracks. | Build procedural texture generator engine for skin, cloth, metal, concrete, and asphalt. |
| **Ethan Cross (Protagonist)** | Primitive geometric block assembly; flat crimson hoodie; missing facial structure, tactical radio cord, and weathered gear. | Gritty survivor model: anatomical head with cheekbone structure, textured beard stubble, brow ridge, tactical ballistic vest with MOLLE webbing, shoulder radio with coiled cord, drop-leg holster with straps, textured cargo trousers with dirt stains, and heavy rucksack with canteen. | Completely overhaul Ethan Cross 3D geometry and procedural PBR materials. |
| **Zombie Infected (All 4 Classes)** | Generic cylinder/box limbs with solid green/purple/brown color fills; lack visceral horror details. | Visually distinct horror classes: **Walker** (fractured ribs, asymmetrical rotting jaw, torn civilian cloth fringes, clouded cataract eye); **Runner** (wiry sinewy anatomy, frantic reaching posture, torn athletic gear, bloodied jaw); **Stalker** (emaciated dark silhouette, crouching stance, necrotic skin lesions); **Brute** (massive mutated torso, swollen asymmetrical arm, calcified bone spikes, torn industrial jumpsuit). | Build bespoke geometries, bone fragments, and infection vein textures for all 4 zombie types. |
| **Weapons & Ballistics** | Basic geometric blocks without ejection ports, rails, or textured grips. | Semi-realistic firearms: Two-tone 9mm Tactical Pistol with slide serrations and laser module; M4 Carbine with telescoping stock, quad Picatinny rail, and holographic sight. | Enhance weapon geometry, metallic reflections, and holographic reticle. |
| **Combat VFX** | Single red point light flash with no particle dissipation. | Multi-stage gunfire VFX: conical 4-point muzzle flash, rising barrel smoke, dynamic illumination, and material-specific impact sparks/blood. | Build particle-driven muzzle flash, barrel smoke, and impact bursts. |

---

### 3. Transformation Strategy & Roadmap
- **Phase 1 (Visual Foundation):** Implement ACES filmic tonemapping, 3-point cinematic lighting (key, ambient, rim), atmospheric dust particle simulation, and procedural canvas PBR texture synthesis.
- **Phase 2 (Characters & Zombies):** Complete overhaul of Ethan Cross anatomical model and all 4 infected classes (Walker, Runner, Stalker, Brute) with procedural skin and clothing textures.
- **Phase 3 (Environment & Greyfall City):** Asphalt road cracking, weathered building facades, concrete barrier decals, and police car lighting.
- **Phase 4 (Weapons & Combat VFX):** Tactical firearm modeling, muzzle flashes, barrel smoke, and blood impact particles.
- **Phase 5 (Mobile Optimization & Validation):** Headless Chromium CDP testing, frame rate verification, and GitHub Pages live deployment.
