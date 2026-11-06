# DOOR INTERACTION REPORT — ECLIPSE: THE LAST CIVILIZATION

## 1. System Overview
- **Location**: Military Quarantine Checkpoint Boulevard (`X: 0, Y: 0, Z: 24`).
- **Structure**: Reinforced 3.4m steel security gate with yellow hazard chevrons, heavy posts, and latch handle.
- **Hinge Pivot**: Left post anchor at `(-1.7, 0, 24)`. Swings smoothly 0° to 90° (1.57 rad).

## 2. Player-Only Security Protocol
- **Strict Authorization Check**:
  ```javascript
  function interactDoor(entity) {
      if (entity !== player) {
          console.warn('Entity unauthorized: Only the player can interact with doors.');
          return false;
      }
      // Toggle door state...
  }
  ```
- **Zombie Restriction**: Zombies possess ZERO door interaction privileges. Automated tests verified that zombie interaction requests are rejected with `false` and logged as unauthorized.

## 3. State Behavior Matrix
| State | Player Movement | Zombie Movement | Bullet Line-of-Sight | HUD Prompt |
| :--- | :--- | :--- | :--- | :--- |
| **CLOSED** | Blocked | Blocked (Zombies gather outside) | Blocked (Heavy Cover) | `[E] OPEN SECURITY GATE` |
| **OPEN** | Free passage | Free passage (Zombies enter) | Free passage through doorway | `[E] CLOSE SECURITY GATE` |

## 4. UI & Controls
- **Desktop**: Proximity check (< 3.2m) displays floating HUD banner: `[E] OPEN/CLOSE SECURITY GATE`. Pressing `E` triggers gate.
- **Mobile**: Dynamically displays [GATE [E]] touch button in touch-action-buttons. Single tap toggles gate with haptic feedback.
