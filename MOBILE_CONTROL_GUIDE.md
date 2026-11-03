# ECLIPSE: THE LAST CIVILIZATION
## Mobile Touch Controls & Ergonomics Architecture

**Layout:** Landscape Mode  
**Architecture:** Dual-zone touch isolation (Left: Locomotion, Right: Camera & Combat Actions)

---

### 1. Control Mapping

| Screen Region | Component | Behavior | Desktop Key Equivalent |
|---|---|---|:---:|
| **Left Zone** | Dynamic Virtual Joystick | 360-degree analog locomotion with 10% deadzone protection. | `W`, `A`, `S`, `D` / Arrows |
| **Right Zone (Upper/Center)** | Swipe Drag Canvas | Independent pitch & yaw camera rotation with sensitivity scaling. | Mouse Movement |
| **Right Action: Fire** | `_fireButton` | Continuous hold or tap for firearm discharge / melee swing. | Left Mouse Button |
| **Right Action: Aim** | `_aimButton` | Hold to transition camera into over-the-shoulder / ADS view. | Right Mouse Button |
| **Right Action: Reload** | `_reloadButton` | Tap to trigger magazine reload cycle. | `R` Key |
| **Right Action: Sprint** | `_sprintButton` | Hold to sprint (consumes stamina). | Left Shift |
| **Right Action: Switch** | `_switchWeaponButton` | Cycle equipped primary, secondary, and melee slots. | `Q` Key |
| **Right Action: Interact** | `_interactButton` | Contextual prompt to open doors, loot containers, read notes. | `E` Key |
| **Top Right: Inventory** | `_inventoryButton` | Open grid backpack and equipment menu. | `Tab` Key |
| **Top Right: Pause** | `_pauseButton` | Pause game, open settings, or return to main menu. | `Escape` Key |
| **Contextual: Revive** | `_reviveButton` | Cooperative revive hold when near a downed teammate. | `F` Key |

---

### 2. Notch & Safe-Area Adaptation
All UI elements are parented under a canvas containing [`SafeAreaFitter.cs`](file:///C:/Users/selva/.gemini/antigravity/scratch/eclipse-the-last-civilization/Assets/_Project/Scripts/Mobile/SafeAreaFitter.cs). This automatically queries `Screen.safeArea` and clamps the UI borders away from device notches, Dynamic Island cutouts, and bottom navigation pill bars on both Android and iOS.
