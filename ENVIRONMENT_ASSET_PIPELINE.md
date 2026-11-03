# ECLIPSE: THE LAST CIVILIZATION
## Environment Asset Production Pipeline & Technical Standards

**Developer:** SVK Creationz  
**Game Engine:** Unity 6 (6000.0.32f1 LTS)  
**Render Pipeline:** Universal Render Pipeline (URP 17.0.3)  
**World Setting:** Greyfall County, 2038 (15 Years Post-Collapse)  
**Target Platforms:** Android & iOS (Primary), PC / Editor (Scalable)  
**Document Classification:** Production Standard Operating Procedure (SOP)  

---

### 1. Asset Categorization & Kit Specifications

Every environment asset deployed in *ECLIPSE* belongs to one of seven structured production categories. No primitive placeholders (untextured cubes, cylinders) are permitted in production scenes.

```
Assets/ECLIPSE/Environment/
├── Architecture/     (Modular structural kits, foundations, walls, cornices)
├── Buildings/        (Commercial storefronts, residential tenements, industrial depots)
├── Roads/            (Multi-lane asphalt, curbs, expansion joints, crosswalks, storm drains)
├── Vehicles/         (Civilian, emergency, military, commercial wrecks & bicycles)
├── Nature/           (Urban trees, overgrown weeds, climbing ivy, grass tufts, boulders)
├── Debris/           (Rubble piles, shattered glass, broken bricks, garbage, fallen signs)
└── Props/            (Barricades, sandbags, streetlights, fire hydrants, supply crates)
```

---

### 2. Architectural Modular Kit Standards

To maximize visual variety while maintaining strict mobile draw call budgets through the **GPU Resident Drawer** and **SRP Batcher**, all architecture is authored according to a standardized modular grid.

#### A. Modular Grid Dimensions
- **Horizontal Wall Unit:** Standard width of `4.0m` (Sub-units: `2.0m`, `1.0m`).
- **Story Height:** Ground floor commercial = `4.0m`; Upper residential/office floors = `3.5m`.
- **Wall Thickness:** `0.25m` standard exterior masonry thickness.
- **Doorways:** `1.2m` width x `2.4m` height (accommodates player combat controller with weapon raise).
- **Windows:** Standard opening `1.4m` width x `2.0m` height.

#### B. Architectural Kit Components
1. **Ground Storefront Modules:**
   - Intact commercial shopfront (display windows, recessed entryway).
   - Looted shopfront (shattered safety glass, twisted metal security grates).
   - Boarded shopfront (weathered plywood planks nailed over broken frames).
2. **Mid-Floor Modules:**
   - Double-hung residential window facade.
   - Fire escape anchor facade (structural mounting lugs for modular metal stairs).
   - Cracked brick facade with exposed mortar and structural fracture lines.
3. **Roofline Modules:**
   - Decorative stone cornice, parapet wall (`0.8m` height for cover).
   - Rooftop utility structures: HVAC compressor units, wooden gravity water tanks, access door sheds.
4. **Window Interior Mapping (Fake 3D Interiors):**
   - Windows utilize a specialized **URP Parallax Interior Mapping Shader** to project procedural furnished rooms (abandoned living rooms, ransacked pharmacies) behind glass without adding geometric interior polygons.

---

### 3. Road & Infrastructure Pipeline

The road network of Greyfall County reflects 15 years of vehicular abandonment, seismic damage, and botanical reclamation.

#### A. Geometric Standards
- **Lane Width:** Standard US highway lane width of `3.5m`.
- **Road Cross-Section:** 2-lane road = `7.0m`; 4-lane arterial = `14.0m` plus `0.3m` concrete gutters.
- **Curbs:** Cast concrete curb stones with `0.15m` height and `0.20m` width, including storm drain intake basins at `25m` intervals.
- **Sidewalks:** `2.5m` - `3.5m` width composed of modular `2.0m x 2.0m` concrete flagstones.

#### B. Surface Shading Architecture
- Controlled via [`RoadAsphaltPBR.shader`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/ECLIPSE/Environment/Materials/RoadAsphaltPBR.shader):
  - **Base Layer:** Coarse weathered asphalt PBR with normal map detail.
  - **Faded Markings Layer:** Double solid yellow centerlines and dashed white dividers modulated by wear/grime masks.
  - **Structural Cracks:** Blended crack normal and cavity maps highlighting frost heaves and root damage.
  - **Dynamic Wet Puddles:** Smoothness override and normal flattening in depressed micro-cavities to simulate rainwater accumulation.

