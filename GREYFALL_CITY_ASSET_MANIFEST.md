# ECLIPSE: THE LAST CIVILIZATION
## Greyfall City Asset Manifest & Bill of Materials (Batch B)

- **Date:** 2026-11-03
- **Classification:** Game-Ready Environment Assets
- **Target Pipeline:** Unity 6 URP / WebGL Playable Client

---

### 1. Modular Infrastructure & Road Kit
| Asset Identifier | Asset Type | Dimensions | Material / Shader | Polycount (LOD0 / LOD1) |
| :--- | :--- | :--- | :--- | :--- |
| `SM_Road_Avenue_32x240m` | Static Mesh | $32\text{m} \times 0.2\text{m} \times 240\text{m}$ | `M_Road_Asphalt` (URP Lit) | 240 / 80 tris |
| `SM_Road_Marking_DoubleYellow` | Decal / Overlay | $0.14\text{m} \times 11\text{m}$ | `M_Road_Marking_Yellow` | 40 / 12 tris |
| `SM_Road_Marking_WhiteDash` | Decal / Overlay | $0.18\text{m} \times 5.5\text{m}$ | `M_Road_Marking_White` | 40 / 12 tris |
| `SM_Road_Zebra_Crosswalk` | Decal / Overlay | $1.2\text{m} \times 4.2\text{m}$ | `M_Road_Marking_White` | 40 / 12 tris |
| `SM_Road_Puddle_Reflective` | Decal / Overlay | Variable ($3.5\text{m} - 4.5\text{m}$) | `M_Road_Asphalt_Wet` | 64 / 24 tris |
| `SM_Curb_Stone_240m` | Static Mesh | $0.5\text{m} \times 0.35\text{m} \times 240\text{m}$ | `M_Concrete_Curbs` | 180 / 60 tris |
| `SM_Sidewalk_Deck_240m` | Static Mesh | $8.5\text{m} \times 0.30\text{m} \times 240\text{m}$ | `M_Sidewalk_Tiles` | 240 / 80 tris |
| `SM_StormDrain_CastIron` | Inset Mesh | $0.8\text{m} \times 0.04\text{m} \times 1.8\text{m}$ | `M_Rusted_Metal` | 96 / 32 tris |
| `SM_Streetlight_CobraHead` | Municipal Mesh | Height $8.0\text{m}$, Arm $2.4\text{m}$ | `M_Rusted_Metal` + Emissive | 480 / 150 tris |
| `SM_TrafficGantry_31m` | Municipal Mesh | Width $31.5\text{m}$, Height $8.2\text{m}$ | `M_Rusted_Metal` + Hazard | 920 / 310 tris |
| `SM_FireHydrant` | Municipal Mesh | $0.35\text{m} \times 0.9\text{m} \times 0.35\text{m}$ | `M_Blood_Decal` (Red) | 160 / 60 tris |

---

### 2. Architecture & Modular Building Kit
| Asset Identifier | Style | Stories | Explorable Interior | Polycount (LOD0 / LOD1 / LOD2) |
| :--- | :--- | :--- | :--- | :--- |
| `Bld_Apartment_Brick_01` | Residential | 4 stories | No | 3,800 / 1,400 / 450 tris |
| `Bld_Apartment_Brick_Explorable` | Residential | 3 stories | **Yes (Living Area & Stash)** | 4,200 / 1,600 / 520 tris |
| `Bld_Commercial_Shopfront_01` | Commercial | 3 stories | No | 3,400 / 1,200 / 380 tris |
| `Bld_Commercial_Grocery_Explorable`| Commercial | 4 stories | **Yes (Shelves & Crate)** | 3,900 / 1,500 / 480 tris |
| `Bld_Clinic_Plaster_Explorable` | Medical | 3 stories | **Yes (Triage Bed & Kit)** | 3,600 / 1,350 / 420 tris |
| `SM_FireEscape_Balcony` | Architectural | Standard per floor | Accessible | 320 / 110 tris |
| `SM_Rooftop_AC_Condenser` | Utility Mesh | $2.2\text{m} \times 1.8\text{m} \times 2.8\text{m}$ | No | 180 / 60 tris |
| `SM_Rooftop_WaterTower` | Utility Mesh | Diameter $2.2\text{m}$, Height $4.5\text{m}$| No | 340 / 120 tris |

---

### 3. Abandoned Vehicle Kit
| Asset Identifier | Classification | Damage States | Special Features | Polycount (LOD0 / LOD1) |
| :--- | :--- | :--- | :--- | :--- |
| `SM_Veh_PoliceCruiser_Damaged` | Emergency Sedan | Flat FL tire, open door, shattered glass | Flashing red siren beacon | 4,200 / 1,600 tris |
| `SM_Veh_Sedan_Evacuation` | Civilian Vehicle | Weathered paint, dented hood | Strapped roof luggage rack | 3,800 / 1,450 tris |
| `SM_Veh_MilitaryTruck_Blocking` | Heavy Transport | Dust accumulation, rusted cargo bed | Full road lane obstruction | 4,900 / 1,800 tris |
| `SM_Veh_Ambulance_Emergency` | Paramedic Van | Bloodstains, dropped stretcher | Open rear medical doors | 4,600 / 1,750 tris |
| `SM_Veh_Motorcycle_Crashed` | Two-Wheeler | Scratched frame, cracked headlight | Overturned on sidewalk curb | 1,200 / 450 tris |

---

### 4. Quarantine Checkpoint & Storytelling Kit
| Asset Identifier | Purpose | Dimensions | Colliders |
| :--- | :--- | :--- | :--- |
| `SM_JerseyBarrier_Concrete` | Highway Obstacle | $3.6\text{m} \times 1.1\text{m} \times 0.8\text{m}$ | Heavy Box Collider |
| `SM_SandbagNest_3Tier` | Defense Emplacement | $2.4\text{m} \times 0.85\text{m} \times 1.2\text{m}$ | Box Collider |
| `SM_Military_Searchlight` | Evacuation Spot | Height $2.2\text{m}$, Spot $45^\circ$ | Cylinder Collider |
| `SM_BiohazardSign_Board` | Storytelling Warning | $1.4\text{m} \times 1.0\text{m} \times 0.06\text{m}$ | Non-blocking |
| `SM_Military_AmmoCrate` | Supply Prop / Loot | $0.9\text{m} \times 0.7\text{m} \times 0.9\text{m}$ | Box Collider |
| `SM_StoryDecal_BloodStain` | Outbreak Indicator | $2.5\text{m} \times 3.2\text{m}$ | Non-blocking |
| `SM_Scattered_Luggage` | Aftermath Prop | $0.6\text{m} \times 0.25\text{m} \times 0.45\text{m}$ | Box Collider |

---

### 5. Nature Reclaiming Kit
| Asset Identifier | Description | Materials | LOD Setup |
| :--- | :--- | :--- | :--- |
| `SM_Tree_Oak_Mature` | Broadleaf oak tree with 3-tier canopy | `M_Tree_Bark`, `M_Foliage_Overgrowth` | LOD0: 2,800, LOD1: 950 tris |
| `SM_Wild_Bush_Cluster` | Dense ground shrub along curbs | `M_Foliage_Overgrowth` | LOD0: 420, LOD1: 140 tris |
| `SM_RoadCrack_Weed` | Asphalt crack vegetation | `M_Foliage_Overgrowth` | 64 tris (LOD cull at 15m) |

