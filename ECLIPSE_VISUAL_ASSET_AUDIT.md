# ECLIPSE: THE LAST CIVILIZATION
## Phase 1 — Comprehensive Visual Asset & Material Audit

**Studio:** SVK Creationz  
**Engine:** Unity 6 + Universal Render Pipeline (URP 17.0.3)  
**Target Visual Benchmark:** Master Concept Key Art (`Assets/ECLIPSE/Art/Concept/ECLIPSE_Master_KeyArt_Stylized.jpg`)  
**Document Ref:** `ECLIPSE_VISUAL_ASSET_AUDIT.md`  

---

### 1. Executive Summary & Aesthetic Gap Analysis

The visual presentation of *ECLIPSE: THE LAST CIVILIZATION* was previously oriented toward realistic/gritty survival rendering. Following the approval of the **Master Stylized Concept Key Art**, the entire game is transitioning to **Stylized 3D Animated Cartoon-Realism** (akin to *Fortnite*, *Overwatch*, and stylized AAA cinematic animation).

This audit identifies the current asset state across all visual domains, highlights stylistic gaps, and defines the transformation requirements.

---

### 2. Character Asset Audit: Ethan Cross & Survivors

```
CURRENT PROTOTYPE STATE               TARGET STYLIZED ART DIRECTION
────────────────────────────         ────────────────────────────────────────
• Gritty, desaturated textures       • Expressive, bold hero silhouette
• Ultra-dense realistic micro-detail  • Clean read, chiseled planar facial features
• Flat civilian clothing             • Vivid crimson hooded jacket, tactical vest
• Generic combat harness             • Heavy survival backpack with rolled sleeping pad
• Realistic proportion rig           • Heroic proportions, muscular forearms, knee pads
```

#### Stylistic Gap & Transformation Requirements:
1. **Ethan Cross (Protagonist):**
   - **Head & Face:** Sharp angular jawline, stylized chiseled beard, dark expressive eyebrows, intense determined gaze.
   - **Upper Body:** Red hooded jacket/vest (`#B72626` base) over dark tactical body armor and rolled-up khaki/olive sleeves. Fingerless combat gloves and tactical wrist watch.
   - **Backpack:** Heavy modular canvas survival rucksack with radio antenna, carabiners, bedroll, and canteen.
   - **Lower Body:** Rugged charcoal tactical cargo pants with reinforced ballistic knee guards and sturdy leather combat assault boots.
2. **Survivor Roster (Batch C Integration):**
   - **Sarah Mitchell (Medic):** Mustard-yellow hooded tactical windbreaker with white medical cross patch, ponytail, tactical sniper scope harness.
   - **Marcus Vance (Bruiser):** Cobalt blue hooded work jacket, knit beanie, muscular build, spiked heavy baseball bat.
   - **Elena Rostova (Scout):** Olive tank top, backwards tactical cap, dual drop-leg pistol holsters.
   - **Sergeant Jackson (Soldier):** Full ballistic helmet, tactical goggles, desert camo combat harness, assault rifle.

---

### 3. Infected Zombie Asset Audit

The current prototype features 5 distinct functional zombie classes (`Walker`, `Runner`, `Stalker`, `Brute`, `Alpha Boss`), but their visual identity lacks the iconic stylized punch demonstrated in the master key art.

| Zombie Class | Current State | Target Stylized Visual Identity | Iconic Feature |
| :--- | :--- | :--- | :--- |
| **Walker** | Standard decaying humanoid | Tattered civilian clothes, pale olive-gray skin, bruised joints | **Glowing toxic green eyes**, slack-jawed feral snarl |
| **Runner** | Agile lean infected | Emaciated torso, shredded athletic wear, hyper-extended neck | Glowing green eyes, predatory hunched forward sprint |
| **Stalker** | Shadowy infiltrator | Dark mottled fungal growths, elongated claw-like fingers | Subdued sickly eye glow, silent stalking crouch |
| **Brute** | Heavy infected | Massive hyper-developed muscular torso, burst shirt seams | Heavy calloused stone-like skin plates, cracked skull |
| **Alpha Boss** | Mutation leader | Towering mutated mass, protruding bone spikes, bioluminescent pustules | Intense pulsating emerald eye glare, ground-pound shockwaves |

> [!IMPORTANT]
> The **toxic green eye glow** is the unifying signature visual marker across all zombie classes in the master artwork. It must be implemented via emission channels in [`StylizedInfectedPBR.shader`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Shaders/Character/StylizedInfectedPBR.shader) to ensure instant player target identification at distance and in low light.

---

### 4. Environment Asset Audit: Greyfall City & Fortifications

#### Current Assets vs Master Artwork Requirements:

1. **Defensive Fortifications:**
   - *Current:* Concrete highway Jersey barriers.
   - *Target:* Heavy rough-hewn timber palisade walls, wooden stakes, rusted concertina wire coils, and red-stenciled **"SURVIVE TOGETHER"** plank signage.
2. **Fortified Barrels & Sandbags:**
   - Sandbags stacked on top of painted red industrial fuel drums and steel oil barrels.
   - Sturdy olive-drab military ammunition cans with yellow stenciled caliber markings.
   - Brass/iron kerosene hurricane lantern emitting a warm amber point-light glow (`#FFAA44`).
3. **Macro Architecture & Storytelling Landmarks:**
   - Elevated timber watchtower flying a red survivor clan banner.
   - Municipal water tower atop steel lattice legs with hand-painted **"HOPE LIVES"** slogan.
   - Yellow weathered highway sign stenciled with **"SAFE ZONE"** and survivor stick figures.
   - Overturned rusted transit bus overgrown with lush green ivy creepers.
   - Distant skyline of ruined skyscraper silhouettes against a golden hour sunset sky.

---

### 5. Lighting & Atmosphere Audit

- **Current:** Cold overcast directional lighting with neutral gray ambient sky.
- **Target (Master Key Art Benchmark):**
  - **Skybox & Sun:** Dramatic golden hour sunset with warm orange/amber horizon gradient blending into soft magenta/violet cloud tops.
  - **Directional Sunlight:** Warm amber-gold sun (`#FFB85C`) casting long, dramatic, cinematic shadows across the barricades.
  - **Atmospheric Fill:** Soft cool violet/blue ambient sky fill to create complementary color contrast between warm sunlit surfaces and cool shaded areas.
  - **Volumetric Fog:** Low-density golden haze catching light shafts filtering between buildings and wooden palisades.