---

### 4. Abandoned Vehicle Production Standards

Vehicles in Greyfall County serve dual purposes: visual world-building and tactical cover gameplay.

#### A. Vehicle Taxonomy
1. **Emergency Service:** Greyfall Police Interceptor (flashing LED light bar, bullbar, siren mount), County Ambulance, Fire Engine.
2. **Civilian Evacuation:** Station Wagons with roof luggage racks, 4-door family sedans, compact hatchbacks, pickup trucks.
3. **Military & Heavy Commercial:** 6x6 Heavy Troop Transport, Armored Security Van, Transit Bus, Delivery Box Truck.
4. **Light Transport:** Abandoned cruiser motorcycles, mountain bikes chained to lampposts.

#### B. Component Decomposition & Cover Mechanics
- Vehicles are structured with detachable/articulable sub-meshes:
  - `Door_Front_Left` (can be modeled ajar or ripped off).
  - `Hood` (crumpled from frontal collision or open exposing stripped engine block).
  - `Trunk` (popped open showing ransacked emergency supplies).
  - `Wheels` (4 independent meshes with deflated flat-tire variations).
- **Navigation AI Integration:**
  - Every vehicle prop includes [`AbandonedVehicleProp.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Environment/AbandonedVehicleProp.cs) with a dynamic `UnityEngine.AI.NavMeshObstacle` (`carving = true`).
  - This guarantees zombie hordes cleanly route around car wrecks without clipping through fenders or doors.

---

### 5. Nature & Botanical Overgrowth Pipeline

Nature has aggressively reclaimed Greyfall County over 15 years of human absence.

#### A. Vegetation Species & Hierarchy
1. **Urban Canopy Trees:** Deciduous Ash, Red Oak, Weeping Willow, and dead desiccated trees.
2. **Shrubs & Brambles:** Wild blackberry brambles, overgrown boxwood hedges flanking sidewalks.
3. **Pavement Weeds & Turf:** Crabgrass, dandelion tufts, and tall fescue bursting through asphalt cracks, curb gutters, and storm drains.
4. **Vertical Ivy (Climbers):** English Ivy clusters adhering to damp brick facades and telephone poles.

#### B. Mobile Foliage Shading & Wind Animation
- Vegetation uses a specialized **URP Foliage Shader** authored in Shader Graph:
  - **Two-Sided Lighting:** Transmission / Subsurface Scattering approximation for sunlit leaves.
  - **Vertex Wind Displacement:** Hierarchical wind motion driven by vertex color weights:
    - *Red Channel:* Main trunk bending / sway (low frequency).
    - *Green Channel:* Branch oscillation (medium frequency).
    - *Blue Channel:* Leaf flutter (high frequency).
  - **Alpha Cutout Optimization:** Dithered LOD cross-fade with conservative alpha cutoff threshold (`0.45`) to prevent mobile GPU overdraw bottlenecks.

---

### 6. PBR Material & Texture Technical Standards

#### A. Material Channel Standards (Metallic / Smoothness Workflow)

All environment materials adhere to the Unity Standard URP PBR specification:

| Texture Map | Color Space | Channels | Channel Contents | Format (Mobile) | Format (PC/Editor) |
| :--- | :---: | :---: | :--- | :---: | :---: |
| **_BaseColor** | sRGB | RGBA | RGB: Diffuse Albedo<br>A: Alpha Cutout (if foliage/fence) | ASTC 6x6 | BC7 |
| **_BumpMap (Normal)** | Linear | RGB | DirectX Normal (X+, Y-, Z+)<br>Tangent-space normal vector | ASTC 4x4 or 5x5 | BC5 |
| **_MaskMap (Packed)** | Linear | RGBA | **R:** Metallic (0.0 to 1.0)<br>**G:** Ambient Occlusion (0.0 to 1.0)<br>**B:** Detail Mask / Height<br>**A:** Smoothness (1.0 - Roughness) | ASTC 6x6 | BC7 |
| **_EmissionMap** | sRGB (HDR) | RGB | Self-illumination color and intensity | ASTC 8x8 | BC7 |

#### B. Texel Density Allocations
To maintain visual consistency and prevent blurry textures when the player is in first-person camera mode:

- **1st-Person Weapons & Interactable Loot:** `10.24 px/cm` (1024 pixels per meter).
- **Characters & Vehicles:** `5.12 px/cm` (512 pixels per meter).
- **Architecture, Roads & Walls:** `2.56 px/cm` (256 pixels per meter) paired with a high-frequency `0.5m` tiling detail normal map.
- **Distant Landscape & Mountain Silhouettes:** `0.64 px/cm` to `1.28 px/cm`.

#### C. Texture Resolution Caps (Mobile Budget)
- Master Tileable Materials (Asphalt, Brick, Concrete): `2048 x 2048` (Max 1024 on Low/Medium mobile presets).
- Unique Vehicle Atlases: `2048 x 2048` (Encompassing entire body, interior, and glass).
- Props & Debris Atlases: `1024 x 1024` or `512 x 512`.
- Foliage Atlases: `1024 x 1024` (Single atlas containing twigs, leaves, and bark).

---

### 7. Level of Detail (LOD) & Collision Architecture

#### A. LOD Transition Matrix

Every environment mesh with > 500 triangles must implement a 3-tier `LODGroup`:

```
[Camera Eye] --------------> [LOD0: 100% Poly] -------> [LOD1: 50% Poly] -------> [LOD2: 20% Poly] -------> [Culled]
Screen Relative Metric:            1.0 to 0.60                0.60 to 0.25               0.25 to 0.08              < 0.08
```

| Asset Tier | LOD0 Triangle Budget | LOD1 Triangle Budget | LOD2 Triangle Budget | Shadow Casting |
| :--- | :---: | :---: | :---: | :---: |
| **Multi-Story Building Facade** | 4,000 – 6,000 | 2,000 – 3,000 | 600 – 1,000 | LOD0 & LOD1 Only |
| **Detailed Vehicle Wreck** | 8,000 – 12,000 | 4,000 – 5,000 | 1,200 – 1,800 | LOD0 Only |
| **Mature Urban Tree** | 3,500 – 5,000 | 1,800 – 2,400 | 600 (Cross-Billboard) | LOD0 & LOD1 Only |
| **Small Prop / Supply Crate** | 300 – 600 | 150 – 250 | 50 (Box primitive) | LOD0 Only |

#### B. Collision Hierarchy (No Raw Complex Mesh Colliders)
- **Rule 1:** Complex visual meshes **never** use un-simplified `MeshCollider` components.
- **Rule 2:** Buildings and walls utilize grouped primitive `BoxCollider` components aligned to the architectural grid.
- **Rule 3:** Vehicles utilize compound convex hulls authored in DCC software (`UCX_` prefix) or composite Box/Capsule colliders.
- **Rule 4:** Small rubble, beverage cans, scattered paper, and thin twigs have collision **disabled** to eliminate unnecessary PhysX broadphase overhead.

---

### 8. Asset Technical Validation Protocol

Before any external model or texture is approved into `Assets/ECLIPSE/Environment/Prefabs/`, it must pass the following 7-point technical audit:

1. **Transform Normalization:** Scale = `(1, 1, 1)`, Rotation = `(0, 0, 0)`, Pivot point positioned at the bottom-center contact point with the ground plane.
2. **Degenerate Geometry Check:** Zero non-manifold edges, zero zero-area faces, and zero un-welded co-located vertices.
3. **Lightmap UV Integrity:** UV Channel 1 present, unwrapped within `[0, 1]` bounds without overlap, with minimum 4-pixel padding.
4. **Channel Packing Verification:** Mask map packed correctly (R=Metallic, G=AO, B=Detail, A=Smoothness) with sRGB unchecked.
5. **Normal Map Format:** Texture type set to `Normal map`, format set to ASTC / BC5, linear color space.
6. **LODGroup Validation:** Correct screen-relative transition percentages configured; materials correctly assigned to all LOD renderers.
7. **Collision Layer & Tag:** Tagged as `Environment` or `Cover`; NavMesh static flags set appropriately.
