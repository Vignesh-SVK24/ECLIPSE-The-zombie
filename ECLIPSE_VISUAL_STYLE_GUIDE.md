# ECLIPSE: THE LAST CIVILIZATION
## Master Visual Style Guide & Art Bible

**Studio:** SVK Creationz  
**Engine:** Unity 6 + Universal Render Pipeline (URP 17.0.3)  
**Art Direction:** Stylized 3D Animated Cartoon-Realism  
**Master Key Art Reference:** `Assets/ECLIPSE/Art/Concept/ECLIPSE_Master_KeyArt_Stylized.jpg`  
**Document Ref:** `ECLIPSE_VISUAL_STYLE_GUIDE.md`  

---

### 1. The ECLIPSE Visual Identity

The visual identity of *ECLIPSE: THE LAST CIVILIZATION* is defined by **Stylized 3D Animated Cartoon-Realism** — a mature, high-energy aesthetic that bridges the expressive character appeal of modern 3D animated cinematics (*Fortnite*, *Overwatch*, *Arcane*) with the tension, grit, and environmental weight of a post-apocalyptic zombie survival thriller.

```
                            THE VISUAL SPECTRUM
┌─────────────────────────────┬─────────────────────────────────┬───────────────────────────┐
│       FLAT CARTOON          │  ECLIPSE CARTOON-REALISM (OURS) │      HYPER-REALISM        │
│ ─────────────────────────── │ ─────────────────────────────── │ ───────────────────────── │
│ • Untextured cel-shading    │ • Bold, heroic anatomy & silhouettes • Noisy micro-pores    │
│ • No PBR light response     │ • Chiseled, expressive facial features • Muddy desaturated  │
│ • Low-poly simplistic forms │ • Hand-painted textures with PBR lighting • Uncanny valley  │
│ • Childish, casual appeal   │ • Warm, cinematic sunset illumination • Poor mobile render  │
└─────────────────────────────┴─────────────────────────────────┴───────────────────────────┘
```

---

### 2. Master Color Palette Specifications

The color palette is calibrated to create strong visual hierarchy, emotional warmth, and instantaneous gameplay readability.

#### A. Environmental Palette (Warm Sunset & Reclaimed Nature)
| Color Name | Hex Code | Swatch & Production Role |
| :--- | :---: | :--- |
| **Sunset Amber Gold** | `#FFB85C` | Key directional sunlight, rim highlights, lantern flames |
| **Twilight Violet** | `#4A355A` | Sky dome gradient, soft ambient shadows, deep sky haze |
| **Weathered Timber** | `#6D4C3D` | Palisade log walls, wooden barricades, watchtower planks |
| **Reclaimed Overgrowth**| `#4E6A38` | Lush climbing ivy, sidewalk weeds, tree canopies |
| **Rusted Iron** | `#9E3D24` | Abandoned transit bus, corrugated roofing, barbed wire |
| **Cracked Concrete** | `#757B79` | Urban roadway asphalt, curb stones, ruined skyscraper facades |
| **Deep Charcoal** | `#202426` | Asphalt tarmac base, burnt debris, shadow cavities |

#### B. Survivor Roster Palette (Identifiable Hero Accents)
| Character / Role | Primary Signature Color | Accent Color | Materials & Styling |
| :--- | :---: | :---: | :--- |
| **Ethan Cross (Leader)** | **Crimson Red** (`#B72626`) | Khaki Tan (`#C2A676`) | Red hooded jacket/vest, dark armor, olive sleeves, combat boots |
| **Sarah Mitchell (Medic)** | **Mustard Yellow** (`#E6A117`) | Clean White (`#F5F5F5`) | Yellow tactical windbreaker, Red Cross patch, sniper harness |
| **Marcus Vance (Bruiser)** | **Cobalt Blue** (`#2E64A5`) | Charcoal Gray (`#333333`) | Blue work jacket, dark knit beanie, spiked baseball bat |
| **Elena Rostova (Scout)** | **Tactical Olive** (`#556B2F`) | Saddle Leather (`#8B5A2B`) | Tank top, backwards baseball cap, dual drop-leg holsters |
| **Sgt. Jackson (Soldier)** | **Desert Camo** (`#8F8569`) | Gunmetal (`#424242`) | Ballistic helmet, tactical goggles, modular combat webbing |

