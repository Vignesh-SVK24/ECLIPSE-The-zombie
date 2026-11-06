# ECLIPSE: THE LAST CIVILIZATION
# ZOMBIE ASSET IMPORT & DIRECTORY STRUCTURE REPORT
**Developer:** SVK Creationz  
**Target Engine:** Unity 6 (URP 17.0.3)  

---

## 1. Directory Structure

Following the project standards, the zombie character pipeline is organized under `Assets/ECLIPSE/Characters/Zombies/` and `Assets/_Project/`:

```
Assets/
├── ECLIPSE/
│   ├── Characters/
│   │   └── Zombies/
│   │       ├── Walker/
│   │       │   ├── Models/
│   │       │   ├── Materials/
│   │       │   ├── Textures/
│   │       │   ├── Animations/
│   │       │   └── Prefabs/
│   │       ├── Runner/
│   │       │   ├── Models/
│   │       │   ├── Materials/
│   │       │   ├── Textures/
│   │       │   ├── Animations/
│   │       │   └── Prefabs/
│   │       ├── Stalker/
│   │       │   ├── Models/
│   │       │   ├── Materials/
│   │       │   ├── Textures/
│   │       │   ├── Animations/
│   │       │   └── Prefabs/
│   │       └── Brute/
│   │           ├── Models/
│   │           ├── Materials/
│   │           ├── Textures/
│   │           ├── Animations/
│   │           └── Prefabs/
│   └── Textures/
│       └── Characters/
│           └── Zombies/
│               ├── T_Zombie_Walker_Albedo.png
│               ├── T_Zombie_Walker_ORM.png
│               ├── T_Zombie_Runner_Albedo.png
│               ├── T_Zombie_Brute_Albedo.png
│               └── T_Zombie_AlphaBoss_Albedo.png
└── _Project/
    ├── ScriptableObjects/
    │   └── AI/
    │       ├── Zombie_Walker.asset
    │       ├── Zombie_Runner.asset
    │       ├── Zombie_Stalker.asset
    │       ├── Zombie_Brute.asset
    │       └── Zombie_AlphaBoss.asset
    └── Scripts/
        └── AI/
            ├── ZombieController.cs
            ├── ZombieType.cs
            ├── ZombieTypeData.cs
            ├── ZombieMovement.cs
            ├── ZombieDetection.cs
            ├── ZombieHearing.cs
            ├── ZombieAnimationController.cs
            ├── ZombieAudioController.cs
            ├── ZombieGroupManager.cs
            └── ZombieSpawner.cs
```

---

## 2. PBR Material & Texture Association

| Asset | Shader | Albedo Map | ORM Mask Map | Emission Map | Emissive Color / Glow |
|---|---|---|---|---|---|
| **M_Zombie_Walker** | Universal Render Pipeline/Lit | `T_Zombie_Walker_Albedo.png` | `T_Zombie_Walker_ORM.png` | Procedural / Emissive Mask | `#2BF805` (Lime Green, Single Eye 3.5x) |
| **M_Zombie_Runner** | Universal Render Pipeline/Lit | `T_Zombie_Runner_Albedo.png` | Shared ORM | Procedural / Emissive Mask | `#00FF66` (Dual Eye 4.5x) |
| **M_Zombie_Stalker**| Universal Render Pipeline/Lit | Dark Desaturated Albedo | Shared ORM | Procedural / Emissive Mask | `#1AFF55` (Subtle Sockets 1.8x) |
| **M_Zombie_Brute**  | Universal Render Pipeline/Lit | `T_Zombie_Brute_Albedo.png` | Shared ORM | Procedural / Emissive Mask | `#2BF805` (Toxic Vein Glow 2.5x) |
| **M_Zombie_Alpha**  | Universal Render Pipeline/Lit | `T_Zombie_AlphaBoss_Albedo.png`| Shared ORM | Procedural / Emissive Mask | `#FF0033` & `#00FF66` (Bioluminescent 5.0x) |
