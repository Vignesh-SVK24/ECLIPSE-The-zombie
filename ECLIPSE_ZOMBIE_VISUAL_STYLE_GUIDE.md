# ECLIPSE: THE LAST CIVILIZATION
# MASTER ZOMBIE VISUAL STYLE GUIDE
**Studio / Developer:** SVK Creationz  
**Target Engine & Pipeline:** Unity 6 (URP 17.0.3), C#, WebGL  
**Visual Direction:** Stylized 3D Animated Cartoon-Realism with Mature Post-Apocalyptic Survival Horror  
**Primary Visual Anchor:** Concept Key Art (Ethan Cross, Barricades, Toxic Green-Eyed Infected Horde)  

---

## 1. Core Visual Philosophy

In *ECLIPSE: THE LAST CIVILIZATION*, the infected are not fantasy monsters, nor are they cute or childish caricatures. They are **decaying, mutated human beings** whose physiology and psychology have been violently rewired by the 2038 outbreak in Greyfall County.

Every zombie belongs to a single coherent infected population while exhibiting marked anatomical divergence based on the duration, severity, and mutation pathway of the pathogen.

```
       [2038 Greyfall Outbreak Pathogen]
                      │
     ┌────────────────┼────────────────┬────────────────┐
     ▼                ▼                ▼                ▼
[Walker]          [Runner]         [Stalker]         [Brute]
Common Shamble   Nervous Agony    Stealth Hunter   Hypertrophic
Low/Group Threat  High Agility    Shadow Ambush     Massive Armor
Single Eye Glow   Dual Eye Flare   Faint Glimmer    Toxic Vein Core
```

---

## 2. Common Visual Identity & Infection Language

All four classes share the following biological signatures:

### 2.1 Decaying Skin & Necrotic Tissue
- **Base Color Palette:** Sickly pale gray-green (`#5A6B54` to `#4A584A`) with uneven localized desaturation.
- **Infection Veins:** Subdermal branching networks of dark forest-green, bruised violet, and obsidian veins radiating outward from the chest, neck, and ocular orbits.
- **Mottled Surfaces:** Micro-roughness variation (`0.75 - 0.92`) simulating damp, rotting epidermis, dried crusty plasma, and dirt encrustation.
- **Lacerations & Exposed Anatomy:** Irregular tears revealing dull crimson subcutaneous tissue and ivory bone fragments (ribs, radius, ulna, cranium).

### 2.2 Ocular Infection Glow (The "Toxic Eclipse Eye")
- **Internal Biological Luminescence:** The eye glow is an internal bioluminescent mutation of the tapetum lucidum and optic nerve, **not** an external LED flashlight.
- **Color Temperature:** Neon lime-green (`#2BF805` / `#00FF66` / RGB `0.17, 1.0, 0.04`).
- **Asymmetry:** Most commonly manifest in **one dominant eye** (especially in Walkers and Stalkers), with the companion eye clouded by cloudy white cataract blindness. Runners and Brutes exhibit bilateral flare due to advanced central nervous penetration.
- **Environmental Response:** Low diffuse intensity in daylight (`1.5 - 2.5 cd/m²`), flaring with distinct readability in shadows, dark hallways, and twilight.

### 2.3 Facial Distortion & Dental Trauma
- **Jaw Displacement:** Partially dislocated, dropped, or fractured mandible exposing blackened gums and broken, yellowed enamel.
- **Nasal & Lip Erosion:** Asymmetrical tissue necrosis around lips, exposing the upper teeth in an involuntary, permanent rictus grimace.
- **Sunken Orbits:** Deep orbital shadows exaggerating facial bone structure and skull silhouette.

### 2.4 Weathering, Dirt & Outbreak Clothing
- **Fabric Distress:** Frayed seams, bullet tears, claw gashes, and fabric rot along jacket elbows and knee creases.
- **Contamination Layers:** Procedural layering of caked roadside mud, dried blood splatters, soot from burning barricades, and industrial rust.
- **Functional Artifacts:** Broken utility buckles, dangling radio straps, torn backpacks, and shattered protective gear remnants reflecting pre-outbreak civilian and military identities.

---

## 3. Class Silhouette Differentiation

To guarantee instant silhouette recognition during high-stress combat across all platforms (mobile, PC, console), each class conforms to an exaggerated, unmistakable bounding geometry:

```
Walker               Runner               Stalker              Brute
(Trapezoid Down)    (Inverted Triangle)  (Crouched Diamond)   (Massive Heavy Block)

   [  O  ]               \ O /               .  O  .           [======O======]
   /  |  \               / | \               /  |  \           |  /|     |\  |
  /   |   \             /  |  \             /   |   \          | / |     | \ |
 /   / \   \           /  / \  \           /   / \   \         |/  |     |  \|
    /   \                 |   |               /   \           [====|=====|====]
 (Slow Drag)          (Spring Stance)       (Low Crawl)          /   \ /   \
                                                                (Ground-Shaking)
```

1. **Walker (Common):** Uneven shoulder drop (`~15°`), one dragging foot, arms loosely dangling forward.
2. **Runner (Pursuit):** Forward-pitched torso (`~35°`), hyper-flexed knees, athletic tendon tension, wide manic claw reach.
3. **Stalker (Ambush):** Hyper-hunched spine (`~50°`), elongated spider-like forearm silhouette, crouched low center of gravity.
4. **Brute (Titan):** Expanded chest width (`2.2x` human standard), hypertrophic neck muscles obscuring jawline, armored shoulder silhouettes.

---

## 4. Mobile & URP PBR Material Standards

| Texture Map | Format | Compression (Android/iOS) | Resolution | Notes |
|---|---|---|---|---|
| **Albedo + Alpha** | RGBA32 | ASTC 6x6 / BC7 | 1024x1024 (512 on Low) | Base skin, clothes, blood, alpha for torn fringe |
| **Normal Map** | RGB24 | ASTC 6x6 (Normal) / BC5 | 1024x1024 (512 on Low) | Muscle definition, ripped cloth, bone ridges |
| **Mask (ORM)** | RGB24 | ASTC 6x6 / BC7 | 512x512 | R: Occlusion, G: Roughness, B: Metallic |
| **Emission** | RGB24 | ASTC 8x8 / BC7 | 256x256 | Internal eye glow, toxic vein luminescence |

- **Shader Model:** URP Lit Shader with GPU Instancing enabled.
- **Draw Call Ceilings:** Combined zombie pass < 35 draw calls on mobile via dynamic instancing.
