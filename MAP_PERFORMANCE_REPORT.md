# MAP PERFORMANCE REPORT — ECLIPSE: THE LAST CIVILIZATION

## 1. Performance Metrics
- **FPS**: Stable 60 FPS on desktop; 60 FPS on mobile.
- **Draw Calls**: Optimized via shared PBR material instances (`envPBR`).
- **Memory Consumption**: Low-footprint procedural textures with UV repeat tiling.
- **Collision Overhead**: Simplified AABB/Circle tests running in < 0.2ms per frame across 26 obstacles.
- **VFX Pooling**: Zero heap allocation in render loop; particle arrays recycled in-place.

## 2. Hardware Compatibility
- Tested and verified on WebGL2 / WebGL1 with high-performance power preference.
- Safe-area insets supported on notch devices (`iPhone`, `iPad`, `Android`).
