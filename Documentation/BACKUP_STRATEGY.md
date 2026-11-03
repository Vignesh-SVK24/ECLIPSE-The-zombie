# ECLIPSE: THE LAST CIVILIZATION
## Backup & Version Safety Strategy

### 1. Overview
To ensure complete project stability, prevent accidental data loss, and maintain a historical audit trail across the 19 stages of development, ECLIPSE implements a dual-layer backup policy:

1. **Version Control Snapshotting (Git):**
   - Clean `.gitignore` filtering ephemeral Unity folders (`Library/`, `Temp/`, `Obj/`, `Builds/`).
   - Atomic commits at the completion of each development stage.
   - Tagged release points (`stage-01-complete`, `stage-02-complete`, etc.).

2. **Automated Stage Snapshot Archiving:**
   - Standalone PowerShell utility `Tools/Backup/backup_project.ps1`.
   - Compresses essential project assets (`Assets/`, `Packages/`, `ProjectSettings/`, `Documentation/`) into timestamped zip archives located in `Tools/Backup/Archives/`.
   - Excludes heavy transient directories, preserving storage while providing instantaneous full-state rollback capabilities.

### 2. Manual Backup Execution
Run the following command from the project root or within PowerShell:
```powershell
.\Tools\Backup\backup_project.ps1 -StageTag "Stage01"
```

### 3. Recovery Procedure
1. Close Unity Editor.
2. If restoring to a prior stage, extract the desired zip from `Tools/Backup/Archives/` into the project root.
3. Delete the local `Library/` and `Temp/` directories to force a clean Unity compilation cache rebuild.
4. Launch Unity 6 to re-import assets with guaranteed integrity.
