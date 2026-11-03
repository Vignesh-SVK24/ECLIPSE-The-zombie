# ECLIPSE: THE LAST CIVILIZATION
## Android Real-Device Validation Checklist & Report

**Test Application:** `ECLIPSE: THE LAST CIVILIZATION`  
**Package:** `com.svkcreationz.eclipse`  
**Target SDK:** Android API Level 34 (Android 14) / Min SDK 26 (Android 8.0)  
**Graphics API:** Vulkan (with OpenGLES3 fallback)

---

### Verification Checklist for Physical Phone Testing

| Item | Test Objective | Verification Procedure | Expected Result | Status |
|---|---|---|---|:---:|
| **1. Startup & Splash** | Cold app launch | Tap icon from phone launcher | Splash screen displays without black screen; launches within 3.5s | READY FOR TEST |
| **2. Main Menu UI** | Menu navigation | Tap New Game / Settings | Clean landscape rendering, text readable, buttons responsive | READY FOR TEST |
| **3. Virtual Joystick** | Left thumb locomotion | Drag joystick 360 degrees | Accurate forward/back/strafe movement without axis inversion | READY FOR TEST |
| **4. Camera Swipe** | Right thumb rotation | Swipe across right-side screen | Smooth look rotation without jitter; does not accidentally fire weapon | READY FOR TEST |
| **5. Multitouch** | Simultaneous inputs | Left thumb moving + Right thumb aiming + Index firing | All 3 inputs register concurrently without dropping packets | READY FOR TEST |
| **6. Safe Area** | Camera notch padding | Test on phone with punch hole / notch | Controls offset cleanly away from screen edges & camera cutout | READY FOR TEST |
| **7. Combat Actions** | Fire, Aim, Reload | Press combat action buttons | Firing animations trigger; ammunition decreases; reload cycles | READY FOR TEST |
| **8. Performance / FPS** | Thermal & framerate stability | Play in Greyfall sector for 10 minutes | Stable 30 FPS on low preset, 60 FPS on high preset; zero thermal crash | READY FOR TEST |
| **9. Minimization / Resume** | Android lifecycle | Press Home button, reopen app from recent apps | Application resumes cleanly without black screen or audio freeze | READY FOR TEST |
| **10. Pause & Exit** | In-game pause | Tap Pause button, exit to main menu | Time stops; menu displays; cleanly returns to title screen | READY FOR TEST |
