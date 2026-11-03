# ECLIPSE: THE LAST CIVILIZATION
## Master UI Design System & Component Specification

**Studio:** SVK Creationz  
**Engine:** Unity 6 + URP  
**Framework:** Unity uGUI (with TextMeshPro SDF) & Responsive Mobile Layout Engine  
**Visual Style:** Stylized Post-Apocalyptic Tactical Survival UI  
**Document Ref:** `ECLIPSE_UI_DESIGN_SYSTEM.md`  

---

### 1. Design System Foundations

The ECLIPSE UI Design System provides a unified, reusable component architecture engineered specifically for landscape mobile gameplay (Android/iOS) and scalable desktop development.

#### A. Core Color Tokens
```css
/* Master UI Tokens */
--color-brand-red:        #D32F2F;  /* Critical alerts, logo stencil, danger */
--color-brand-amber:      #FFB300;  /* Stamina, interactive focus, gold XP */
--color-brand-green:      #00E676;  /* Toxic infection, medkit healing */
--color-brand-blue:       #1976D2;  /* Shielding, multiplayer ally marker */

/* Neutral Dark Surface Tokens */
--surface-panel-bg:       #14191B;  /* Deep gunmetal panel background (85% alpha) */
--surface-card-bg:        #1E2528;  /* Item card container plate */
--surface-border-bronze:  #8D6E63;  /* 1px-2px weathered bronze stroke */
--surface-border-gold:    #FFB300;  /* Focused / selected border stroke */

/* Typography Tokens */
--text-primary:           #ECEFF1;  /* High-contrast off-white body text */
--text-muted:             #90A4AE;  /* Secondary sub-headers and captions */
--text-highlight:         #FFD54F;  /* Important stat numbers and currency */
```

