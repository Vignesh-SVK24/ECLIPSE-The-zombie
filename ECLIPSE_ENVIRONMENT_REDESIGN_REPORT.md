# ECLIPSE: THE LAST CIVILIZATION
## ENVIRONMENT REDESIGN TECHNICAL REPORT — POST-APOCALYPTIC WORLD ATLAS

**Document Version:** 2.0.0-PRO  
**Environment Lead:** Senior Technical Environment & Lighting Artist  
**Focus Sector:** Greyfall City Quarantine Sector (Batch A Prototype)  

---

### 1. GREYFALL CITY — THE ABANDONED METROPOLIS (PROTOTYPE SCENE)

Greyfall City was the primary metropolitan hub in the regional evacuation grid before quarantine perimeter breach. The visual presentation evokes an authentic disaster aftermath:

1. **Roadways & Pavement:**
   - 4-lane cracked asphalt roadway with heavy wear, faded yellow double centerlines, and transverse expansion joints.
   - Potholes exposing structural aggregate and rusted rebar.
   - Puddle accumulation with realistic ground fresnel reflections catching sunset skylight.
2. **Abandoned Vehicles:**
   - City transit bus wedged sideways across the evacuation boulevard, windows smashed, interior emergency lights dead.
   - Police squad cars with dented doors, flat tires, and intermittent roof beacon flashes.
   - Civilian sedans crushed beneath fallen concrete debris.
3. **Barricades & Checkpoints:**
   - Modular concrete K-rail barricades with rusted steel rebar reinforcement.
   - Rusted concertina razor wire strung across choke points.
   - Weathered wooden barricades spray-painted with survivor markings: *"SURVIVE TOGETHER"*, *"SAFE ZONE 2 KM"*.
4. **Atmospheric FX & Dressing:**
   - Metal burning oil drum with real-time flickering point light and drifting fiery embers.
   - Decayed trash bags, cardboard shipping cartons, abandoned ammo crates, and spent brass casings scattered on the sidewalks.
   - Deciduous ivy and weeds reclaiming concrete seams and drainage ditches.

---

### 2. THE EXPANDED SECTOR ATLAS (BATCH D ROADMAP)

| Sector | Environmental Archetype | Dominant Palette | Key Landmark Assets |
|:---|:---|:---|:---|
| **01. Greyfall City** | Urban evacuation corridor | Concrete Gray, Wet Black, Rust | Blocked transit bus, barricades, burning barrels |
| **02. Blackwood Forest** | Dense temperate wilderness | Moss Green, Mud Brown, Bark | Overturned supply trucks, abandoned ranger cabins |
| **03. Ironshield Military Base** | Fortified military compound | Olive Drab, Sandbag Tan, Gunmetal | Watchtowers, sandbag emplacements, armored APCs |
| **04. Ashen Industrial District** | Heavy manufacturing plants | Rust Orange, Charcoal, Iron | Overhead gantry cranes, rail cars, steam pipes |
| **05. Research Complex** | High-containment bio-lab | Sterile White, Hazard Yellow, Blood | Shattered isolation chambers, server racks, airlocks |

---

### 3. TECHNICAL BUDGET & PERFORMANCE BENCHMARKS

- **Draw Call Target:** Under 85 draw calls per frame on mobile devices via GPU instancing and static batching.
- **LOD Hierarchy:**
  - LOD 0 (0-15m): Full detailed PBR meshes with normal and roughness maps.
  - LOD 1 (15-35m): 45% polygon reduction, single combined material pass.
  - LOD 2 (>35m): Simplified collision silhouettes with baked diffuse.
- **Lighting Model:** Baked indirect illumination + 1 real-time directional sun with cascade shadow maps + up to 4 dynamic point lights for torches and fires.
