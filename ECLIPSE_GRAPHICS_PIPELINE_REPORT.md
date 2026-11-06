# ECLIPSE: THE LAST CIVILIZATION
## GRAPHICS PIPELINE & RENDERING REPORT — URP 17 & WEBGL ARCHITECTURE

**Document Version:** 2.0.0-PRO  
**Graphics Lead:** Senior URP Graphics Programmer & Technical Artist  
**Engines Covered:** Unity 6 URP 17.0.3 & Playable WebGL 3D Pipeline  

---

### 1. URP 17.0.3 RENDERING PIPELINE ARCHITECTURE

The graphics pipeline utilizes Universal Render Pipeline (URP 17.0.3) configured across three discrete mobile/desktop tiers:

```
+-------------------------------------------------------------------------------+
|                       URP 17.0.3 RENDERING PIPELINE                           |
+-------------------------------------------------------------------------------+
|  FORWARD+ RENDERING PATH                                                      |
|  - Clustered light culling (supports multiple dynamic lights per cluster)    |
|  - Single-pass forward execution minimizing bandwidth on mobile tiles        |
+-------------------------------------------------------------------------------+
|  PBR SHADER MODEL                                                             |
|  - Cook-Torrance microfacet specular BRDF                                     |
|  - GGX normal distribution function                                          |
|  - Smith joint masking-shadowing function                                     |
|  - Schlick's approximation for Fresnel reflectance                            |
|  - Subsurface scattering approximation for character skin                     |
+-------------------------------------------------------------------------------+
|  POST-PROCESSING VOLUME STACK                                                 |
|  - Filmic Tone Mapping (ACES standard)                                        |
|  - Bloom (Threshold: 1.15, Soft Knee: 0.5, Intensity: 0.35)                   |
|  - Vignette (Classic camera lens falloff for claustrophobic survival tension) |
|  - Color Adjustments: Post Exposure +0.15, Contrast +12, Saturation -15       |
|  - Depth of Field: Bokeh for Main Menu showcase & Weapon Inspection           |
+-------------------------------------------------------------------------------+
```

---

### 2. QUALITY PROFILES SPECIFICATION

| Pipeline Feature | Low Quality (Entry Mobile) | Medium Quality (Balanced) | High Quality (Flagship / Web) |
|:---|:---|:---|:---|
| **Render Scale** | 0.85x | 1.0x | 1.0x |
| **Anti-Aliasing** | FXAA | SMAA Low | SMAA High / MSAA 2x |
| **Shadow Quality** | Hard Shadows, 1 Cascade, 25m | Soft Shadows, 2 Cascades, 50m | Soft Shadows, 4 Cascades, 90m |
| **Reflection Probes** | Off (Spherical Harmonics) | Baked 128x128 Cubemap | Realtime Blended 256x256 |
| **Particle Budget** | 250 max concurrent particles | 600 max concurrent particles | 1,500 max with soft particles |
| **Skinned Bones/Vertex** | 2 Bones per vertex | 4 Bones per vertex | 4 Bones per vertex |
| **Target Frame Rate** | 30 FPS Rock Solid | 60 FPS Smooth | 60 - 120 FPS Max Refresh |

---

### 3. SHADER UPGRADES IN PROTOTYPE

1. **`Eclipse/Character/RealisticCharacterPBR`:**
   - Evaluates direct lighting with GGX specular highlights.
   - Replaces previous cartoon cel-shaded ramp with smooth physical light falloff.
   - Incorporates subtle skin SSS scatter map and ambient occlusion cavity shadows.
2. **`Eclipse/Character/RealisticInfectedPBR`:**
   - Mottled albedo blend with deep purple thrombosed vein channels.
   - Multi-layer roughness map capturing moist decaying organic tissue.
   - Controlled ocular mutation emission with sinusoidal ocular pulse modulation.
3. **`Eclipse/Environment/WetAsphaltPBR`:**
   - Normal-mapped asphalt macro texture with micro-aggregate roughness.
   - Real-time puddle masking displaying sky reflection and dynamic light highlights.
