# ECLIPSE: THE LAST CIVILIZATION
# ENVIRONMENT ART DIRECTION & CINEMATIC WORLD DESIGN

**Lead Environment Artist:** SVK Creationz  
**Target Biomes:** Greyfall County, Pacific Northwest Post-Apocalyptic Atmosphere  
**Visual Influences:** Realistic ruin decay, overgrown concrete, overcast Pacific skies, oppressive mist, decaying pine forests  

---

## 1. WORLD SECTORS & PALETTE SPECIFICATION

```
Sector 1: Greyfall City Ruins (Urban Decay)
├── Concrete, cracked asphalt, rusted rebar, abandoned vehicles, barricades
└── Color Palette: Concrete Grey (#5C6364), Rust Oxide (#8F432B), Moss Green (#3E4F3E)

Sector 2: Blackwood Forest & River Valley (Wilderness)
├── Douglas firs, Western hemlocks, mossy boulders, mud tracks, rushing mountain creek
└── Color Palette: Deep Evergreen (#1D2B1F), Wet Soil (#3D2E24), Slate Rock (#4A5056)

Sector 3: Outpost Omega (Military Compound)
├── Sandbags, razor wire, weathered olive drab containers, watchtowers, spotlight rigs
└── Color Palette: Military Green (#4B5320), Galvanized Steel (#7B8893), Warning Hazard Yellow (#C2922E)

Sector 4: Abandoned Industrial District (Manufacturing)
├── Corrugated metal warehouses, smokestacks, chemical vats, decaying freight trains
└── Color Palette: Burnt Orange (#8B4513), Soot Black (#1C1E20), Corroded Iron (#54473F)

Sector 5: Sub-Level 4 Biolab (Underground Horror)
├── Flickering fluorescent tubes, smashed biosafety containment glass, dried blood trails
└── Color Palette: Cold Sterile Cyan (#2B4C56), Biohazard Amber (#D9822B), Shadow Black (#0E1113)
```

---

## 2. PBR MATERIAL & TEXTURE REUSE ARCHITECTURE

To achieve AAA fidelity on mobile devices without exceeding memory bandwidth:
* **Tileable Trimsheets & Macro Details:**
  * `Trim_Military_Panels_2K` (Doors, wall panels, electrical conduits).
  * `Trim_Urban_Decay_2K` (Curbs, brick damage, window frames, sheet metal).
* **Decal Projection System (URP Decal Renderer Feature):**
  * Blood spatters, puddle ripples, cracked plaster, graffiti, bullet impact holes.
* **Vertex Color Blending:**
  * Vertex Color R: Mud / Puddle wetness layer.
  * Vertex Color G: Moss / Lichen accumulation.
  * Vertex Color B: Rust / Scorch marks.
  * Vertex Color A: Ambient dirt occlusion.

---

## 3. CINEMATIC LIGHTING & ATMOSPHERE

1. **Day/Night Cycle Lighting:**
   * **Noon (Overcast):** Diffuse sunlight through heavy cloud cover. Soft contact shadows, cold ambient bounce.
   * **Golden Hour / Sunset:** Low-angle warm amber sunlight cutting through tree branches (volumetric god rays).
   * **Midnight (Eclipse Dark):** Extreme pitch-black exterior, illuminated solely by player tactical flashlight, moonlight breaking through cloud rifts, and eerie emergency red flares.
2. **Volumetric Fog & Particle Mist:**
   * Ground-hugging height fog in riverbeds and low valleys.
   * Particle dust motes in abandoned building interiors illuminated by window light shafts.
