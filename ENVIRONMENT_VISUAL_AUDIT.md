# ECLIPSE: THE LAST CIVILIZATION
# ENVIRONMENT VISUAL AUDIT & CURRENT STATE ANALYSIS

**Document Version:** 1.0.0  
**Target:** Unity 6 (6000.0.32f1 LTS) Universal Render Pipeline (URP 17.0.3)  
**World:** Greyfall County, 2038  
**Developer:** SVK Creationz  
**Date:** October 2026  

---

## 1. EXECUTIVE SUMMARY & AUDIT FINDINGS

An in-depth inspection of the current environment assets, scenes, materials, shaders, lighting, and layout was completed. 

### Critical Deficiencies Identified:
1. **Flat, Lifeless Roads:** Roads are represented by untextured flat rectangular planes (`0x1c1e1d` grey) lacking asphalt aggregate, micro-roughness, large cracks, potholes, oil stains, tyre skids, lane dividers, stop lines, or pedestrian crosswalks.
2. **Primitive Box Buildings:** Structures along streets are primitive uniform boxes (`BoxGeometry(16, h, 18)`) without architectural relief, doors, window frames, glass reflections, fire escapes, roof structures, electrical conduits, or weathering.
3. **Empty, Devoid of Environmental Narrative:** The environment lacks evidence of an interrupted civilization: no abandoned luggage, personal effects, medical stretchers, biohazard quarantine checkpoints, military barricades, or emergency shelters.
4. **Simplistic Vehicle Representation:** Vehicles are restricted to single-box stand-ins without wheels, tires, transparent glass, dented panels, open doors, interior steering wheels, or damage rust.
5. **Absence of Reclaiming Vegetation:** Sidewalks and road surfaces have no weeds, grass sprouting through pavement seams, moss accumulation, or natural tree canopies.
6. **Primitive Lighting & Atmosphere:** Lighting relies on a basic directional sun light without local reflection probes, contact shadows, atmospheric volumetric height fog, or flickering localized emergency lighting.

---

## 2. SUBSYSTEM-BY-SUBSYSTEM AUDIT MATRIX

| Subsystem | Existing State | Target Post-Apocalyptic AAA State | Required Technical Action |
| :--- | :--- | :--- | :--- |
| **Roads & Streets** | Flat untextured plane | Layered asphalt PBR with aggregate, cracks, potholes, faded thermoplastic markings, puddles | Custom URP Road Shader / Decal projection system |
| **Pavements & Curbs** | Uniform grey blocks | Raised beveled concrete curb stones, fractured paving slabs, storm drain grates | Modular sidewalk kit with vertex color dirt/moss |
| **Buildings** | Monolithic colored cubes | Modular facade kits: brick, concrete, glass windows, fire escapes, AC units, awnings | Modular building architecture kit with LOD groups |
| **Abandoned Vehicles** | Single box stand-in | Detailed sedans, pickup trucks, police cruisers, ambulances with flat tires, broken glass | Rigged vehicle prefabs with damaged variants |
| **Vegetation** | Minimal/absent | Overgrown nature: weeds in cracks, sidewalk grass, creeping vines, weathered oak/fir trees | GPU instanced foliage with vertex wind animation |
| **Debris & Props** | Only red explosive barrels | Evacuation aftermath: scattered luggage, barricades, sandbags, wooden pallets, street signs | Environmental storytelling prop library |
| **Lighting & Mood** | Flat directional sun | Cinematic overcast/golden hour, volumetric fog, flickering streetlamps, emergency beacon pulses | URP Volume Profile, ACES tonemapping, Light Probes |
| **Collision & NavMesh** | Basic box colliders | Convex mesh colliders, NavMeshObstacle components on abandoned vehicles | Preserved AI pathfinding around wreckage |

---

## 3. PRESERVATION OF CORE GAMEPLAY SYSTEMS

During this visual overhaul, the following systems **must not be disrupted**:
1. **Player Locomotion & Collision:** CharacterController step height (`0.35m`), slope limit (`45°`), and ground checking.
2. **Zombie AI Navigation:** NavMesh agent pathfinding, obstacle avoidance around abandoned vehicles, sound event listening, and vision cones.
3. **Combat & Hit Detection:** Sub-collider hitbox routing (`HitBox.cs`), explosive barrel mechanics, damage numbers, and recoil recovery.
4. **Camera Controls:** Switchable First-Person and Third-Person Over-The-Shoulder cameras with obstacle sphere-cast collision.
5. **Mobile Touch Input:** Virtual joysticks, action buttons, and frame rate management.