#### C. Infected Zombie Palette (Signature Feral Threat)
| Anatomy Element | Hex Code | Visual Rule & Shader Behavior |
| :--- | :---: | :--- |
| **Toxic Glowing Eyes** | **`#00FF66`** | **Mandatory Signature Feature.** Uncapped HDR emission, glowing through evening haze and darkness. |
| **Decayed Flesh Base** | `#5A6B54` | Pale olive-gray rotting skin with subtle hand-painted muscle fibers. |
| **Contusion / Bruising** | `#4B2A4A` | Dark purple/magenta bruising around wounds, joints, and veins. |
| **Dried Arterial Blood**| `#681616` | Deep crimson dried blood spattered across mouths, claws, and garments. |

#### D. Tactical UI & Typography Palette
| UI Role | Hex Code | Visual Application |
| :--- | :---: | :--- |
| **ECLIPSE Stencil Red** | `#D32F2F` | Game logo, critical health alert, "SURVIVE TOGETHER" stencils |
| **Survival Amber** | `#FFB300` | Stamina bar, tactical compass, interactive prompts, XP rewards |
| **Tactical Shield Blue** | `#1976D2` | Armor bars, ally status rings, active multiplayer teammates |
| **Gunmetal Carbon** | `#1A2022` | Background panel plates, inventory item card containers |
| **Weathered Bronze** | `#8D6E63` | Outer panel borders, card bevels, socket frames |
| **Optic Off-White** | `#ECEFF1` | Primary typographic text (100% readable on mobile displays) |

---

### 3. Character Design & Silhouette Principles

1. **Bold Silhouettes & Proportions:**
   - Characters possess athletic, mature, rugged builds with slightly broadened shoulders and tapered waists.
   - Distinct headwear and equipment silhouettes (Ethan's red hood and large backpack; Sarah's ponytail and sniper rifle; Marcus's beanie and raised bat).
2. **Expressive Planar Facial Geometry:**
   - Facial features are constructed with clean, readable planes rather than hyper-dense noisy wrinkles.
   - Sharp jawlines, stylized hair clumps with hand-painted highlight streaks, and intense expressive eyes.
3. **Layered Tactical Survival Gear:**
   - Visual clothing communicates survival pragmatism: rolled canvas sleeves, reinforced knee/elbow pads, fingerless gloves, carabiners, radio walkie-talkies, and strapped ammunition pouches.

---

### 4. Zombie Design & Horde Identity

1. **Instant Readability at Range:**
   - The **toxic green eye glow** serves as a vital combat beacon, allowing players to identify threats instantly across dark alleyways or through dense fog.
2. **Aggressive Feral Postures:**
   - Distinct kinematic poses for each class: Walkers stagger with loose shambling strides; Runners sprint in a low predatory crouch; Brutes charge with heavy forward-leaning momentum.
3. **Decaying Hand-Painted Shading:**
   - Shaded via [`StylizedInfectedPBR.shader`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Shaders/Character/StylizedInfectedPBR.shader) with stylized rim lighting and green eye emission.

---

### 5. Environment & Fortification World-Building

1. **The Human Bastion (Survival Together):**
   - Rough-hewn timber logs lashed together into defensive palisades.
   - Coiled rusted concertina barbed wire strung along parapets.
   - Stenciled red graffiti (**"SURVIVE TOGETHER"**, **"SAFE ZONE"**, **"HOPE LIVES"**).
   - Practical survival lighting: Warm brass kerosene hurricane lanterns hanging from fence posts.
2. **The Crumbling Civilization (Greyfall County):**
   - Concrete buildings with weathered brick facades, metal fire escapes, and broken display windows.
   - Abandoned transit buses and emergency vehicles overgrown with lush ivy.
   - Golden hour sunset light piercing through city dust and atmospheric fog.
