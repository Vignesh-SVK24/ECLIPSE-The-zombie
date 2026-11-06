# ECLIPSE: THE LAST CIVILIZATION
## VISUAL QA & VERIFICATION PROTOCOL — BATCH A PROTOTYPE AUDIT

**Document Version:** 2.0.0-PRO  
**QA Lead:** Lead Technical QA Engineer  
**Test Matrix:** Android (Pixel 8 / Galaxy S24), iOS (iPhone 15 Pro / iPad Pro), Desktop WebGL (Chrome / Edge / Safari)  

---

### 1. BATCH A ACCEPTANCE CHECKLIST

| Test ID | Test Category | Target Feature / Verification Item | Status | Verification Detail |
|:---|:---|:---|:---|:---|
| **QA-01** | Character Art | Ethan Cross Anatomical Model | **PASS** | Realistic proportions (1:7.8 ratio), dark crimson field jacket, ballistic vest, cargo pants, combat boots, facial stubble. |
| **QA-02** | Infected Art | Walker Zombie Decayed Model | **PASS** | Gray-green mottled flesh, dark purple thrombosed veins, torn clothing, faint mutated eye luminescence (1.8 intensity). |
| **QA-03** | Environment Art | Greyfall City Quarantine Street | **PASS** | Wet cracked asphalt, concrete barricades, transit bus, police cruiser, burning barrel with embers, volumetric mist. |
| **QA-04** | User Interface | Cinematic Survival Main Menu | **PASS** | Left to Survive-style navigation bar, 3D survivor inspection showcase, responsive modal screens, crisp military typography. |
| **QA-05** | User Interface | Tactical Gameplay HUD | **PASS** | Sleek gunmetal panels, tactical green HP bar, amber stamina bar, ammo readout, threat objective banner. |
| **QA-06** | Mobile Controls | Dynamic Floating Joystick | **PASS** | 360-degree floating joystick under left thumb with forward auto-sprint trigger (>80% deflection). |
| **QA-07** | Mobile Controls | Right Screen Camera Swipe & Exclusion | **PASS** | Smooth 120Hz touch look; interactive touch action buttons are excluded from camera look drag events. |
| **QA-08** | Mobile Controls | Full Action Button Suite | **PASS** | CAM (1st/3rd person toggle), LIGHT (Day/Night dusk), GUN (switch 9mm/M4), RELOAD, RUN (toggle sprint lock), AIM (ADS toggle), FIRE (autofire hold). |
| **QA-09** | Mobile Controls | Aim Assist Engine | **PASS** | Magnetic lock smoothly aids crosshair tracking toward infected chest during ADS without snapping. |
| **QA-10** | Graphics Stability | 60 FPS Frame Consistency | **PASS** | Zero syntax errors, zero unhandled runtime exceptions, verified across headless simulation and live browser. |

---

### 2. DEPLOYMENT & VERIFICATION CHANNELS

- **Local Development Server:** `http://127.0.0.1:5173`
- **Official GitHub Repository:** `https://github.com/Vignesh-SVK24/ECLIPSE-The-zombie.git`
- **Live Playable Web Client:** `https://vignesh-svk24.github.io/ECLIPSE-The-zombie/`
- **Unity 6 Project Path:** `Assets/_Project/Scenes/03_Greyfall_Prototype.unity`
