# ECLIPSE: THE LAST CIVILIZATION
# FOUR-CLASS ZOMBIE DESIGN SPECIFICATION
**Developer:** SVK Creationz  
**Target Engine:** Unity 6 (URP 17.0.3)  
**Game Genre:** Open-world zombie survival, action, horror & adventure  

---

## 1. Class Comparison Matrix

| Attribute | Class 01: Walker | Class 02: Runner | Class 03: Stalker | Class 04: Brute | Elite Encounter: Alpha Boss |
|---|---|---|---|---|---|
| **Role** | Common Pressurer | High-Speed Pursuer | Stealth Ambush Hunter | Heavy Wall / Tank | Special Horde Encounters |
| **Threat Tier** | Low (High in Swarms) | High (Agile) | Very High (Lethal) | Extreme (Destructive) | Catastrophic Boss |
| **Health Multiplier** | 1.0x (80 HP) | 0.7x (55 HP) | 0.9x (70 HP) | 4.5x (350 HP) | 7.0x (550+ HP) |
| **Damage Multiplier**| 1.0x (12 DMG) | 1.5x (18 DMG) | 2.0x (24 DMG) | 3.5x (45 DMG) | 4.5x (55 DMG) |
| **Movement Speed** | Walk: 1.2 / Chase: 2.6 | Walk: 2.2 / Chase: 5.6 | Walk: 1.6 / Burst: 4.5 | Walk: 1.0 / Charge: 3.2| Walk: 1.8 / Enrage: 4.8 |
| **Eye Mutation** | Single Eye (Pale Lime) | Dual Eyes (Vivid Lime) | Dual Faint Sockets | Deep Green Vein Flare | Crimson / Toxic Dual Flare |
| **Primary Attack** | Claw & Bite | Leaping Lunge | Blindspot Neck Gash | Heavy Cleave & Ground Slam | Arm Sweep & Shockwave |
| **Weak Point** | Cranium (Headshot 2.5x)| Head & Kneecaps | Exposed Spine / Neck | Exposed Back & Arm Gaps | Head & Chest Mutation Core |
| **Spawning Bias** | Streets, Apartments | Highways, Open Lots | Dark Interiors, Bunkers| Military Bases, Factories | Major Story Encounters |

---

## 2. Deep Class Profiles

### 2.1 Class 01 — Walker (Common Infected)
- **Concept:** Everyday survivors, workers, and civilians who succumbed during the early outbreak weeks. The infection has degraded their neuromuscular coordination, leaving them in a persistent stumbling crawl.
- **Physical Details:**
  - Average human build, slightly slumped spine.
  - Torn civilian jacket over faded shirt, frayed cargo trousers, single scuffed boot with exposed decaying ankle.
  - Dropped jaw with broken teeth, dried dark brown blood around mouth and throat.
  - Left arm shows an exposed ulna bone fragment poking through torn sleeve fabric.
  - Left eye glows with a subtle lime-green luminescence; right eye is milky and clouded.
- **Combat Flow:**
  - Approaches in shambling clusters; utilizes numerical advantage to pin the player against walls or cars.
  - Low individual health makes them ideal targets for conservation of pistol ammunition or melee execution.
- **Appearance Presets:**
  1. *Civilian Walker:* Canvas coat, denim jeans, scuffed sneakers.
  2. *Former Security Walker:* Faded navy guard shirt, badge clip, duty belt.
  3. *Military Walker:* Torn digital camo blouse, dog tag chain, combat boots.
  4. *Industrial Worker Walker:* High-visibility vest remnants, grime-stained coveralls.
  5. *Injured Walker:* Head bandage fragments, compound arm fracture, dragging limp.

---

### 2.2 Class 02 — Runner (Agile Hunter)
- **Concept:** Infected whose viral strain accelerated motor-neuron excitation and adrenaline output. Muscles and tendons remain tightly wound, operating under continuous manic spasm.
- **Physical Details:**
  - Lean, wiry anatomy with visible striated sinew along arms and calves.
  - Torn athletic wear or shredded lightweight civilian clothing; barefoot or wearing single ruined running shoe.
  - Dual glowing eyes burning with high-intensity toxic green luminescence.
  - Skin pulled taut across cheekbones; erratic breathing and twitching neck muscles.
