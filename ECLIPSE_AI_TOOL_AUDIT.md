# ECLIPSE: THE LAST CIVILIZATION
## Phase 1 — Comprehensive AI Toolchain & Unity 6 Integration Audit

**Studio:** SVK Creationz  
**Engine:** Unity 6 (6000.0.32f1 LTS / Host Installed: 6000.6.7f1)  
**Render Pipeline:** Universal Render Pipeline (URP 17.0.3)  
**Platform Focus:** Android & iOS (Primary), PC Development  
**Document Ref:** `ECLIPSE_AI_TOOL_AUDIT.md`  

---

### 1. Executive Summary & Audit Scope

This document provides a rigorous technical assessment of AI-assisted creative tools, official Unity AI packages, MCP integrations, generative 3D platforms, and DCC workflows for *ECLIPSE: THE LAST CIVILIZATION*. 

The objective is to establish an accelerated, high-fidelity production pipeline that transforms the entire game into a **cohesive, premium stylized 3D animated cartoon-realism** aesthetic (matching the master concept key art) without introducing unvetted dependencies or risking runtime instability.

---

### 2. Unity Official AI Tools & Services Audit

#### A. Unity Muse Suite (Unity 6 Compatible)
Unity Muse is Unity's official suite of runtime and editor-based generative AI capabilities.

| Tool / Service | Package / Access | Status | Technical Role & Suitability for ECLIPSE | Account / Pricing Model |
| :--- | :--- | :---: | :--- | :--- |
| **Unity Muse Chat** | Unity Cloud / Package Manager | **Available** | In-editor contextual coding assistant, URP shader debugging, and scene inspection. | Requires Unity Cloud account & Muse subscription ($30/mo per seat). |
| **Unity Muse Texture** | `com.unity.muse.texture` | **Available** | Generates tileable PBR texture sets (Albedo, Normal, Height, Smoothness) directly in the editor. Suitable for stylized hand-painted concrete, wood planks, and brick facades. | Credit-based generation (included in Muse tier). |
| **Unity Muse Sprite** | `com.unity.muse.sprite` | **Available** | Generates 2D UI elements, item icons (medkits, ammo boxes, rations), and HUD decals in clean vector-like stylized art style. | Credit-based. |
| **Unity Muse Animate** | Unity Cloud Web / Editor Bridge | **Available** | Text-to-motion generative skeletal animations. Generates humanoid idle loops, staggered zombie walks, and survivor aim offsets. | Cloud-processed, exported to FBX humanoid format. |
| **Unity Muse Sound** | `com.unity.muse.sound` | **Available** | Generates short sound effects (debris rustling, bullet casings hitting pavement, zombie snarls). | Credit-based. |

#### B. Unity Sentis (On-Device Neural Engine)
- **Package:** `com.unity.sentis` (`2.1.0` for Unity 6).
- **Capability:** Runs quantized ONNX neural network models directly on mobile GPUs/CPUs at runtime.
- **Application in ECLIPSE:** Can be deployed for on-device advanced procedural zombie flocking and contextual voice command recognition for survivor companions without requiring cloud API calls.
- **Cost:** Free, fully local on-device execution.

#### C. Unity MCP (Model Context Protocol) & AI Gateway
- **Unity AI Gateway:** Unity's enterprise service connecting external LLMs to the editor.
- **Unity MCP Server:** Evaluated against our installed MCP servers (`StitchMCP`, `firebase-mcp-server`). An official Unity MCP server requires running a local WebSocket bridge between Antigravity and the Unity Editor instance (`com.unity.mcp` or `UnityWebSocketBridge`). 
- **Recommendation:** Implement a lightweight local bridge script in `Tools/AI/` to execute headless commands, automated texture imports, and asset validation scripts.

---

### 3. Scenario AI (Visual Consistency & Art Direction Engine)

Scenario is identified as the **primary visual consistency engine** for *ECLIPSE: THE LAST CIVILIZATION*.

