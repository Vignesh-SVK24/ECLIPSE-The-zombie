# ECLIPSE: THE LAST CIVILIZATION
## MASTER ART DIRECTION SPECIFICATION — CINEMATIC SEMI-REALISTIC ZOMBIE SURVIVAL

**Document Version:** 2.0.0-PRO  
**Author:** Antigravity Senior AAA Art Director & Graphics Engineer  
**Target Engine:** Unity 6 (URP 17.0.3) & WebGL 3D Playable Client  
**Genre Inspiration:** High-end mobile cinematic zombie survival shooters (e.g. *Left to Survive* visual quality & presentation)  
**Status:** Approved for Batch A Prototype Implementation  

---

### 1. CORE VISUAL PHILOSOPHY

ECLIPSE: THE LAST CIVILIZATION transitions completely away from stylized cartoon aesthetics (oversized heads, saturated plastic shaders, hyper-exaggerated proportions, flat painted textures) into a **gritty, cinematic, semi-realistic 3D survival horror universe**.

Every visual component reinforces the narrative of an unforgiving, decayed post-outbreak world:
1. **Grounded Human Anatomy:** Survivors exhibit authentic 1:7.5 to 1:8 head-to-body ratios, weathered faces with realistic skin pore structure, micro-scars, dirt, sweat, and functional field gear.
2. **Believable Infected Humanity:** Zombies are former humans corrupted by the fungal-biological Eclipse pathogen—not fantasy or cartoon monsters. They feature pale gray-green mottled skin, visible purple/black thrombosed veins, exposed bone and cartilage, broken teeth, torn utilitarian clothing, and faint, internal bioluminescent ocular mutations.
3. **PBR Surface Fidelity:** Surfaces rely on realistic roughness, micro-scratches, metal oxidation, edge wear, and wet asphalt fresnel highlights.
4. **Cinematic Atmospheric Lighting:** Naturalistic environmental lighting (overcast daylight, dusk sunset, deep industrial night) with volumetric fog scattering, controlled bloom, chromatic aberration, and filmic ACES tone mapping.
5. **Tactical Survival UI/UX:** A sleek, military-grade interface utilizing gunmetal gray, charcoal slate, olive drab, and hazard amber accents with crisp typography, high readability, and responsive hardware safe-area scaling.

---

### 2. MASTER COLOR PALETTE MATRIX

| Category | Primary Color Name | Hex Code | Roughness / Specular Role | Applied Surfaces |
|:---|:---|:---|:---|:---|
| **Environment** | Concrete Gray | `#4A4E51` | Rough (0.75) | Cracked roadways, building facades, barriers |
| **Environment** | Dusty Beige / Sand | `#8C7D6B` | Rough (0.85) | Road dust, debris piles, dried earth |
| **Environment** | Weathered Rust | `#7A3E26` | Matte/Pitted (0.80) | Rusted shipping containers, corrugated steel, guardrails |
| **Environment** | Wet Asphalt Black | `#1F2224` | Smooth (0.25) + Fresnel | Road surfaces, puddles, rainy gutters |
| **Environment** | Overgrown Olive | `#3B4233` | Subsurface (0.60) | Street weeds, climbing vines, decayed grass |
| **Characters** | Desaturated Field Red | `#752522` | Matte Canvas (0.65) | Ethan Cross weathered tactical canvas jacket |
| **Characters** | Tactical Slate / Charcoal | `#2B2F33` | Semi-Rough (0.50) | Cargo pants, tactical vest plates, rucksack straps |
| **Characters** | Weathered Saddle Leather | `#4A3525` | Worn Leather (0.42) | Combat boots, gun holster, utility belt, magazine pouches |
| **Characters** | Survivor Skin Tone | `#BFA08A` | Subsurface (0.48) | Ethan Cross face, hands, neck (subtle SSS scatter) |
| **Infected** | Decaying Gray-Green | `#566152` | Mottled Slime (0.40) | Walker / Runner infected skin surface |
| **Infected** | Thrombosed Vein Purple | `#342238` | Deep Sub-dermal (0.60) | Infection vascular network branching from wounds |
| **Infected** | Faint Toxic Eye Glow | `#45D422` | Emissive (Intensity 1.8) | Internal mutated pupil glow (subtle, non-LED) |
| **UI System** | Gunmetal Background | `#12171A` | 85% Alpha Overlay | Menu panels, HUD containers, modal backdrops |
| **UI System** | Tactical Olive Accent | `#4B5E43` | Solid / Border (1px) | Active tabs, inventory slots, health bar |
| **UI System** | Hazard Amber Highlight | `#D4A034` | Text & Accent Glow | Critical alerts, selected buttons, ammo numbers |
| **UI System** | Warning Crimson | `#C23828` | Emergency Glow | Danger state, threat alerts, low health flash |

