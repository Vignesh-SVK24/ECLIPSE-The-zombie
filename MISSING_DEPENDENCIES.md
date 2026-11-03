# ECLIPSE: THE LAST CIVILIZATION
## Missing & Required Dependencies Analysis

This document outlines the dependencies identified during the Phase 1 Environment Audit that must be provisioned or configured for full C# Dev Kit, Python, Blender MCP, and Unity MCP integration.

---

### Priority 1: .NET SDK (Required for C# Dev Kit)
- **Status:** **NOT FOUND**
- **Impact:** While Unity provides its own Mono / CoreCLR runtime for game execution, Microsoft's **C# Dev Kit** (`ms-dotnettools.csdevkit`) and the **C# Language Server** (`ms-dotnettools.csharp`) require a modern .NET SDK (version 8.0 or 9.0) installed on the host machine to execute the Roslyn Language Server Protocol (LSP) service and provide IntelliSense, code navigation, and solution parsing.
- **Recommended Remediation:**
  - Automated/Winget:
    ```powershell
    winget install Microsoft.DotNet.SDK.8
    ```
  - Manual Download: Official installer from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Verification Command:**
  ```powershell
  dotnet --version
  ```

---

### Priority 2: Blender 4.2 LTS / 4.3 (Required for Blender MCP)
- **Status:** **NOT FOUND**
- **Impact:** Blender MCP integration requires an active or headless Blender installation capable of executing Python scripts and loading the Blender MCP server add-on.
- **Recommended Remediation:**
  - Automated/Winget:
    ```powershell
    winget install BlenderFoundation.Blender
    ```
  - Manual Download: Official installer from [https://www.blender.org/download/](https://www.blender.org/download/)
  - Target Path: `C:\Program Files\Blender Foundation\Blender 4.2\` (or Blender 4.3)
- **Verification Command:**
  ```powershell
  blender.exe -v
  ```

---

### Priority 3: Unity MCP Bridge (Unity Editor Side)
- **Status:** **NOT CONFIGURED**
- **Impact:** Antigravity communicates with Unity via a Model Context Protocol server. For Antigravity to inspect game objects, scenes, console logs, and materials, the Unity Editor requires a companion package or Editor script that exposes a local communication endpoint (typically localhost WebSocket/HTTP/IPC).
- **Available Architectures:**
  1. Unity MCP Server (`unity-mcp` or `Unity-MCP-Bridge`): A lightweight Unity Editor bridge package added to `Assets/Editor/MCP` or `Packages/manifest.json`.
  2. Local MCP Server bridge running via Node.js or Python communicating with the Unity Editor API.
- **Action Required:** Package integration and local port binding configuration.

---

### Priority 4: Python Path Aliasing
- **Status:** **INSTALLED (Manual Path Mapping Required)**
- **Detail:** Python 3.11.4 is installed at `C:\Users\selva\AppData\Local\Programs\Python\Python311\python.exe`. However, the generic `python` command in PowerShell defaults to the broken Windows App Store shim (`%LOCALAPPDATA%\Microsoft\WindowsApps\python.exe`).
- **Remediation:**
  - Configure Antigravity `settings.json` with `python.defaultInterpreterPath`:
    `"C:\\Users\\selva\\AppData\\Local\\Programs\\Python\\Python311\\python.exe"`
  - Create dedicated virtual environments inside `Tools/Python/.venv` using this interpreter.

