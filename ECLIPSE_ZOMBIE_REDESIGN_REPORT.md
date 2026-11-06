# ECLIPSE: THE LAST CIVILIZATION
## ZOMBIE REDESIGN TECHNICAL REPORT — THE FOUR INFECTED CLASSES

**Document Version:** 2.0.0-PRO  
**Author:** Creature Artist & Character Technical Lead  
**Scope:** Walker, Runner, Stalker, Brute Specifications (Walker Approval Prototype)  

---

### 1. UNIFIED PATHOLOGY OF INFECTION

All infected entities in *ECLIPSE: THE LAST CIVILIZATION* descend from human hosts contaminated by the Eclipse bio-spore outbreak.
- **Dermal Deterioration:** Pale gray-green decaying flesh (`#566152`) with mottled desquamation, localized epidermal necrosis, and subcutaneous purple/black thrombosed vascular trees radiating outward from bite lacerations.
- **Facial Asymmetry:** Partially decayed labial tissue revealing broken, stained teeth, exposed jawbone fragments, and sunken orbital sockets.
- **Ocular Mutation:** Faint toxic green luminescence (`#45D422`) located deep inside the vitreous humor—resembling an active fungal mutation rather than an artificial lightbulb. Glow intensity is dialed down to 1.8 with natural pulse oscillations.

---

### 2. DETAILED BREAKDOWN OF THE FOUR CLASSES

#### Class 1: The Walker (Batch A Prototype Class)
- **Host Profile:** Infected civilians, paramedics, and patrol officers caught unprepared during the initial Greyfall quarantine collapse.
- **Physical Appearance:** Withered, hunched posture; shredded work clothing or torn jeans; exposed rib cage and forearm tissue; dragging left foot due to severed Achilles tendon.
- **Behavioral Audio & Motion:** Low guttural rattles, erratic limp-step gait, sudden aggressive forward lunges when within 4.5m of the survivor.
- **Combat Stats Preserved:** HP: 80 | Speed: 2.2 m/s | Damage: 18 | Headshot Multiplier: 2.5x.

#### Class 2: The Runner (Batch B Deployment)
- **Host Profile:** Young, athletic victims whose muscular vitality was hijacked by early-stage fungal contagion.
- **Physical Appearance:** Emaciated, sinewy musculature, frantic breathing, torn tracksuits or running shorts, heavily clawed fingers.
- **Behavioral Audio & Motion:** Piercing vocal shrieks, high-velocity zigzag sprints (6.8 m/s), aggressive jumping tackle.
- **Combat Stats Preserved:** HP: 55 | Speed: 6.8 m/s | Damage: 14 | Headshot Multiplier: 2.0x.

#### Class 3: The Stalker (Batch B Deployment)
- **Host Profile:** Stealth-adapted subterranean mutations from the city subway tunnels.
- **Physical Appearance:** Cadaverous pale skin, nearly hairless, elongated spinal column, sharp calcified fingernails.
- **Behavioral Audio & Motion:** Silent crawling, wall and barricade stalking, pounce ambush from shadow zones.
- **Combat Stats Preserved:** HP: 65 | Speed: 4.8 m/s | Damage: 26 | Headshot Multiplier: 2.2x.

#### Class 4: The Brute (Batch B Deployment)
- **Host Profile:** Heavily built industrial dockworkers and riot police mutated by massive spore density.
- **Physical Appearance:** Huge asymmetrical torso, calcified bone plate shoulders, torn heavy cargo overalls, massive clubbed fists.
- **Behavioral Audio & Motion:** Earth-shaking roar, heavy marching steps, unstoppable forward charge, ground-slam shockwave.
- **Combat Stats Preserved:** HP: 380 | Speed: 3.2 m/s | Damage: 45 | Headshot Multiplier: 1.5x (Armor Plating).

---

### 3. PROTOTYPE WALKER MATERIAL CONFIGURATION

```csharp
// Realistic Decayed Skin Shader Parameters
_BaseColor          = new Color(0.34f, 0.38f, 0.32f, 1.0f); // Gritty olive gray
_VeinColor          = new Color(0.20f, 0.14f, 0.22f, 1.0f); // Dark thrombosed purple
_ShadowColor        = new Color(0.12f, 0.13f, 0.11f, 1.0f); // Cold dark occlusion
_EyeGlowColor       = new Color(0.27f, 0.83f, 0.13f, 1.0f); // Faint fungal ocular glow
_EyeGlowIntensity   = 1.8f;                                  // Reduced from previous 4.5f
_Smoothness         = 0.32f;                                 // Damp decayed organic surface
```
