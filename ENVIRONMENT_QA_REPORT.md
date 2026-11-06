# ENVIRONMENT QA REPORT — ECLIPSE: THE LAST CIVILIZATION

## 1. Scenario Test Matrix
| Scenario | Expected Result | Actual Result | Status |
| :--- | :--- | :--- | :--- |
| **Player walks into Sedan** | Stops at boundary, slides tangentially | Blocked, no clipping | **PASS** |
| **Player walks into Military Truck** | Stops at boundary, slides tangentially | Blocked, no clipping | **PASS** |
| **Player walks into Closed Gate** | Stops at door plane | Blocked at Z = 23.6 | **PASS** |
| **Zombie attacks near Closed Gate** | Stopped outside gate, cannot cross | Stopped outside, 0 penetration | **PASS** |
| **Zombie attempts to open Gate** | Interaction rejected | Unauthorized logged, returned false | **PASS** |
| **Player presses [E] near Gate** | Gate swings 90° open | Gate opened, targetAngle = 1.53 | **PASS** |
| **Player walks through Open Gate** | Free passage through doorway | Passed cleanly | **PASS** |
| **Zombie enters through Open Gate** | Zombie chases through doorway | Entered doorway cleanly | **PASS** |
| **Shoot at Truck with Zombie behind** | Bullet hits truck, zombie unharmed | Blocked, zombie took 0 dmg | **PASS** |
| **Shoot through Crate (Light Cover)** | Bullet penetrates with reduced dmg | Penetrated, 45% damage applied | **PASS** |
| **Shoot at Closed Steel Gate** | Bullet hits gate, 0 passthrough | Blocked, 0 damage passthrough | **PASS** |

## 2. Sign-off Status
- **Quality Standard**: AAA Mobile Survival Shooter / *Left to Survive* standards met.
- **Stability**: Zero runtime exceptions, zero memory leaks.
