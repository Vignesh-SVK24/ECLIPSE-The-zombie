# ECLIPSE: THE LAST CIVILIZATION
## Part 1 — Master Development Plan (Stages 01 – 19)

### Architecture Principles
- **Engine Target:** Unity 6 (6000.0.x LTS) with Universal Render Pipeline (URP).
- **Platform Priority:** Android & iOS (mobile-first architecture, touch ergonomics, battery/thermal efficiency), with scalable architecture for PC.
- **Code Standards:** Modular, decoupled event-driven systems (GameEventBus, state machines, scriptable configs), zero-garbage runtime loops, strong null safety.
- **Visuals:** PBR materials, mobile-scaled URP lighting & shadows, dynamic atmospheric environment.

---

### Phase Roadmap

| Stage | Title | Focus & Core Deliverables | Status |
|---|---|---|---|
| **01** | **Project Inspection & Backup Strategy** | Directory hierarchy, backup automation, documentation baseline, asset inventory setup. | **COMPLETE** |
| **02** | **Unity 6 & URP Configuration** | Package manifests, URP pipeline settings, mobile quality presets (Low/Medium/High), frame-rate manager. | **COMPLETE** |
| **03** | **Folder Architecture & Core Systems** | GameBootstrapper, GameManager, SceneLoader, GameStateManager, GameEventBus, ApplicationLifecycleManager. | Pending |
| **04** | **Initial Scenes & Scene Transitions** | 00_Bootstrap, 01_MainMenu, 03_Greyfall_Prototype scenes, async loading screen with progress feedback. | Pending |
| **05** | **Player Character & Movement** | Ethan Cross controller, CharacterController locomotion (Walk 2.2m/s, Run 4.0m/s, Sprint 5.8m/s, Crouch 1.3m/s), slopes, gravity. | Pending |
| **06** | **Animation Architecture** | Animator blend trees, 1D/2D locomotion states, smooth transitions, procedural turn/ground alignment. | Pending |
| **07** | **Third-Person Camera** | Over-the-shoulder orbital camera, collision avoidance raycasting, pitch/yaw clamps, smooth damping. | Pending |
| **08** | **First-Person Camera & Switching** | Eye-level FP camera, seamless transition controller, orientation sync, no model clipping. | Pending |
| **09** | **Mobile Input & Responsive Controls** | Left virtual joystick, right camera-look touch canvas, action buttons (Sprint, Jump, Crouch, Cam-switch), safe area adaptation. | Pending |
| **10** | **Greyfall City Prototype Environment** | Playable city block, roads, modular damaged buildings, road signs, rubble collision, navigation bounds. | Pending |
| **11** | **World Interaction System** | InteractableBase, InteractionDetector, raycast trigger, context prompt UI (doors, notes, switches). | Pending |
| **12** | **Day & Night System** | Directional sun/moon rotation, atmospheric ambient light grading, skybox color lerp, mobile performance safeguards. | Pending |
| **13** | **Weather Foundation** | WeatherManager (Clear, Cloudy, Light Rain, Heavy Rain, Fog), pooled rain particles, ambient audio hooks. | Pending |
| **14** | **Graphics & Lighting Foundation** | PBR materials (concrete, asphalt, rust, wood), Light Probes, Reflection Probes, mobile shadow cascade tuning. | Pending |
| **15** | **Main Menu & Gameplay HUD** | Dark charcoal/rust UI palette, responsive HUD (health/stamina bars, compass, interaction prompts, pause button). | Pending |
| **16** | **Settings & Local Saving** | SettingsManager (graphics, audio, camera sensitivity, mobile control sizing), JSON local persistence with validation. | Pending |
| **17** | **Audio Foundation** | AudioManager, multi-channel volume buses (Master, Music, SFX, Ambient), spatial audio triggers. | Pending |
| **18** | **Performance Optimization** | Mobile profiler tuning, occlusion culling settings, LOD group standards, draw call batching, memory audits. | Pending |
| **19** | **Final Validation & Bug Fixing** | End-to-end acceptance verification against prompt checklist, build configuration tests for Android & iOS. | Pending |
