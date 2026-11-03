# ECLIPSE: THE LAST CIVILIZATION
# VFX IMPLEMENTATION PLAN: COMBAT, ENVIRONMENT & CINEMATICS

**Lead VFX Artist:** SVK Creationz  
**Target:** Unity 6 VFX Graph & Shuriken Particle System (URP Mobile Compatible)  

---

## 1. CORE VFX CATEGORIES & POOLING ARCHITECTURE

To eliminate garbage collection pauses on Android and iOS devices, all particle effects use fixed-capacity object pools (`VFXPoolManager`).

### 1.1 Weapon & Combat Effects
* **VFX_MuzzleFlash_Pistol / VFX_MuzzleFlash_Rifle:**
  * Multi-plane starburst sprite, high-intensity point light (duration 0.04s), volumetric smoke puff.
* **VFX_BulletImpact_Flesh (Zombie Hit):**
  * Directional blood mist cone, heavy blood droplet streaks with gravity, surface decal spatter.
* **VFX_BulletImpact_Headshot (Critical Kill):**
  * High-velocity explosive blood dispersal, cranial bone fragment particles, dark crimson stain decal.
* **VFX_BulletImpact_Concrete / Metal / Wood:**
  * Concrete dust cloud, sparks with bouncing physics (metal), splinters and dust (wood).
* **VFX_Explosion_Barrel:**
  * Shockwave distortion ring, fireball expansion (flame flipbook), secondary smoke plume, flying debris shrapnel.

### 1.2 Environment & Atmospheric Effects
* **VFX_GroundMist_Drift:**
  * Low-velocity alpha-blended planar smoke quads hugging terrain surfaces.
* **VFX_Rain_HeavyStorm:**
  * Screen-aligned rain streaks, camera lens droplet simulation, splash ripples on water surfaces.
* **VFX_Fire_Campfire / Barrel:**
  * Procedural flame cards, rising thermal heat distortion, glowing ember sparks with turbulent curl noise.

---

## 2. PARTICLE BUDGETS & MOBILE PERFORMANCE RULES

1. **Max Active Particles:** 300 on Mobile Low, 800 on Mobile Medium, 2,000 on Mobile High.
2. **Overdraw Prevention:** Soft particle fading (`SoftParticlesDistance = 0.5`), tight quad geometry rather than square quads to minimize transparent overdraw.
3. **No Dynamic Mesh Colliders for Particles:** All particle bouncing uses simple mathematical plane approximation or raycast sample.
4. **Lighting Mode:** Unlit or lightweight VertexLit for 90% of particles; only explosion fireballs and muzzle flashes cast dynamic point lights.
