# ECLIPSE: THE LAST CIVILIZATION
# CHARACTER REDESIGN PLAN: ETHAN CROSS, SURVIVORS & THE INFECTED

**Lead Character Designer:** SVK Creationz  
**Scope:** Humanoid Anatomy Overhaul, Skeletal Topology, PBR Shaders, and Animation BlendTrees  

---

## 1. ETHAN CROSS — HERO PROTAGONIST SPECIFICATION

### 1.1 Character Lore & Visual Silhouette
* **Identity:** Ethan Cross, former search-and-rescue team lead and tactical survival specialist in Greyfall County.
* **Silhouette:** Broad shoulders, athletic torso tapering to a utility-belt-cinched waist, muscular legs with armored knee reinforcement. Recognizable asymmetrical silhouette created by a single-shoulder tactical strap, left-arm tactical watch, and a rugged field backpack.
* **Physical Proportions:**
  * Height: 1.83 m (6 ft 0 in)
  * Head-to-Body Ratio: 1:7.5
  * Eye Line: Halfway down head height
  * Shoulder Width: 2.2 head units

```
                 [ HEAD: 0.24m ]  <-- Anatomical brow, eyes, beard, scar
                 [ NECK: 0.12m ]
       [SHOULDER]==============[SHOULDER]  <-- 0.52m width
            |   [ CHEST / VEST ]   |       <-- Ballistic plate & pouches
            |   [  SPINE / BELT ]  |       <-- Utility belt & sidearm holster
       [ARM]       [ HIPS ]        [ARM]   <-- Tactical glove fingers
       [ELBOW]     /      \        [ELBOW]
       [HAND]    [THIGH]  [THIGH]  [HAND]
                   |        |
                 [KNEE]   [KNEE]           <-- Molded polymer pads
                   |        |
                 [BOOT]   [BOOT]           <-- Lugged combat soles
```

### 1.2 Facial Topology & Expression Rigs
* **Topology:** Circular edge loops radiating from mouth and eyes.
* **Blendshape Targets:**
  * `Flinch_Pain_Left` / `Flinch_Pain_Right` (Triggered on bullet or claw impact).
  * `Aim_Squint` (Subtle focus eye narrowing during ADS).
  * `Exertion_Grit` (Jaw clench during sprint, melee attack, and heavy recoil).
  * `Blink` (Natural procedural eye blinks).

---

## 2. SURVIVOR ROLES SPECIFICATION

| Character | Role | Distinct Visual Elements | Signature Gear |
| :--- | :--- | :--- | :--- |
| **Sarah Mitchell** | Combat Medic | Red cross arm patch, hair bound in practical ponytail, sterile surgical gloves under tactical mitts | Trauma kit, defibrillator pack, defibrillator paddles |
| **Marcus Vance** | Heavy Gunner | Ballistic plate carrier with extra magazines, tattooed arms, ballistic goggles | LMG sling harness, heavy combat knife, knee braces |
| **Elena Rostova** | Recon Scout | Hooded concealment mantle, lightweight ghillie camouflage accents | Compact compound bow / scoped rifle, binocular pouch |
| **Father Gabriel** | Settlement Leader | Worn clergyman's collar, rugged leather coat with stitched bullet holes | Wooden rosary wrapped around left fist, flashlight |

---

## 3. INFECTED CLASSES SPECIFICATION

1. **Walker (Tier 1):**
   * Decayed greyish-blue flesh, shredded civilian shirts/jeans, exposed clavicles, open wounds with dried coagulated blood.
   * Motion: Unstable, dragging right leg, head tilted at unnatural angle.
2. **Runner (Tier 2):**
   * Spastic muscle spasms, twitching tendons, emaciated torso, blood-engorged red eyes, hyper-ventilating chest.
   * Motion: Low-slung, predatory sprints, erratic zig-zag pathing.
3. **Stalker (Tier 3):**
   * Dark grey, almost charred skin tone, prolonged claws (distended calcified fingernails), sunken spine.
   * Motion: Low crouch, silent wall-skirting, leap attacks.
4. **Brute (Tier 4):**
   * Massive mutated muscle hypertrophy, ruptured skin with exposed osseous armor plates, scarred cranium.
   * Motion: Heavy lumbering stomp, earth-shaking charge, ground pound.
5. **Alpha Boss (Tier 5):**
   * Massive asymmetric infection mutation. Left arm transformed into a giant ossified club with pulsating red veins.
   * Motion: Aggressive battle arena dominance, roar shockwave, area-of-effect sweep attacks.

---

## 4. HUMAN-COMPATIBLE RIG ARCHITECTURE

Each character model will use standard Unity Humanoid bone mapping:
```csharp
HumanDescription {
    human = new HumanBone[] {
        new HumanBone { boneName = "Hips", humanName = "Hips" },
        new HumanBone { boneName = "Spine", humanName = "Spine" },
        new HumanBone { boneName = "Chest", humanName = "Chest" },
        new HumanBone { boneName = "Neck", humanName = "Neck" },
        new HumanBone { boneName = "Head", humanName = "Head" },
        new HumanBone { boneName = "LeftUpperArm", humanName = "LeftUpperArm" },
        new HumanBone { boneName = "LeftLowerArm", humanName = "LeftLowerArm" },
        new HumanBone { boneName = "LeftHand", humanName = "LeftHand" },
        // ... all standard 55 humanoid bones
    }
}
```
This guarantees 100% interoperability with Mixamo, Cascadeur, and Unity Mecanim animation clips without manual retargeting.
