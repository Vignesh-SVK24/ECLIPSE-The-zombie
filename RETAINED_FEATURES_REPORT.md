# ECLIPSE: THE LAST CIVILIZATION
## RETAINED FEATURES REPORT

### 1. Verification of Retained Gameplay Capabilities
In strict adherence to the project cleanup mandate, zero core gameplay mechanics were altered, diminished, or rebuilt:

1. **Locomotion & Controller Systems:**
   - 360-degree free character rotation based on movement vector.
   - Smooth banking into turns (lean angle calculated from angular delta).
   - Speed profiles: Controlled Walk (3.6 m/s), Tactical Jog (5.4 m/s), Sprint (7.5 m/s), Crouch (2.0 m/s), ADS Strafe (2.4–3.0 m/s).
   - Dynamic floating mobile joystick with auto-sprint forward threshold (>82%).
   - Touch look swipe with sensitivity damping and multi-touch isolation.

2. **Procedural Skeletal Animation Rig:**
   - 16-joint procedural hierarchy: Root, Hips, Spine, Chest, Neck, Head, Shoulders, Upper Arms, Forearms, Weapon Socket, Thighs, Calves, Feet.
   - Weapon-specific holding stances: Two-handed M4 Tactical Carbine grip and 9mm Combat Pistol Weaver stance.
   - Procedural breathing idle, footstep bobbing, and recoil kick/pitch damping.

3. **Infected Zombie Population:**
   - 4 distinct zombie classes: Walker, Runner, Spitter, Brute Tank.
   - Procedural limp shambling, erratic neck spasms, and gaping jaw twitching.
   - Proximity-based AI chase, aggro tracking, and melee damage application.

4. **Combat & Arsenal:**
   - Semi-automatic 9mm Combat Pistol (15/60 ammo).
   - Fully automatic M4 Tactical Carbine (30/120 ammo).
   - Tactical raycast ballistics with headshot multiplier (2.5x) and blood particle emission.
   - Dynamic recoil kick and procedural muzzle flash lighting.

5. **Atmospheric Environment & Lighting:**
   - Post-apocalyptic Greyfall City boulevard with abandoned police cruiser (flashing red siren), civilian sedans, concrete barricades, watchtowers, and streetlights.
   - Dynamic Day/Night cycle toggle (Dusk vs. Midnight Survival).
   - Atmospheric fog and PBR material roughness/metalness tuning.
