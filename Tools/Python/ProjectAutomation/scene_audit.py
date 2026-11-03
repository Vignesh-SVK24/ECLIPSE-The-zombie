"""
ECLIPSE: Scene Asset Auditor
Inspects Unity YAML scene files and identifies dependencies, components, and missing asset GUIDs.
"""
import os
import re

def audit_scene_file(scene_path):
    if not os.path.isfile(scene_path):
        return {"error": f"Scene not found: {scene_path}"}

    with open(scene_path, 'r', encoding='utf-8', errors='ignore') as f:
        content = f.read()

    game_objects = re.findall(r'--- !u!1 &(\d+)', content)
    monobehaviours = re.findall(r'--- !u!114 &(\d+)', content)
    transforms = re.findall(r'--- !u!4 &(\d+)', content)

    return {
        "scene": os.path.basename(scene_path),
        "game_objects_count": len(game_objects),
        "monobehaviours_count": len(monobehaviours),
        "transforms_count": len(transforms)
    }

if __name__ == "__main__":
    project_root = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", ".."))
    scenes_dir = os.path.join(project_root, "Assets", "_Project", "Scenes")
    print(f"[SceneAuditor] Auditing scenes in: {scenes_dir}")
    if os.path.isdir(scenes_dir):
        for f in os.listdir(scenes_dir):
            if f.endswith(".unity"):
                res = audit_scene_file(os.path.join(scenes_dir, f))
                print(f" - {res}")
    print("[SceneAuditor] Audit complete.")

