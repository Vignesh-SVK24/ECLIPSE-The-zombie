# ECLIPSE: THE LAST CIVILIZATION
## AI Asset Production Pipeline & Generation Standards

**Studio:** SVK Creationz  
**Engine:** Unity 6 + Universal Render Pipeline (URP 17.0.3)  
**Style Reference:** Stylized 3D Animated Cartoon-Realism (`ECLIPSE_Master_KeyArt_Stylized.jpg`)  
**Document Ref:** `ECLIPSE_AI_ASSET_PIPELINE.md`  

---

### 1. Master Generation Pipeline Overview

To prevent visual drift and guarantee that every generated asset feels part of the same game world, all AI asset generation follows a standardized 5-stage pipeline:

```
┌─────────────────┐     ┌──────────────────┐     ┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│ 1. CONCEPT & 2D │ ──> │ 2. 3D GENERATION │ ──> │ 3. BLENDER PASS │ ──> │ 4. UNITY IMPORT │ ──> │ 5. IN-GAME QA   │
│ • Scenario AI   │     │ • Meshy AI       │     │ • Retopology    │     │ • PBR Materials │     │ • Mobile 60 FPS │
│ • Muse Sprite   │     │ • Tripo AI       │     │ • Lightmap UVs  │     │ • Prefab Setup  │     │ • Hitboxes      │
│ • UI Icons      │     │ • Raw FBX/GLTF   │     │ • LOD0/LOD1/LOD2│     │ • NavMesh Carve │     │ • Lighting Check│
└─────────────────┘     └──────────────────┘     └─────────────────┘     └─────────────────┘     └─────────────────┘
```

---

### 2. Standardized Prompt Engineering Templates

Every AI generation prompt must include the **ECLIPSE Core Style Token** to prevent photorealistic noise or flat mobile casual rendering.

#### Core Style Token:
> `"Stylized 3D animated game asset, cartoon-realism, Fortnite and Overwatch inspired, hand-painted PBR texture, bold heroic silhouette, bevelled edges, warm cinematic lighting, mature post-apocalyptic survivor aesthetic, Unreal Engine 5 render style, clean geometry, 8k resolution, trending on ArtStation."`

#### Core Negative Prompt:
> `"Photorealistic, hyperrealistic, noisy micro-texture, live action, flat shaded, low-poly casual, chibi, anime, deformed anatomy, disconnected limbs, blurry, pixelated, ugly, bad topology, non-manifold geometry."`

---

### 3. Asset-Specific Generation Prompts & Workflows

#### A. Survivor Character Turnarounds (Scenario AI)
- **Prompt (Ethan Cross):**
  > `"Full body character concept sheet, front view, side view, back view, Ethan Cross, rugged male survivor, 32 years old, athletic muscular build, short brown hair and trimmed beard, wearing crimson red hooded tactical jacket over dark Kevlar chest rig, rolled olive sleeves, fingerless gloves, charcoal cargo pants with knee pads, heavy survival backpack with bedroll, holding customized assault rifle, stylized 3D animated hero, clean white background."`
- **Output:** 2048x2048 PNG turnaround sheet saved to `Assets/ECLIPSE/Art/Characters/Concepts/`.

#### B. Infected Zombie Variations (Scenario / Tripo AI)
- **Prompt (Walker Zombie):**
  > `"Full body character concept, infected zombie walker, male civilian, decaying pale olive-green skin, torn flannel shirt and shredded denim jeans, sunken feral snarl, intense glowing toxic emerald green eyes, menacing hunched stance, stylized 3D animated cartoon-realism, hand-painted texture details, clean background."`
- **Key Requirement:** Must feature high-emission toxic green eyes (`#00FF66`).

#### C. Defensive Fortifications & Settlement Props (Meshy AI)
- **Prompt (Palisade Barricade with Stencil):**
  > `"Heavy timber palisade barricade, thick vertical wooden logs lashed with weathered steel wire, rusted barbed wire strung across top, red stenciled spray paint lettering saying 'SURVIVE TOGETHER', stacked sandbags on base, stylized 3D game asset, hand-painted wood grain texture, beveled edges, game ready low-poly PBR model."`
- **Output:** Raw FBX with Albedo, Normal, Roughness, Metallic.

#### D. Survival UI Item Icons (Unity Muse Sprite / Scenario)
- **Prompt (Military First Aid Kit):**
  > `"Stylized 2D game icon, military emergency medical kit, olive drab metal box with bold red cross emblem, latches, bandages and adrenaline syringe visible, hand-painted digital art, bold outlines, warm lighting, centered on dark transparent background."`
- **Output:** 512x512 RGBA PNG with alpha transparency saved to `Assets/ECLIPSE/Art/UI/Icons/`.

---

### 4. Technical Acceptance Criteria & Quality Gates

Before an asset can be committed to the project repository:

1. **Polygon Budget Compliance:**
   - Hero Character (Ethan Cross): Max `25,000` triangles (LOD0), `12,000` (LOD1), `4,000` (LOD2).
   - Standard Zombie (Walker/Runner): Max `12,000` triangles (LOD0), `5,000` (LOD1), `1,500` (LOD2).
   - Environment Prop (Barricade/Vehicle): Max `4,000` triangles (LOD0), `1,800` (LOD1), `500` (LOD2).
   - Small Prop (Ammo box/Lantern): Max `600` triangles (LOD0).
2. **Texture Memory Budget:**
   - Characters: 1x 2048x2048 Albedo, Normal, MaskMap (ASTC 6x6 on Mobile).
   - Environment Props: 1024x1024 or 512x512 atlased PBR sets.
   - UI Icons: 256x256 or 512x512 sprite atlases.
3. **Rigging & Sockets:**
   - Humanoid avatar compatible.
   - Right hand socket (`Weapon_R`) correctly oriented for firearm grips.
4. **Collision & NavMesh:**
   - Static barricades must have compound Box Colliders and `NavMeshObstacle` carving.