#### B. Canvas Scaler & Layout Rules
- **Reference Resolution:** `1920 x 1080` (Landscape 16:9 base).
- **Match Mode:** `Match Width Or Height` with a weight of `0.5`.
- **Safe Area Inset:** Controlled by [`SafeAreaFitter.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Mobile/SafeAreaFitter.cs), ensuring no UI elements clip under camera notches, speaker bars, or dynamic islands.

---

### 2. Reusable UI Component Library

```
Assets/_Project/Prefabs/UI/Components/
├── Buttons/
│   ├── Btn_Primary_Tactical.prefab     (Heavy beveled red/amber button with sound trigger)
│   ├── Btn_Secondary_Gunmetal.prefab   (Dark steel panel button with bronze border)
│   ├── Btn_Icon_Circular.prefab        (Touch action buttons for mobile HUD)
│   └── Btn_Tab_Header.prefab           (Top navigation tab with active indicator line)
├── Cards/
│   ├── Card_Inventory_Item.prefab      (Item icon, stack count, rarity border, stat bar)
│   ├── Card_Weapon_Preview.prefab      (Weapon blueprint, current level, attachment slots)
│   ├── Card_Survivor_Roster.prefab     (Survivor portrait, role icon, health status)
│   └── Card_Settlement_Building.prefab (Building illustration, upgrade level, resource cost)
├── Bars/
│   ├── Bar_Health_Curved.prefab        (Red segmented health meter with critical pulse)
│   ├── Bar_Stamina_Segmented.prefab    (Amber athletic stamina gauge)
│   └── Bar_Resource_Fill.prefab        (Wood, metal, fuel, medicine progress fills)
└── Modals/
    ├── Modal_Confirm_Prompt.prefab     (Backdrop blur, header, message, Confirm/Cancel)
    └── Tooltip_Item_Inspector.prefab   (Dynamic stat comparison, lore description)
```

---

### 3. Screen Specifications & Layout Blueprints

#### 3.1 Cinematic Main Menu (`UI_MainMenu`)
- **Background:** Real-time 3D vignette of Ethan Cross resting against a fortified palisade at sunset, breathing animation, flickering lantern, and distant city smoke.
- **Top Center:** Master distressed ECLIPSE logo with stencil red lettering and survivor silhouette.
- **Left Panel Navigation Stack:**
  - `CONTINUE` (Loads latest checkpoint in Greyfall County).
  - `NEW GAME` (Difficulty selection: Story, Survivor, Hardcore Nightmare).
  - `MISSIONS` (Opens county tactical operations map).
  - `SETTLEMENT` (Opens bastion dashboard).
  - `INVENTORY` (Opens gear loadout & character inspection).
  - `MULTIPLAYER CO-OP` (Opens Netcode room lobby).
  - `SETTINGS` (Graphics presets: Low, Medium, High, Custom, Audio, Controls).
  - `EXIT GAME`.

#### 3.2 In-Game HUD (`UI_InGameHUD`)
- **Top Left (Survivor Vitals):**
  - Character portrait icon with red border.
  - Curved Health Bar (Red, dynamically pulses below 25%).
  - Curved Stamina Bar (Amber, depletes on sprint/melee).
  - Micro icons for Hunger (bread loaf) and Thirst (water droplet) status.
- **Top Right (Threat Radar & Mission Tracker):**
  - Current objective text with diamond marker and distance meter.
  - Threat detection semicircle (flashes yellow when zombies detect sound; red when chasing).
- **Bottom Right (Tactical Weapon Card):**
  - Large stylized weapon silhouette (M4A1, Remington 870, Beretta M9, Combat Knife, Grenade).
  - Current magazine count / Reserve ammo in bold high-visibility numbers.
  - Fire mode indicator (AUTO / SEMI).
  - Quick weapon swap carousel buttons.
- **Center Screen:**
  - Dynamic crosshair with bloom-spread on fire and tightness clamp on ADS aiming.
  - Contextual interaction prompt (`[E]` / `[HAND TAP]` - "Loot Supply Crate").

#### 3.3 Inventory & Equipment Screen (`UI_InventoryScreen`)
- **Left Column (3D Character Inspection):**
  - Real-time rotating 3D preview of Ethan Cross with equipped armor, backpack, and weapon.
  - Equipment slots: Primary Weapon, Secondary Sidearm, Melee Weapon, Helmet, Tactical Vest, Backpack, Boots.
- **Center / Right (Backpack Grid):**
  - Tabbed categories: All, Weapons, Ammo, Meds, Consumables, Crafting Parts.
  - 24-slot grid with drag-and-drop support.
  - Item rarity color coding:
    - *Common:* Gunmetal Gray (`#9E9E9E`)
    - *Uncommon:* Forest Green (`#4CAF50`)
    - *Rare:* Cobalt Blue (`#2196F3`)
    - *Epic:* Royal Purple (`#9C27B0`)
    - *Legendary:* Burnished Gold (`#FFA000`)
- **Bottom Drawer (Item Detail Panel):**
  - Item name, lore text, sell/scrap value, stat changes (Damage, Fire Rate, Range, Weight).

#### 3.4 Weapon Upgrade & Workbench Screen (`UI_WeaponWorkbench`)
- 3D exploded view of weapon on workbench table.
- Modular attachment sockets:
  1. *Optics:* Iron Sights, Red Dot Holo, 4x Tactical ACOG Scope.
  2. *Muzzle:* Compensator, Flash Hider, Tactical Suppressor.
  3. *Magazine:* Standard 30-round, Extended 45-round, Quick-draw Mag.
  4. *Underbarrel:* Vertical Grip, Angled Foregrip, Tactical Flashlight.
- Stat comparison bars showing green positive delta and red negative delta.

#### 3.5 Settlement Bastion Dashboard (`UI_SettlementDashboard`)
- Stylized 3D interactive view of the fortified compound.
- **Header Resource Ticker:** Wood, Scrap Metal, Fuel, Medical Supplies, Clean Water, Rations.
- **Building Management Cards:**
  - *Watchtower (Defense):* Increases perimeter threat warning radius.
  - *Medical Clinic:* Accelerates survivor wound recovery.
  - *Workshop:* Unlocks advanced weapon blueprints and ammo crafting.
  - *Diesel Generator:* Supplies electrical power for automated floodlights.
  - *Water Tower ("HOPE LIVES"):* Increases settlement capacity and population morale.
- **Survivor Assignment:** Drag survivor cards into buildings to boost production.

#### 3.6 Mission Tactical County Map (`UI_MissionSelect`)
- Stylized topographical map of Greyfall County with pin locations:
  1. *Sector 4 — Downtown Evacuation Line* (Difficulty: Normal, Loot: Medical & Weapons).
  2. *Blackwood Forest Outpost* (Difficulty: Hard, Loot: Timber & Survival Rations).
  3. *Ironshield Military Checkpoint* (Difficulty: Very Hard, Loot: Heavy Munitions & Armor).
  4. *Ashen Industrial District* (Difficulty: Extreme, Loot: Fuel & Generator Parts).
  5. *ECLIPSE Research Laboratory* (Difficulty: Nightmare Boss Zone, Loot: Genetic Cure Data).

#### 3.7 Mobile HUD & Touch Controls (`UI_MobileHUD`)
- **Left Touch Zone:** Floating dynamic virtual joystick with auto-centering on finger touch.
- **Right Touch Zone:** Free-look swipe area with smooth acceleration curve.
- **Action Cluster (Ergonomically Arc-Arranged for Right Thumb):**
  - Large Primary Fire Button (styled with crosshair icon).
  - ADS Aim Button (toggles zoom perspective).
  - Sprint / Dash Button (running man silhouette).
  - Reload Button (circular ammo reload sweep).
  - Crouch / Slide Button.
  - Quick Weapon Switch Thumb-Wheel.
