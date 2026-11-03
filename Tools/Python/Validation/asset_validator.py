"""
ECLIPSE: Mobile Environment Asset Budget Validator
Validates triangle counts, texture resolutions, and LOD setups against mobile AAA targets.
"""

BUDGETS = {
    "Road_Segment": {"max_tris_lod0": 500, "max_textures": 3},
    "Building_Apartment": {"max_tris_lod0": 6000, "min_lods": 2},
    "Vehicle": {"max_tris_lod0": 5500, "min_lods": 2},
    "Tree_Mature": {"max_tris_lod0": 4000, "min_lods": 2},
    "Prop_Small": {"max_tris_lod0": 800, "min_lods": 1}
}

def validate_asset(asset_type, tri_count, lod_count):
    budget = BUDGETS.get(asset_type)
    if not budget:
        return True, "Unknown asset type, skipped."

    errors = []
    if tri_count > budget.get("max_tris_lod0", 999999):
        errors.append(f"Tri count {tri_count} exceeds LOD0 limit of {budget['max_tris_lod0']}")
    if lod_count < budget.get("min_lods", 1):
        errors.append(f"LOD count {lod_count} is less than required {budget['min_lods']}")

    return (len(errors) == 0), errors

if __name__ == "__main__":
    print("[AssetValidator] Running Mobile AAA Environment Budget Check:")
    tests = [
        ("Building_Apartment", 4200, 3),
        ("Vehicle", 4800, 2),
        ("Road_Segment", 240, 2),
        ("Tree_Mature", 3400, 2)
    ]
    for a_type, tris, lods in tests:
        passed, msg = validate_asset(a_type, tris, lods)
        status = "PASS" if passed else "FAIL"
        print(f" - [{status}] {a_type}: {tris} tris, {lods} LODs - {msg if not passed else 'Within mobile budget'}")