---

### 3. CHARACTER & HERO REDESIGN (ETHAN CROSS)

- **Facial Features:** 34-year-old hardened combat engineer and survivor. Realistic brow line, determined jawline with short dark stubble, faint scar across the left cheekbone, realistic irises with natural wet reflections.
- **Silhouette & Attire:** 
  - Outer Layer: Weathered dark crimson tactical jacket with rolled cuffs and reinforced shoulder stitching (dirt and oil stains on elbows).
  - Torso: Concealed level-IIIA ballistic chest carrier in matte charcoal cordura with molle webbing.
  - Legs: Reinforced olive/charcoal ripstop cargo trousers with knee articulation seams.
  - Footwear: Heavy-duty combat boots with mud accumulation on soles and scuffed leather caps.
  - Equipment: Leather drop-leg pistol holster, double magazine pouches on utility belt, survival radio on chest rig, modular field rucksack with bedroll strapped to base.
- **Weapon Handling:** Firm, professional two-handed grip on assault rifles and pistols; realistic recoil recovery and dynamic shoulder sway when aiming down sights (ADS).

---

### 4. INFECTION DESIGN LANGUAGE (THE FOUR CLASSES)

1. **Walker (Primary Prototype Class):**
   - Former civilian / security guard in torn apparel (ragged denim, torn button-down or security polo).
   - Sluggish, off-balance gait due to necrotized leg muscle tissue.
   - Jaw partially damaged with broken, discolored teeth and dark dried fluids around mouth.
   - Right eye mutated with faint toxic luminescence; left eye clouded with cataract opacity.
2. **Runner:**
   - Athletic anatomy, shredded running apparel, frantic hyperventilating chest spasms, aggressive lunges.
3. **Stalker:**
   - Emaciated silhouette, silent low-crouched stalking posture, pale translucent skin revealing veins.
4. **Brute:**
   - Massive mutated musculature, torn industrial heavy work clothes, thick callous skin plates, heavy stomping charge.

---

### 5. CINEMATIC GREYFALL CITY SCENE PRESENTATION

- **Atmosphere:** Evacuation route trapped at twilight. Long sunset shadows cast through towering ruins.
- **Volumetric Fog:** Light street mist settling over the asphalt, reflecting the amber warm glow of burning barrels and distant emergency strobes.
- **Environmental Storytelling:**
  - Abandoned passenger bus blocking the cross-street with shattered safety glass.
  - Overturned police interceptor with dim intermittent red/blue roof flasher.
  - Military concrete barricades with razor wire coils and painted survival graffiti: *"SURVIVE TOGETHER"*, *"NO ENTRY - QUARANTINE"*.
  - Realistic puddles on asphalt reflecting sky light with realistic specular highlights.

---

### 6. PRESENTATION & MAIN MENU DESIGN (LEFT TO SURVIVE INSPIRATION)

- **Cinematic 3D Menu Scene:** Ethan Cross stands vigilant in a ruined street outpost holding his assault rifle, while background smoke drifts past a barricaded checkpoint.
- **Left-Side Navigation:** Sleek vertical navigation bar:
  - `[CONTINUE]` — Jump into active checkpoint.
  - `[NEW GAME]` — Begin the Greyfall City evacuation operation.
  - `[MISSIONS]` — Tactical sector map (Greyfall City, Blackwood Forest, Ironshield Base, Ashen District, Research Complex).
  - `[CHARACTERS]` — Inspect survivor roster, gear, and combat perks.
  - `[WEAPONS]` — 3D armory inspection, attachments, and stats.
  - `[INVENTORY]` — Grid-based survival gear, rations, meds, and crafting components.
  - `[SETTLEMENT]` — Base management: defensive barricades, radio tower, medical bay, workshop, generators.
  - `[SETTINGS]` — Graphics profiles (Low, Medium, High), audio, sensitivity, and platform controls.
- **Audio Atmosphere:** Low ominous cello drone layered with distant wind, crackling fire, and muffled thunder.
