# ECLIPSE: THE LAST CIVILIZATION
# GRAPHICS OVERHAUL AUDIT & CURRENT STATE ANALYSIS

**Document Version:** 1.0.0  
**Target:** Unity 6 (6000.0.32f1 LTS) Universal Render Pipeline (URP 17.0.3)  
**Lead Architect:** SVK Creationz Development Team  
**Date:** October 2026  

---

## 1. EXECUTIVE SUMMARY & VISUAL AUDIT FINDINGS

An exhaustive audit of the existing *ECLIPSE: THE LAST CIVILIZATION* visual assets, characters, environments, and shaders was conducted. The audit confirms the critical feedback:
1. **Characters are Primitive Placeholders:** Both in the Unity project structure and the local playable client, characters are represented by basic geometric primitives (cylinders, spheres, and rectangular boxes) with flat color fills.
2. **Missing Anatomical Geometry:** There are no proper human proportions, facial topology (eyes, nose, mouth, ears, jawline), segmented fingers, or organic clothing folds.
3. **Absence of Skinned Mesh Hierarchy:** Characters lack true humanoid bone rigs (`Transform` hierarchy with 55+ standard humanoid bones), skin weights, and `SkinnedMeshRenderer` configurations.
4. **Placeholder Shading:** Materials currently use untextured default `Universal Render Pipeline/Lit` shaders with basic base color and roughness values, lacking PBR normal maps, ambient occlusion maps, metallic maps, roughness maps, and subsurface scattering (SSS) approximations for human skin.
5. **Environment & VFX Status:** Environment assets rely on flat terrain, primitive trees, and basic block buildings. Particle systems are rudimentary and lack realistic smoke dissipation, bullet debris, blood splatters, and cinematic atmospheric fog.

---

## 2. CHARACTER-BY-CHARACTER VISUAL BREAKDOWN

### 2.1 Ethan Cross (Protagonist)
* **Current State:** Single box torso (0.7m x 0.9m x 0.4m), sphere head (radius 0.3m), primitive backpack box. Rigid hierarchy with no bone deformation.
* **Target State:**
  * **Anatomy:** Realistic adult male (38 years old, 1.83m tall, 84kg, athletic survivor build). Correct 7.5 to 8 head-height proportions.
  * **Head & Face:** Anatomical head mesh, realistic brow, distressed eyes with reflective cornea and iris geometry, weathered facial features with survivor beard stubble, scarred left cheek.
  * **Hands & Fingers:** Fully articulated 5-finger hands with distinct knuckles, fingernails, and tactical half-finger gloves.
  * **Clothing & Gear:** Weathered olive-drab tactical tactical jacket, reinforced combat trousers with knee pads, multi-pocket tactical chest rig, holster with sidearm, military combat boots with textured tread.
  * **Skeletal Rig:** Complete 65-bone humanoid skeleton compatible with Unity Humanoid Avatar and standard animation BlendTrees.

### 2.2 Survivor Archetypes
* **Sarah Mitchell (Field Medic):**
  * Practical tactical vest, medical satchel, ponytail hair with physics bone support, utilitarian jumpsuit.
* **Marcus Vance (Weapons Specialist):**
  * Heavy build, plate carrier, tactical sling, muscular forearms with scars, combat radio earpiece.
* **Elena Rostova (Scout / Sniper):**
  * Lean athletic silhouette, hooded recon cape, ghillie accents, fingerless marksman gloves, lightweight boots.
* **Father Gabriel (Camp Leader):**
  * Weathered trench coat, worn clerical collar, bandage on right hand, aged facial wrinkles, expressive eyes.

### 2.3 The Infected (Zombie Classes)
* **Walker (Common Infected):**
  * Decomposed epidermis, torn civilian clothing, exposed tendons, sunken eye sockets, shambling gait posture.
* **Runner (Agile Infected):**
  * Emaciated hyper-athletic build, hyper-extended spine, twitching facial muscles, bloodied fingers with torn nails.
* **Stalker (Stealth Predator):**
  * Low quadrupedal hunch, elongated limbs, blackened veins, stealthy gait, silent hunting posture.
* **Brute (Armored Juggernaut):**
  * Massive hypertrophied musculature (2.3m tall, 180kg), mutated ossified bone plates penetrating skin, torn riot armor remnants.
* **Alpha Boss (Hive Patriarch):**
  * Bioluminescent spore pustules, asymmetric muscular corruption, pulsating vascular network, terrifying facial disfigurement.

---

## 3. RENDERING & SHADING AUDIT

| Component | Current State | Required Overhaul | Mobile Impact |
| :--- | :--- | :--- | :--- |
| **Skin Shading** | Standard Lambert / Flat Lit | Multi-layer PBR with Pre-integrated SSS approximation & micro-roughness | Optimized single-pass LUT on mobile Vulkan/Metal |
| **Eye Shading** | None (Single color sphere) | Dual-layer specular cornea + refracted iris depth + wetness rim | Lightweight mobile specular formula |
| **Hair Shading** | None | Anisotropic Kajiya-Kay specular strand shading or alpha-tested hair cards | 2-pass depth prepass with alpha cutoff (0.5) |
| **Clothing PBR** | Flat diffuse color | Fabric micro-fiber fuzz (rim lighting), roughness/metallic/AO ORM packing | 2K ASTC 6x6 compressed ORM maps |
| **Lighting** | Single directional light | High-fidelity directional Sun with cascaded shadow maps, spherical harmonics ambient, contact shadows | 2-cascade shadows on Mobile High; 1 on Low |
| **Post-Processing** | Basic color tint | Tonemapping (ACES), Bloom with dirty lens, Vignette, Screen-Space Ambient Occlusion (SSAO), Depth of Field | Mobile SSAO with half-resolution downsampling |

---

## 4. GAP ANALYSIS & ACTION PLAN

1. **Phase 1: Architecture & Rigging Engine:** Implement standard humanoid avatar setup, bone mapping, and procedural/imported mesh binding components in `Eclipse.Core` and `Eclipse.Character`.
2. **Phase 2: Ethan Cross Character Assets:** Generate and configure complete anatomical Ethan Cross mesh, humanoid rig, PBR materials, and animation state machine.
3. **Phase 3: Zombie Redesign:** Replace capsule AI targets with anatomically decayed infected meshes, hitboxes attached to specific bones (head, torso, arms, legs), and ragdoll physics.
4. **Phase 4: Cinematic Environment Assets:** PBR architectural kits, realistic post-apocalyptic foliage with wind vertex shaders, and atmospheric particle systems.
5. **Phase 5: Local Playable Upgrade:** Mirror all high-fidelity anatomical models, PBR shading, and animations in the WebGL local playable client.