- **Capabilities:** Allows training custom LoRA / Style Models on approved concept art (such as our newly established master key art).
- **Pipeline Integration:**
  - Survivor Character Concepts (Ethan Cross, Sarah Mitchell, Marcus Vance, Elena Rostova).
  - Zombie Variations (Walker, Runner, Stalker, Brute, Alpha Boss).
  - 2D Inventory Iconography (consumables, crafting parts, tactical attachments).
  - Loading Screens & Cinematic Storyboards (Greyfall City, Blackwood Forest, Ironshield Base).
- **Unity Integration:** Scenario provides the `Scenario Unity SDK` (`com.scenario.unity` via Git URL or REST API).
- **API Safety:** API keys are stored strictly in local environment variables (`SCENARIO_API_KEY`) and `.env.local` (never committed to Git).
- **Pricing:** Free trial available; standard tier ($29/mo) provides unlimited model training and high-resolution generation.

---

### 4. Generative 3D Asset Platforms: Meshy AI vs Tripo AI

| Dimension | Meshy AI (Meshy-4) | Tripo AI (Tripo v2.5) | ECLIPSE Production Recommendation |
| :--- | :--- | :--- | :--- |
| **Primary Specialty** | Hard-surface props, barricades, vehicles, survival gear, and PBR texturing. | Rapid organic character sculpting and stylized creature geometry. | **Dual Pipeline:** Use Meshy for environment props/barricades; use Tripo for organic zombie body variations. |
| **Mesh Quality** | Quad-dominant or clean triangulated meshes, controllable poly counts (5K–25K). | Watertight triangulated meshes (10K–30K), fast preview. | High quality; requires Blender pass for LOD generation. |
| **PBR Textures** | Generates Albedo, Normal, Roughness, and Metallic maps. | Embedded vertex colors or single Albedo atlas. | Meshy maps align directly with Unity URP standard PBR. |
| **Unity Integration** | Official Meshy Unity Plugin (`Meshy-Unity-Plugin`) or direct GLTF/FBX drag-and-drop. | Tripo Unity SDK or REST API. | Direct FBX ingestion with automated import post-processors in `Assets/_Project/Editor/`. |
| **Pricing** | Free tier (200 credits/mo); Pro ($16/mo). | Free tier (300 credits/mo); Professional ($20/mo). | Utilize free tier for initial prototype assets; do not purchase subscriptions without explicit approval. |

---

### 5. Blender 4.2+ LTS Integration & Optimization Pipeline

Blender serves as the mandatory bridge and quality firewall between AI-generated raw geometry and the Unity 6 project:

```
[AI Generator: Meshy / Tripo]
             │ (Raw FBX / GLTF)
             ▼
[Blender 4.2 Automation Script: `Tools/DCC/process_stylized_asset.py`]
             │
             ├── 1. Mesh Cleanup (Remove non-manifold geometry, interior degenerate faces)
             ├── 2. Retopology (Quad Remesh / Decimate to mobile budget: 1.5K - 6K tris)
             ├── 3. UV Unwrapping (Non-overlapping Lightmap UV Channel 1 with 4px padding)
             ├── 4. Material Channel Remapping (Pack into ECLIPSE ORM Mask standard)
             ├── 5. LOD Generation (LOD0: 100%, LOD1: 50%, LOD2: 20%)
             └── 6. Collision Hull Generation (Export `UCX_` convex collision meshes)
             │
             ▼
[Unity 6 URP Asset Ingestion (`Assets/ECLIPSE/Environment/`)]
```

---

### 6. Toolchain Readiness & Action Plan

1. **Unity Packages to Verify/Add:**
   - `com.unity.shadergraph` (17.0.3)
   - `com.unity.visualeffectgraph` (17.0.3)
   - `com.unity.splines` (2.6.1)
   - `com.unity.adaptiveperformance` (5.1.0)
2. **AI Connector Scripts to Deploy:**
   - `Tools/AI/scenario_client.py`: Python CLI for batch generation of UI icons and textures using Scenario REST API.
   - `Tools/AI/meshy_client.py`: Python CLI for queuing 3D prop generation (barricades, sandbags, ammo crates).
   - `Tools/DCC/process_stylized_asset.py`: Automated Blender headless processor for cleanup and LOD export.
