# ECLIPSE: THE LAST CIVILIZATION
# ADOBE PHOTOSHOP TO UNITY 6 URP ASSET PIPELINE

**Document Version:** 1.0.0  
**Target:** Universal Render Pipeline (URP 17.0.3), Android, iOS & Desktop  
**Developer:** SVK Creationz  
**Date:** October 2026  

---

## 1. PIPELINE OVERVIEW & ASSET REPOSITORY

This document formalizes the production workflow between Adobe Photoshop and Unity 6 URP for *ECLIPSE: THE LAST CIVILIZATION*. 

All source art and exported runtime textures are strictly categorized into the project directory:

```
Assets/ECLIPSE/
├── Art/
│   └── Photoshop/                 # Master editable .PSD / .PSB source files
│       ├── Characters/
│       │   ├── EthanCross/        # PSDs for Ethan Cross skin, cloth, gear
│       │   ├── Survivors/         # PSDs for Sarah, Marcus, Elena
│       │   └── Zombies/           # PSDs for Walker, Runner, Brute, Alpha
│       ├── Environment/           # Tileable architectural & nature PSDs
│       ├── VFX/                   # Smoke flipbooks, muzzle flashes
│       └── Decals/                # Blood splatters, bullet impacts, warning signs
│
└── Textures/                      # Optimized runtime game textures (.PNG, .TGA)
    ├── Characters/
    │   ├── EthanCross/            # 1024 / 2048 Albedo, Normal, ORM
    │   ├── Survivors/
    │   └── Zombies/
    ├── Environment/               # Tileable concrete, rusted metal, asphalt
    ├── VFX/                       # Particle spritesheets with alpha channels
    └── Decals/                    # Transparent decal projector textures
```

---

## 2. PHOTOSHOP LAYER STRUCTURE STANDARDS

To maintain non-destructive editing across our development team, every `.PSD` file must follow this standardized layer group hierarchy:

```
[Layer Group] EXPORT_ALBEDO (Passthrough / Normal)
├── [Group] FX_WEATHERING (Dirt, blood splatters, mud, dust motes)
├── [Group] DETAILS_SURFACE (Stitching, micro-pores, scars, fabric weave)
├── [Group] BASE_COLORS (Base tones, skin tones, camouflage patterns)
└── [Layer] UV_TEMPLATE (Wireframe overlay - Hidden upon export)

[Layer Group] EXPORT_ROUGHNESS (Grayscale: 0 = Glossy/Smooth, 255 = Rough)
├── [Curves] Contrast adjustment
└── [Group] Material Roughness Assignments (Skin 0.45, Cloth 0.75, Polymer 0.35)

[Layer Group] EXPORT_METALLIC (Grayscale: 0 = Dielectric, 255 = Raw Metal)
└── [Group] Metal Masks (Zippers, buckles, rifle receivers, brass eyelets)

[Layer Group] EXPORT_NORMAL (Tangent Space / Bevel & Emboss micro-normals)
```

---

## 3. COLOR MANAGEMENT & CHANNEL PACKING (URP MASK MAP)

### 3.1 Color Space Integrity
- **BaseColor / Albedo:** Must be painted and exported in **sRGB (IEC 61966-2.1)**.
- **Normal Maps:** Linear color space. Photoshop must **NOT** embed an sRGB profile into normal maps.
- **Mask Maps (ORM):** Linear color space. In Unity, the `sRGB (Color Texture)` checkbox must be **UNCHECKED**.

### 3.2 Channel Packing Scheme (ORM)
In Unity 6 URP Lit shaders, texture sampling bandwidth is cut by 66% on mobile GPUs by combining three grayscale textures into a single RGBA file:

| Channel | Map Content | Photoshop Layer Source | Value Range |
| :--- | :--- | :--- | :--- |
| **Red (R)** | Ambient Occlusion (AO) | Baked AO from 3D model + painted crevice dirt | 0.0 (Occluded) - 1.0 (Open) |
| **Green (G)** | Roughness / Micro-surface | Inverted glossiness layer (skin 0.45, cloth 0.75) | 0.0 (Mirror) - 1.0 (Matte) |
| **Blue (B)** | Metallic Mask | Pure white for metals (buckles, guns), black for cloth/skin | 0.0 (Dielectric) - 1.0 (Pure Metal) |
| **Alpha (A)** | Subsurface Scattering / Detail | Skin translucency mask (ears, nose tip) | 0.0 (None) - 1.0 (Full SSS) |

---

## 4. REALISTIC HUMAN & ZOMBIE TEXTURING RULES

### 4.1 Ethan Cross & Survivors
- **Skin Tone:** Multi-tonal caucasian base (`#DCB08C`) with warm reddish subsurface hues (`#B55B48`) in cheeks, nose, and earlobes.
- **Micro-details:** Subtle non-uniform freckles, faint brow worry lines, and a light 5 o'clock shadow beard stubble (`#5A4436`) with soft alpha feathering.
- **Clothing & Gear:** Weathered olive drab tactical shirt (`#2A382C`) with subtle weave micro-roughness, nylon ripstop grid for ballistic vest, scuffed leather boots.
- **Anti-Uncanny Valley Rule:** Never use flat uniform color fills for human skin; blend at least 3 color variations (epidermal base, vascular red, dermal yellow/blue) using soft noise brushes at low opacity (10-15%).

### 4.2 The Infected (Zombies)
- **Epidermal Decay:** Mottled pallid cyanotic grey-green tone (`#485848` for Walker, `#6E382D` for Runner, `#2B2538` for Stalker).
- **Subdermal Trauma:** Dark necrotic venous networks, bruised contusions (`#2E1824`), and coagulated arterial blood spatters (`#5A1212`).
- **Damaged Fabrics:** Shredded civilian shirt seams, oil and mud stains around cuffs and knees, exposed subcutaneous tissue with glistening high specular roughness (0.15).

---

## 5. UNITY 6 TEXTURE IMPORT PRESETS

| Texture Role | Texture Type | sRGB (Color Texture) | Alpha Source | Compression (Mobile) | Compression (Desktop) |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Albedo / BaseMap** | Default | Checked (True) | None / Transparency | ASTC 6x6 (High Quality) | BC7 |
| **Normal Map** | Normal map | Unchecked (False) | None | ASTC 5x5 | BC5 |
| **Mask Map (ORM)** | Default | **Unchecked (False)** | Input Texture Alpha | ASTC 6x6 | BC7 |
| **Decal / VFX** | Default | Checked (True) | Input Texture Alpha | ASTC 4x4 | BC7 |

---

## 6. AUTOMATION & EXPORT TOOLS

The project includes automated utilities in `Tools/Photoshop/`:
1. `pack_orm_textures.py`: Merges Photoshop-exported `_AO.png`, `_Roughness.png`, and `_Metallic.png` into unified `_ORM.png` assets.
2. `export_psd_to_unity.jsx`: Adobe ExtendScript automation for batch exporting Photoshop layer comps to `Assets/ECLIPSE/Textures/`.
