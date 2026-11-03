# ECLIPSE: THE LAST CIVILIZATION
# ENVIRONMENT PERFORMANCE BASELINE & HARDWARE TARGETS

**Document Version:** 1.0.0  
**Target Hardware:** Android (Vulkan API 26-34), iOS (Metal), PC/Desktop  
**Lead Optimization Engineer:** SVK Creationz  
**Date:** October 2026  

---

## 1. PERFORMANCE BASELINE & METRIC BUDGETS

To deliver AAA visual immersion on mobile without inducing thermal throttling, frame stutter, or battery drain:

| Performance Metric | Mobile Low (Budget) | Mobile Medium (Mid-Range) | Mobile High (Flagship) | PC / Editor Target |
| :--- | :--- | :--- | :--- | :--- |
| **Target Frame Rate** | 30 FPS stable | 45-60 FPS | 60 FPS locked | 60-120 FPS |
| **Visible Screen Triangles** | 180,000 - 250,000 | 400,000 - 600,000 | 800,000 - 1,100,000 | 2,500,000+ |
| **Draw Calls (Batches)** | < 110 | < 200 | < 320 | < 1,000 |
| **SetPass Calls** | < 40 | < 70 | < 110 | < 250 |
| **Environment VRAM** | < 350 MB | < 650 MB | < 1,100 MB | < 3,500 MB |
| **Shadow Distance** | 25 meters | 45 meters | 75 meters | 150 meters |
| **Shadow Cascades** | 1 Cascade (1024x1024) | 2 Cascades (2048x2048) | 4 Cascades (2048x2048) | 4 Cascades (4096) |
| **LOD Transition Heights** | 0.60 / 0.25 / 0.05 | 0.50 / 0.20 / 0.04 | 0.40 / 0.15 / 0.03 | 0.30 / 0.10 / 0.02 |
| **Texture Compression** | ASTC 6x6 / ETC2 | ASTC 6x6 / ASTC 4x4 | ASTC 4x4 | BC7 / BC5 |

---

## 2. MEMORY & BANDWIDTH MITIGATION RULES

1. **SRP Batcher Compatibility:**
   - Every environmental custom shader must wrap its material parameters in uniform buffers `CBUFFER_START(UnityPerMaterial)` to batch hundreds of static building and road draw calls into single draw batches.
2. **GPU Instancing on Foliage & Props:**
   - Enabled on all grass clumps, shrubs, debris piles, road signs, and barriers.
3. **Occlusion Culling:**
   - City buildings act as large occluders, hiding parallel streets and alleyways from the GPU rasterizer.
4. **Decal Projection Budgeting:**
   - Max 16 simultaneous visible screen-space decals on Mobile Low; max 48 on High.
   - Potholes and small road cracks baked directly into primary road materials rather than using separate overlapping decal quads.
