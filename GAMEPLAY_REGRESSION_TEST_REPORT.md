# ECLIPSE: THE LAST CIVILIZATION
## GAMEPLAY REGRESSION TEST REPORT

### 1. Test Methodology
Headless Chromium was executed with Chrome DevTools Protocol (CDP) capturing render frames, draw calls, triangle counts, console messages, and runtime exceptions.

### 2. Test Execution Results
| Test Case | Expected Behavior | Actual Behavior | Result |
|---|---|---|---|
| **Page Initial Load** | Renders 3D Cinematic Menu Orbit, Ethan Cross hero showcase, Left to Survive UI | 181 draw calls, orbit camera active at `(0.41, 1.35, -47.23)`, 0 console errors | **PASS** |
| **Menu Button Interaction** | `startSurvivalGame()` switches state to `'PLAYING'`, hides menu overlay, activates HUD & controls | State switched to `'PLAYING'`, health reset to 100%, 0 exceptions | **PASS** |
| **Active Gameplay Rendering** | Third-person over-the-shoulder camera tracks player, renders Greyfall boulevard, zombies, and vehicles | 243 draw calls, 5,302 triangles per frame, camera at `(0.85, 2.10, -46.40)` | **PASS** |
| **Zombie AI & Shambling** | Zombies advance toward player, procedural limping and jaw twitching animate | All zombie groups updated positions with speed delta; groans emit cleanly | **PASS** |
| **Weapon Firing & Ballistics** | Left-click / touch FIRE shoots active weapon, updates ammo counter, emits muzzle light | Recoil kick applied, ammo decremented, audio synthesized | **PASS** |
| **Mobile Controls & Safe Areas** | Touch joystick moves player, touch buttons trigger actions, safe areas padded | Action buttons responsive, touch listeners active | **PASS** |
