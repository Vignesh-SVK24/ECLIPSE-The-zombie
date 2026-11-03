"""
ECLIPSE: Asset Organizer
Standardizes directory structures, file naming, and metadata tags for 3D environment assets.
"""
import os
import sys

def audit_directory_structure(root_dir):
    categories = [
        "Architecture", "Roads", "Sidewalks", "Buildings",
        "Vehicles", "Props", "Debris", "Decals", "Nature", "VFX"
    ]
    results = {}
    for cat in categories:
        target = os.path.join(root_dir, "Assets", "ECLIPSE", "Environment", cat)
        exists = os.path.isdir(target)
        results[cat] = exists
        if not exists:
            os.makedirs(target, exist_ok=True)
    return results

if __name__ == "__main__":
    project_root = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", ".."))
    print(f"[AssetOrganizer] Auditing ECLIPSE project at: {project_root}")
    status = audit_directory_structure(project_root)
    for cat, exists in status.items():
        print(f" - {cat}: {'EXISTS' if exists else 'CREATED'}")
    print("[AssetOrganizer] Directory verification complete.")

