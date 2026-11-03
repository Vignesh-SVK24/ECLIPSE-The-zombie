# ECLIPSE: THE LAST CIVILIZATION
## Master MCP & Development Tools Integration Plan

### Executive Overview
This document defines the technical roadmap for integrating **Microsoft C# Dev Kit**, **Python Automation Toolchain**, **Blender MCP**, and **Unity MCP** into Google Antigravity for *ECLIPSE: The Last Civilization*.

---

### Architecture & System Topology

```
+-----------------------------------------------------------------------------------------+
|                                  GOOGLE ANTIGRAVITY                                     |
|  +-------------------------+  +-------------------------+  +--------------------------+ |
|  | C# Dev Kit (OmniSharp)  |  | Python 3.11 Runtime     |  | MCP Client Orchestrator  | |
|  | - Roslyn LSP            |  | - Tools/Python/.venv    |  | - mcp_config.json        | |
|  | - Unity Solution Parser |  | - Asset Pipelines       |  |                          | |
|  +------------+------------+  +------------+------------+  +------------+-------------+ |
+---------------|----------------------------|----------------------------|---------------+
                |                            |                            |
                v                            v                            v
   +------------------------+   +------------------------+   +-------------------------+
   |   Unity 6 Project      |   |   Python Pipelines     |   |       MCP Servers       |
   |   ECLIPSE C# Codebase  |   |   - Textures / PBR     |   |   1. Unity MCP Server   |
   |   Assets/_Project/     |   |   - Asset Validation   |   |   2. Blender MCP Server |
   |                        |   |   - Automation         |   |   3. Existing Stitch    |
   +-----------+------------+   +------------------------+   +------------+------------+
               |                                                          |
               |                     Local IPC / JSON-RPC                 |
               +==========================================================+
```

---

### Phase 2: C# Dev Kit Integration Plan
1. **Target Extensions:**
   - `ms-dotnettools.csdevkit` (C# Dev Kit)
   - `ms-dotnettools.csharp` (C# Base Language Support)
   - `VisualStudioToolsForUnity.vstuc` (Visual Studio Tools for Unity)
2. **Configuration Steps:**
   - Configure `.vscode/settings.json` targeting the installed .NET SDK and Unity solution file.
   - Configure OmniSharp / Roslyn LSP formatting, code navigation, and syntax diagnostics.
   - Generate project `.sln` and `.csproj` structures via Unity headless editor batchmode command if not generated.
3. **Verification:**
   - Syntax validation, symbol navigation, and class definition lookups across `Assets/_Project/Scripts/`.

---

### Phase 3: Python Integration & Automation Framework Plan
1. **Target Extension:**
   - `ms-python.python` (Python extension)
2. **Environment Configuration:**
   - Setup project-isolated Python virtual environment at `Tools/Python/.venv` using `C:\Users\selva\AppData\Local\Programs\Python\Python311\python.exe`.
   - Configure `.vscode/settings.json` to link `python.defaultInterpreterPath`.
3. **Directory Structure:**
   ```
   Tools/Python/
   ├── .venv/
   ├── requirements.txt
   ├── AssetProcessing/
   │   └── __init__.py
   ├── TextureUtilities/
   │   └── __init__.py
   ├── ProjectAutomation/
   │   └── __init__.py
   ├── Validation/
   │   └── __init__.py
   └── Reporting/
       └── __init__.py
   ```
4. **Verification:**
   - Create and run an environment validation script verifying interpreter, libraries, and file I/O.

---

### Phase 4: Blender MCP Integration Plan
1. **Blender MCP Candidate Analysis:**
   - Inspect leading open MCP implementations for Blender:
     - `blender-mcp` (ahujas / ModelContextProtocol community): Uses a Python add-on running inside Blender on `localhost:9876` and a stdio/node bridge MCP server.
     - Direct Python-based Blender RPC MCP server.
2. **Installation Prerequisites:**
   - Install Blender 4.2 LTS or 4.3.
   - Register add-on inside Blender scripts directory (`%APPDATA%\Blender Foundation\Blender\<version>\scripts\addons`).
3. **Antigravity MCP Configuration:**
   - Add `blender` server definition to `C:\Users\selva\.gemini\antigravity\mcp_config.json` while preserving `stitch`.
4. **Verification:**
   - Connect to active/headless Blender, create test cube, inspect properties, clean up test geometry.

---

### Phase 5: Unity MCP Integration Plan
1. **Unity MCP Architecture:**
   - Unity MCP Server provides tools to query scene hierarchy, game objects, components, console logs, and materials.
   - Protocol: Local HTTP / WebSocket bridge server or STDIO process communicating with Unity Editor via Unity Editor Scripting API (`UnityEditor`).
2. **Unity-Side Package Setup:**
   - Add Editor bridge script under `Assets/_Project/Editor/MCP/` or as an embedded local package.
   - Bridge runs a lightweight local listener when the Unity Editor is active.
3. **Antigravity MCP Configuration:**
   - Register `unity` MCP server in `mcp_config.json`.
4. **Verification:**
   - Query Unity version (`6000.6.7f1`), active scene, camera, lighting, and console logs.

---

### Phase 6: Configuration Management & Safety Rules
1. **Preservation:**
   - Never overwrite existing servers in `mcp_config.json` (`stitch` must remain intact).
2. **Port Allocation:**
   - Blender MCP Bridge: `localhost:9876` (or configured port)
   - Unity MCP Bridge: `localhost:8080` / `localhost:8081`
3. **Credentials & Security:**
   - Strictly bind servers to `127.0.0.1`.
   - Never expose external ports or commit API tokens to version control.

---

### Phase 7: Verification Matrix & Acceptance Criteria
| Integration | Verification Metric | Status |
| :--- | :--- | :--- |
| **C# Dev Kit** | C# scripts parsed, syntax highlighted, IntelliSense operational | Pending .NET SDK |
| **Python** | Virtualenv active, test script executes cleanly from Antigravity | Ready to configure |
| **Blender MCP** | Scene inspected, test mesh created, modified, cleaned up | Pending Blender installation |
| **Unity MCP** | Scene hierarchy retrieved, console messages inspected | Ready to configure |
| **Combined Flow**| Blender test asset exported -> imported into Unity -> verified | Dependent on Phases 4 & 5 |

