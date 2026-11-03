# ECLIPSE: THE LAST CIVILIZATION
## AI Creative Tools Installation & Configuration Guide

**Studio:** SVK Creationz  
**Engine:** Unity 6 (6000.0.32f1 LTS)  
**Document Ref:** `ECLIPSE_TOOL_INSTALLATION_GUIDE.md`  

---

### 1. Overview & Setup Prerequisites

This guide provides step-by-step instructions for configuring each AI-assisted creative tool for *ECLIPSE: THE LAST CIVILIZATION*.

```
PREREQUISITES INSTALLED ON HOST:
• Windows 11 (Host Machine: SELVA)
• Unity 6 Editor (Installed at C:\Program Files\Unity\Hub\Editor\6000.6.7f1\)
• Python 3.11 Runtime (Verified in PATH)
• Node.js v24.13.1 (Verified in PATH)
• Git 2.45+ (Verified in PATH)
```

---

### 2. Environment Variables & Secret Management

> [!CAUTION]
> Never commit API keys or passwords to Git. All authentication tokens must be set as Windows User Environment Variables or loaded locally from a `.env.local` file that is ignored by Git.

#### Step 1: Create Local Environment Configuration
In the project root, create a file named `.env.local` (already added to `.gitignore`):

```bash
# ECLIPSE Local AI Toolchain Configuration (.env.local)
SCENARIO_API_KEY=your_scenario_api_key_here
SCENARIO_MODEL_ID=your_custom_eclipse_lora_model_id
MESHY_API_KEY=your_meshy_api_key_here
TRIPO_API_KEY=your_tripo_api_key_here
UNITY_CLOUD_PROJECT_ID=your_unity_cloud_project_id
```

#### Step 2: Alternatively Set in Windows PowerShell
```powershell
[System.Environment]::SetEnvironmentVariable('SCENARIO_API_KEY', 'your_key', 'User')
[System.Environment]::SetEnvironmentVariable('MESHY_API_KEY', 'your_key', 'User')
```

---

### 3. Unity Muse Package Installation

1. Open **Unity Hub** and load *ECLIPSE: THE LAST CIVILIZATION* with Unity 6.
2. In the top menu, navigate to **Window** > **Package Manager**.
3. In the Package Manager window, click the `+` dropdown and select **Add package by name...**:
   - For Muse Texture: Enter `com.unity.muse.texture` and click **Add**.
   - For Muse Sprite: Enter `com.unity.muse.sprite` and click **Add**.
4. In Unity Editor, go to **Edit** > **Project Settings** > **Services** > **Muse**, sign in with your Unity Cloud account, and ensure your organization seat is linked.

---

### 4. Scenario AI Unity Integration

1. Sign up for a free developer account at [Scenario.com](https://www.scenario.com).
2. Generate an API Key under **Account Settings** > **API Keys**.
3. Save the key in your `.env.local` file as `SCENARIO_API_KEY`.
4. To install the official Scenario Unity SDK:
   - In Unity Package Manager, click `+` > **Add package from git URL...**
   - Enter: `https://github.com/scenario-labs/scenario-unity-sdk.git`
5. Test the local Python automation script:
   ```bash
   python Tools/AI/scenario_client.py --test-connection
   ```

---

### 5. Meshy AI 3D Generation Pipeline

1. Register at [Meshy.ai](https://www.meshy.ai) and retrieve your API key from the Dashboard.
2. Save the key in your `.env.local` as `MESHY_API_KEY`.
3. Test generating a stylized wooden barricade via the CLI tool:
   ```bash
   python Tools/AI/meshy_client.py \
       --prompt "Stylized post-apocalyptic wooden palisade barricade with barbed wire and red stencil" \
       --art-style "cartoon-realism" \
       --output "RawAssets/SM_Barricade_01.fbx"
   ```

---

### 6. Blender 4.2+ LTS Headless Processing Setup

1. If not already installed, download **Blender 4.2 LTS** from [blender.org](https://www.blender.org/download/).
2. Add the Blender directory (`C:\Program Files\Blender Foundation\Blender 4.2`) to your Windows `PATH` environment variable.
3. Verify in command prompt:
   ```bash
   blender --version
   ```
4. Run the ECLIPSE asset processor on any raw FBX:
   ```bash
   blender --background --python Tools/DCC/process_stylized_asset.py -- \
       --input "RawAssets/SM_Barricade_01.fbx" \
       --output "Assets/ECLIPSE/Environment/Prefabs/SM_Barricade_01.fbx" \
       --target-tris 3500
   ```