- **Combat Flow:**
  - Sprints aggressively in zig-zag trajectories upon hearing gunfire or seeing the player.
  - Closes distance in seconds; executes a leaping lunge that knocks the player off balance.
  - Short 1.0-second recovery pause after a missed lunge provides the player's primary counter-attack window.
- **Appearance Presets:**
  1. *Civilian Runner:* Hoodless windbreaker, gym shorts, exposed corded muscle.
  2. *Former Athlete Runner:* Track jersey with faded number, knee brace.
  3. *Infected Security Runner:* Tactical t-shirt, torn radio harness.
  4. *Injured Runner:* Bloodied leg torn by razor wire, relentless pace despite trauma.

---

### 2.3 Class 03 — Stalker (Shadow Ambush Hunter)
- **Concept:** Pathogen has caused spinal curvature and extreme auditory sensitivity. Stalkers retreat from bright light and use vertical environments, blind corners, and shadow corridors to stalk prey silently.
- **Physical Details:**
  - Elongated, slender silhouette; spine hunched forward into a predatory arch.
  - Charcoal-gray, damp skin with deep purple-black subdermal vein networks.
  - Deep-set orbital sockets with intermittent bioluminescent eye glimmers that briefly reflect light.
  - Dark, tattered hooded jacket or dark canvas raincoat blending seamlessly into grime and shadows.
- **Combat Flow:**
  - Stays outside direct line of sight; stalks behind cover (dumpsters, cars, pillar corners).
  - Silent approach until within 4 meters, followed by a terrifying shriek and rapid pounce to the player's blind spot.
  - Retreats back into shadow or around obstacles if the player targets it directly before pouncing.
- **Appearance Presets:**
  1. *Civilian Stalker:* Torn dark hoodie, frayed black trousers.
  2. *Research Facility Stalker:* Corrupted laboratory coat remnants, chemical burns.
  3. *Industrial Stalker:* Grease-soaked mechanic jumpsuit, utility belt.
  4. *Damaged Stalker:* Scalp lacerations, barbed-wire snagged around torso.

---

### 2.4 Class 04 — Brute (Heavy Mutated Enforcer)
- **Concept:** Rare cellular gigantism caused by secondary pathogen mutation interacting with pre-existing body mass and tactical body armor.
- **Physical Details:**
  - Enormous upper-body mass (2.2x normal human chest breadth, 1.8x height).
  - Thick neck muscles fused with shoulder trapezius; heavy protruding brow and fractured jaw.
  - Dark moss-green necrotic flesh embedded with osseous calcified bone plates along back and forearms.
  - Wears remnants of military Kevlar vest, reinforced combat harness, or heavy industrial steel-toe boots.
- **Combat Flow:**
  - Moves with deliberate, heavy stomping footsteps that shake nearby loose props.
  - Unyielding poise: ordinary pistol rounds do not stagger a Brute.
  - Attacks include a 180° horizontal arm sweep, an unblockable shoulder charge that knocks the player backward, and a two-handed ground slam.
  - Can demolish temporary wooden barricades and player barricade defenses.
- **Appearance Presets:**
  1. *Military Brute:* Ballistic plate carrier fragments, torn camo trousers, steel boots.
  2. *Industrial Brute:* Heavy leather welding apron, iron chain wrap on forearm.
  3. *Armoured Brute:* Riot shield remnants strapped to left shoulder, helmet bracket.
  4. *Mutated Brute:* Massive calcified bone spur protruding from right elbow, ruptured skin.

---

### 2.5 Rare Elite Encounter — The Alpha Boss
- **Concept:** Unique apex carrier at the heart of major Greyfall County outbreak hotspots.
- **Physical Details:**
  - Colossal scale (2.2m height); asymmetric hypertrophic left arm ending in massive calcified talons.
  - Chest cracked open with glowing toxic core and pulsing bio-luminescent pustules.
  - Dual glowing eyes blazing with crimson and emerald flare.
- **Multi-Phase Gameplay:**
  - *Phase 1 (100% - 50% HP):* Controlled devastating sweeps, heavy shockwaves, and summon roars that call nearby Walkers and Runners.
  - *Phase 2 (Enraged, < 50% HP):* Alpha roars violently, movement speed increases by 45%, attack cooldown drops to 0.8s, and gains ground-rupturing fissure attacks.
