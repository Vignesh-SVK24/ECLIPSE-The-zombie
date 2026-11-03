# ECLIPSE: THE LAST CIVILIZATION
# GRAPHICS PERFORMANCE TARGETS & MOBILE BUDGET SPECIFICATION

**Lead Optimization Engineer:** SVK Creationz  
**Target Hardware:**  
- **Android Tier 1 (Low):** Mali-G52 / Adreno 610, 4GB RAM (Target: 30 FPS stable @ 720p)  
- **Android Tier 2 (Medium):** Mali-G77 / Adreno 660, 6GB RAM (Target: 45-60 FPS @ 1080p)  
- **Android Tier 3 (High):** Mali-G715 / Adreno 740 / Snapdragon 8 Gen 2/3, 8GB+ RAM (Target: 60 FPS @ 1080p-1440p)  
- **iOS Tier 1 (A13/A14 - iPhone 11/12):** 45-60 FPS @ Native Retina  
- **iOS Tier 2 (A15/A16/M-series - iPhone 13-16 / iPad Pro):** 60 FPS Ultra Settings  

---

## 1. HARDWARE PERFORMANCE BUDGET TABLE

| Metric | Mobile Low (30 FPS) | Mobile Medium (45-60 FPS) | Mobile High (60 FPS) | Desktop / PC Target |
| :--- | :--- | :--- | :--- | :--- |
| **Total Scene Triangles** | 150,000 - 250,000 | 350,000 - 550,000 | 800,000 - 1,200,000 | 3,000,000+ |
| **Draw Calls (Batches)** | < 120 | < 220 | < 350 | < 1,200 |
| **SetPass Calls** | < 45 | < 75 | < 120 | < 300 |
| **Skinned Meshes** | Max 8 simultaneously | Max 20 simultaneously | Max 45 simultaneously | Max 100+ |
| **Skin Bone Matrix Weights** | 2 Bones / Vertex | 4 Bones / Vertex | 4 Bones / Vertex | 4 Bones / Vertex |
| **Texture VRAM Budget** | 450 MB | 850 MB | 1,400 MB | 4,000 MB |
| **Render Scale** | 0.75x (720p) | 0.85x - 1.0x (1080p) | 1.0x Native | 1.0x - 1.5x |
| **Shadow Cascades** | 1 Cascade (1024x1024) | 2 Cascades (2048x2048) | 4 Cascades (2048x2048) | 4 Cascades (4096) |
| **Post-Processing** | Bloom, Color Grade | Bloom, Vignette, ACES | SSAO, Bloom, Vignette | SSAO, DoF, Motion Blur |

---

## 2. DRAW CALL & BANDWIDTH MITIGATION STRATEGIES

1. **SRP Batcher Compatibility:**
   * All custom character and environment shaders must declare all material parameters within uniform buffers `UnityPerMaterial`.
   * Never modify individual material properties dynamically with `material.color = ...`; use `MaterialPropertyBlock` or URP Per-Instance CBuffer.
2. **GPU Instancing:**
   * Enabled on all environment props (vegetation, debris, barricades, barrels).
3. **LOD Groups & Culling:**
   * Screen relative transition heights:
     * LOD0 -> LOD1: 0.55 screen height.
     * LOD1 -> LOD2: 0.25 screen height.
     * LOD2 -> LOD3: 0.08 screen height.
     * Cull: < 0.03 screen height.
4. **Dynamic Resolution Scaling:**
   * Enabled in URP asset with target framerate feedback loop to automatically throttle resolution if frame drops below 28 FPS.
