# ECLIPSE: THE LAST CIVILIZATION
## CHARACTER REDESIGN TECHNICAL REPORT — ETHAN CROSS & SURVIVORS

**Document Version:** 2.0.0-PRO  
**Character Lead:** Technical Character Artist & Anatomy Specialist  
**Target Architecture:** Unity 6 Humanoid Rig & WebGL Procedural 3D Mesh Engine  

---

### 1. ETHAN CROSS SPECIFICATION & ANATOMICAL RATIOS

- **Archetype:** Experienced Survival Combat Engineer / Outbreak Veteran.
- **Height:** 1.83 m (6 ft 0 in).
- **Proportion Standard:** 1:7.8 head-to-body ratio (realistic heroic proportion), eliminating previous cartoon 1:6 ratio.
- **Cranial Structure:** Defined zygomatic arches, firm angular jawline, short trimmed dark brown hair with natural volume, textured facial stubble, and a healed laceration scar over the left cheek.

---

### 2. COSTUME & TACTICAL GEAR BREAKDOWN

```
        +----------------------------------------+
        |         ETHAN CROSS TACTICAL RIG       |
        +----------------------------------------+
        |  1. HOODED CANVAS COMBAT JACKET        |
        |     - Desaturated Crimson (#752522)    |
        |     - Reinforced elbow & shoulder pads |
        |     - Canvas roughness: 0.65           |
        +----------------------------------------+
        |  2. CONCEALED BALLISTIC CHEST CARRIER  |
        |     - Matte Charcoal Cordura (#2B2F33) |
        |     - Molle webbing & radio holster    |
        +----------------------------------------+
        |  3. HEAVY UTILITY & HOLSTER BELT       |
        |     - Worn Saddle Leather (#4A3525)    |
        |     - Drop-leg pistol holster (Right)  |
        |     - Dual 9mm magazine pouches (Left) |
        +----------------------------------------+
        |  4. RIPSTOP CARGO COMBAT TROUSERS      |
        |     - Dark Slate Olive (#323835)       |
        |     - Reinforced knee articulation     |
        +----------------------------------------+
        |  5. TACTICAL VIBRAM COMBAT BOOTS       |
        |     - Black oiled leather & mud wear   |
        +----------------------------------------+
```

---

### 3. PBR MATERIAL & TEXTURE SPECIFICATIONS

| Submesh | Material ID | Albedo Map | Normal Map | Metallic | Smoothness | SSS Intensity |
|:---|:---|:---|:---|:---|:---|:---|
| **0: Skin** | `M_Ethan_RealisticSkin` | `T_EthanCross_Skin_Albedo` | `T_EthanCross_Skin_Normal` | 0.00 | 0.42 | 0.35 (Subtle peach) |
| **1: Jacket** | `M_Ethan_FieldJacket` | `T_EthanCross_Cloth_Albedo` | `T_EthanCross_Cloth_Normal` | 0.04 | 0.28 | 0.12 (Low canvas) |
| **2: Armor** | `M_Ethan_TacticalVest` | `T_EthanCross_Gear_Albedo` | `T_EthanCross_Gear_Normal` | 0.22 | 0.45 | 0.00 |
| **3: Leather** | `M_Ethan_WornLeather` | `T_EthanCross_Gear_Albedo` | `T_EthanCross_Gear_Normal` | 0.10 | 0.38 | 0.05 |

---

### 4. RIGGING & SOCKET COMPATIBILITY

Ethan Cross utilizes the standardized Unity 6 65-bone Humanoid Rig:
- `Socket_Weapon_RightHand`: Primary assault rifle / pistol socket with finger trigger alignment.
- `Socket_Weapon_LeftHand`: Foregrip stabilization socket for two-handed firing stance.
- `Socket_Weapon_Holster`: Right hip magnetic lock for tactical sidearm.
- `Socket_Weapon_Back`: Diagonal sling mount across rucksack for primary assault rifle stowing.

In both Unity 6 and the WebGL playable client, the model updates cleanly, maintaining perfect bone weight distribution, smooth foot IK anchoring to the terrain, and dynamic upper-body aiming offsets.
