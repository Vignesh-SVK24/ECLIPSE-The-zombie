# ECLIPSE: THE LAST CIVILIZATION
## MOBILE OPTIMIZATION & PERFORMANCE REPORT — ANDROID & iOS

**Document Version:** 2.0.0-PRO  
**Optimization Lead:** Mobile Performance & Build Specialist  
**Target Hardware:** Android ARM64 (Vulkan / GLES 3.2) & Apple Silicon iOS (Metal)  

---

### 1. MOBILE HARDWARE CONSTRAINTS & TARGET METRICS

Mobile hardware presents strict thermal envelope constraints and memory bandwidth bottlenecks. The optimization system enforces the following limits:

- **Thermal Budget:** Sustained gameplay without thermal throttling for 45+ minutes.
- **Draw Call Limit:** Maximum 90 draw calls per scene view on mobile.
- **Vertex Budget:** Under 120,000 active rendered vertices per frame.
- **Memory Footprint:** Peak RAM consumption under 650 MB on mobile devices.
- **Battery Drain Rate:** Less than 12% battery consumption per 30 minutes of active play.

---

### 2. OPTIMIZATION STRATEGIES IMPLEMENTED

```
+---------------------------------------------------------------------------------+
|                         MOBILE OPTIMIZATION STACK                               |
+---------------------------------------------------------------------------------+
|  1. GPU INSTANCING & MATERIAL BATCHING                                          |
|     - All environment props (barricades, debris, cars, lampposts) share         |
|       instanced material property blocks.                                       |
|     - Zombie variations use GPU-instanced palette offsets.                     |
+---------------------------------------------------------------------------------+
|  2. ADAPTIVE RESOLUTION & FRAME PACING                                          |
|     - Dynamic resolution scaling (0.85x - 1.0x) triggered if frame times        |
|       exceed 16.6ms for 3 consecutive seconds.                                  |
|     - Application.targetFrameRate locked to 60 FPS (with 30 FPS Low Mode).      |
+---------------------------------------------------------------------------------+
|  3. ASYNCHRONOUS OBJECT POOLING                                                 |
|     - Zero runtime allocations in game loops.                                   |
|     - Bullets, blood splatters, muzzle flashes, and zombie ragdolls pre-pooled. |
+---------------------------------------------------------------------------------+
|  4. TEXTURE COMPRESSION MATRIX                                                  |
|     - Android: ASTC 6x6 for character albedos, ASTC 4x4 for normal maps.        |
|     - iOS: ASTC 6x6 / ASTC 4x4 with PVRTC fallback disabled.                   |
|     - WebGL: Optimized DXT / WebP mipmaps with anisotropic filtering capped.    |
+---------------------------------------------------------------------------------+
|  5. HARDWARE SAFE AREA FITTER                                                   |
|     - Screen.safeArea insets automatically applied via CSS env(safe-area-inset) |
|       and Unity SafeAreaFitter.cs to prevent notch and pill overlap.           |
+---------------------------------------------------------------------------------+
```

---

### 3. TOUCH LATENCY & INPUT RESPONSIVENESS

Touch latency is critical for mobile survival shooters:
- **Input Sampling Rate:** 120 Hz display touch polling.
- **Touch Exclusion Layers:** Right-screen camera swiping immediately filters out touch events landing on virtual action buttons (`.btn-touch`, `#joystick-zone`, `.weapon-hud`), completely eliminating accidental camera jumping when tapping buttons.
- **Aim Assist Engine:** Sub-degree magnetic smoothing pulls crosshair toward infected upper torsos during ADS targeting, compensating for capacitive touchscreen friction without feeling artificial.
