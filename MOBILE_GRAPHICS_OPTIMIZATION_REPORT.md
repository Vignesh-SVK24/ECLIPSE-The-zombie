# ECLIPSE: THE LAST CIVILIZATION
## MOBILE GRAPHICS OPTIMIZATION REPORT (PHASE 6)

### 1. Target Platforms & Performance Budget
- **Primary Targets:** Android (Vulkan API 26–34 / GLES 3.2 fallback), iOS (Metal on iPhone 11–16, iPadOS).
- **Target Frame Rate:** 60 FPS on mid-to-high tier mobile chipsets (Snapdragon 7/8 series, Apple A13–A18, Dimensity 8000+).
- **Memory Footprint:** < 35 MB total runtime RAM allocation.
- **Draw Call Budget:** 150–280 draw calls per frame.

---

### 2. Optimization Techniques Applied
1. **Procedural PBR Textures in Client Memory:**
   - Textures synthesized via native Canvas 2D contexts at startup.
   - Eliminates megabytes of HTTP network downloads, cross-origin texture blocking, and VRAM thrashing.
   - Total texture VRAM footprint: ~6 MB.
2. **GPU Instancing & Shared Geometry:**
   - Shared box, cylinder, and sphere buffer geometries reused across player, zombie, and environmental prop meshes.
3. **Optimized Particle Pooling:**
   - Pre-allocated particle buffer geometries for atmospheric dust (420 points) and combat VFX (pooled spheres).
   - Particle positions updated in-place via typed `Float32Array` attributes without Garbage Collection allocations.
4. **Frustum & Distance Culling:**
   - Active directional shadow camera bounded to `d = 50` units around player.
   - Shadows disabled on small secondary debris objects to preserve mobile GPU fill rate.
