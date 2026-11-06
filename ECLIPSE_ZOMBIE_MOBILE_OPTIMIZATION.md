# ECLIPSE: THE LAST CIVILIZATION
# ZOMBIE MOBILE OPTIMIZATION SPECIFICATION
**Target Platforms:** Android (Vulkan API 26-34 / GLES fallback), iOS (Metal, iPhone 11-16, iPadOS)  
**Engine:** Unity 6 URP 17.0.3  

---

## 1. Multi-Enemy Performance Budgets

To ensure stable 60 FPS on midrange mobile chipsets (Snapdragon 7 Gen 1, Dimensity 8200, Apple A14 Bionic) and 30-45 FPS on baseline devices, the enemy rendering budget is strictly enforced:

| Metrics | 5 Zombies (Light Encounter) | 15 Zombies (Street Horde) | 30 Zombies (Major Swarm Limit) |
|---|---|---|---|
| **Total Triangles** | 22,500 tris | 52,500 tris (with LODs) | 85,000 tris (with LODs) |
| **Draw Calls** | 5 - 10 calls | 12 - 18 calls (GPU Instanced) | 18 - 25 calls (GPU Instanced) |
| **Skinned Bone Transforms** | 180 bones/frame | 360 bones/frame (culled distant) | 540 bones/frame (rate throttled) |
| **Audio Channels Active** | 3 simultaneous | 6 simultaneous | 8 max audio pool |

---

## 2. LOD Hierarchy & Distance Throttling

Each zombie class implements a 3-tier **LODGroup**:

```
LOD 0 (0m - 12m)    : 4,500 - 5,500 tris | Full PBR, Normal, ORM, Eye Glow, 60Hz Skinned Mesh
LOD 1 (12m - 28m)   : 1,800 - 2,200 tris | Diffuse + Emission, 30Hz Animation Culling
LOD 2 (28m - 50m+)  : 600 - 800 tris     | Unlit Silhouette, Shadows Disabled, 15Hz AI Tick
Cull (> 50m)        : Renderer Disabled  | Sensory Sleep / Low-frequency NavMesh tick
```

---

## 3. GPU Instancing & Shader Optimization
- All zombie materials enable **Enable GPU Instancing** in URP.
- Material Property Blocks (`MaterialPropertyBlock`) dynamically override skin hue variation, clothing tint, and eye glow intensity without breaking instance batching.
- Normal and ORM texture sampling is bypassed on LOD 1 and LOD 2 to reduce mobile memory bandwidth pressure.
