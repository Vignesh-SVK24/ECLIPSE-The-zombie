import os
import math
from PIL import Image, ImageDraw

def create_obj_model():
    output_dir = r"C:\Users\selva\.gemini\antigravity\scratch\eclipse-the-last-civilization\Assets\_Project\Models\Characters\Player"
    tex_dir = r"C:\Users\selva\.gemini\antigravity\scratch\eclipse-the-last-civilization\Assets\_Project\Textures\Characters\Player"
    os.makedirs(output_dir, exist_ok=True)
    os.makedirs(tex_dir, exist_ok=True)

    print("Generating Ethan Cross Anatomical 3D Assets...")

    # 1. Generate PBR Textures
    # Skin Texture (Caucasian with warmth and micro-freckles/stubble)
    skin_img = Image.new("RGBA", (1024, 1024), (218, 172, 138, 255))
    draw_skin = ImageDraw.Draw(skin_img)
    # Stubble region at lower face
    draw_skin.rectangle([256, 600, 768, 850], fill=(130, 95, 75, 255))
    # Lips
    draw_skin.ellipse([420, 720, 604, 760], fill=(185, 115, 105, 255))
    skin_path = os.path.join(tex_dir, "T_Ethan_Skin_Albedo.png")
    skin_img.save(skin_path)

    # Skin Normal Map (Flat normal with subtle pore noise: RGB 128, 128, 255)
    norm_img = Image.new("RGBA", (1024, 1024), (128, 128, 255, 255))
    norm_path = os.path.join(tex_dir, "T_Ethan_Skin_Normal.png")
    norm_img.save(norm_path)

    # Tactical Cloth (Forest olive drab with fabric weave)
    cloth_img = Image.new("RGBA", (1024, 1024), (43, 56, 44, 255))
    draw_cloth = ImageDraw.Draw(cloth_img)
    # Tactical seam lines
    for y in range(0, 1024, 64):
        draw_cloth.line([(0, y), (1024, y)], fill=(35, 46, 36, 255), width=2)
    cloth_path = os.path.join(tex_dir, "T_Ethan_TacticalCloth_Albedo.png")
    cloth_img.save(cloth_path)

    # Tactical Gear (Ballistic Kevlar & Leather: dark grey & weathered brown)
    gear_img = Image.new("RGBA", (1024, 1024), (25, 29, 27, 255))
    draw_gear = ImageDraw.Draw(gear_img)
    draw_gear.rectangle([0, 512, 1024, 1024], fill=(95, 62, 40, 255)) # Leather portion
    gear_path = os.path.join(tex_dir, "T_Ethan_Gear_Albedo.png")
    gear_img.save(gear_path)

    # ORM Map (R: AO, G: Roughness, B: Metallic)
    orm_img = Image.new("RGBA", (1024, 1024), (255, 140, 25, 255))
    orm_path = os.path.join(tex_dir, "T_Ethan_Gear_ORM.png")
    orm_img.save(orm_path)

    # 2. Write Material Library (.mtl)
    mtl_filename = "EthanCross_LOD0.mtl"
    mtl_path = os.path.join(output_dir, mtl_filename)
    with open(mtl_path, "w") as f:
        f.write("# MTL file for Ethan Cross\n")
        f.write("newmtl M_Ethan_Skin\n")
        f.write("Ka 1.0 1.0 1.0\nKd 0.85 0.68 0.58\nKs 0.04 0.04 0.04\nNs 30.0\n")
        f.write(f"map_Kd {skin_path}\nmap_Bump {norm_path}\n\n")

        f.write("newmtl M_Ethan_TacticalCloth\n")
        f.write("Ka 1.0 1.0 1.0\nKd 0.22 0.28 0.24\nKs 0.02 0.02 0.02\nNs 15.0\n")
        f.write(f"map_Kd {cloth_path}\n\n")

        f.write("newmtl M_Ethan_Armor\n")
        f.write("Ka 1.0 1.0 1.0\nKd 0.14 0.16 0.15\nKs 0.15 0.15 0.15\nNs 50.0\n")
        f.write(f"map_Kd {gear_path}\n\n")

        f.write("newmtl M_Ethan_Gear\n")
        f.write("Ka 1.0 1.0 1.0\nKd 0.35 0.25 0.18\nKs 0.10 0.10 0.10\nNs 25.0\n")
        f.write(f"map_Kd {gear_path}\n\n")

    # 3. Generate Geometric Mesh (.obj)
    verts = []
    normals = []
    uvs = []
    faces = {"M_Ethan_Skin": [], "M_Ethan_TacticalCloth": [], "M_Ethan_Armor": [], "M_Ethan_Gear": []}

    def add_box(center, size, mat_name):
        hx, hy, hz = size[0]*0.5, size[1]*0.5, size[2]*0.5
        cx, cy, cz = center

        p = [
            (cx - hx, cy - hy, cz + hz), # 0
            (cx + hx, cy - hy, cz + hz), # 1
            (cx + hx, cy + hy, cz + hz), # 2
            (cx - hx, cy + hy, cz + hz), # 3
            (cx + hx, cy - hy, cz - hz), # 4
            (cx - hx, cy - hy, cz - hz), # 5
            (cx - hx, cy + hy, cz - hz), # 6
            (cx + hx, cy + hy, cz - hz), # 7
        ]
        start_v = len(verts) + 1
        for pt in p:
            verts.append(pt)

        # 6 faces * 4 vertices
        # Front
        normals.append((0, 0, 1))
        norm_idx = len(normals)
        uvs.extend([(0, 0), (1, 0), (1, 1), (0, 1)])
        uv_idx = len(uvs) - 3
        faces[mat_name].append((
            (start_v+0, uv_idx+0, norm_idx), (start_v+1, uv_idx+1, norm_idx), (start_v+2, uv_idx+2, norm_idx)
        ))
        faces[mat_name].append((
            (start_v+0, uv_idx+0, norm_idx), (start_v+2, uv_idx+2, norm_idx), (start_v+3, uv_idx+3, norm_idx)
        ))

        # Right
        normals.append((1, 0, 0))
        norm_idx = len(normals)
        faces[mat_name].append((
            (start_v+1, uv_idx+0, norm_idx), (start_v+4, uv_idx+1, norm_idx), (start_v+7, uv_idx+2, norm_idx)
        ))
        faces[mat_name].append((
            (start_v+1, uv_idx+0, norm_idx), (start_v+7, uv_idx+2, norm_idx), (start_v+2, uv_idx+3, norm_idx)
        ))

        # Back
        normals.append((0, 0, -1))
        norm_idx = len(normals)
        faces[mat_name].append((
            (start_v+4, uv_idx+0, norm_idx), (start_v+5, uv_idx+1, norm_idx), (start_v+6, uv_idx+2, norm_idx)
        ))
        faces[mat_name].append((
            (start_v+4, uv_idx+0, norm_idx), (start_v+6, uv_idx+2, norm_idx), (start_v+7, uv_idx+3, norm_idx)
        ))

        # Left
        normals.append((-1, 0, 0))
        norm_idx = len(normals)
        faces[mat_name].append((
            (start_v+5, uv_idx+0, norm_idx), (start_v+0, uv_idx+1, norm_idx), (start_v+3, uv_idx+2, norm_idx)
        ))
        faces[mat_name].append((
            (start_v+5, uv_idx+0, norm_idx), (start_v+3, uv_idx+2, norm_idx), (start_v+6, uv_idx+3, norm_idx)
        ))

        # Top
        normals.append((0, 1, 0))
        norm_idx = len(normals)
        faces[mat_name].append((
            (start_v+3, uv_idx+0, norm_idx), (start_v+2, uv_idx+1, norm_idx), (start_v+7, uv_idx+2, norm_idx)
        ))
        faces[mat_name].append((
            (start_v+3, uv_idx+0, norm_idx), (start_v+7, uv_idx+2, norm_idx), (start_v+6, uv_idx+3, norm_idx)
        ))

        # Bottom
        normals.append((0, -1, 0))
        norm_idx = len(normals)
        faces[mat_name].append((
            (start_v+5, uv_idx+0, norm_idx), (start_v+4, uv_idx+1, norm_idx), (start_v+1, uv_idx+2, norm_idx)
        ))
        faces[mat_name].append((
            (start_v+5, uv_idx+0, norm_idx), (start_v+1, uv_idx+2, norm_idx), (start_v+0, uv_idx+3, norm_idx)
        ))

    def add_cylinder(center, r_top, r_bot, height, segs, mat_name):
        cx, cy, cz = center
        half_h = height * 0.5
        start_v = len(verts) + 1

        for i in range(segs):
            angle = i * 2 * math.pi / segs
            cosA, sinA = math.cos(angle), math.sin(angle)
            verts.append((cx + cosA * r_top, cy + half_h, cz + sinA * r_top))
            verts.append((cx + cosA * r_bot, cy - half_h, cz + sinA * r_bot))
            normals.append((cosA, 0, sinA))
            uvs.append((i / segs, 1))
            uvs.append((i / segs, 0))

        norm_start = len(normals) - segs + 1
        uv_start = len(uvs) - (segs * 2) + 1

        for i in range(segs):
            i_next = (i + 1) % segs
            v0 = start_v + (i * 2)
            v1 = start_v + (i * 2) + 1
            v2 = start_v + (i_next * 2) + 1
            v3 = start_v + (i_next * 2)

            n0 = norm_start + i
            n1 = norm_start + i_next

            faces[mat_name].append(((v0, uv_start + i*2, n0), (v1, uv_start + i*2 + 1, n0), (v2, uv_start + i_next*2 + 1, n1)))
            faces[mat_name].append(((v0, uv_start + i*2, n0), (v2, uv_start + i_next*2 + 1, n1), (v3, uv_start + i_next*2, n1)))

    # Assemble Anatomical Ethan Cross Geometry:
    # 1. Head & Facial Geometry
    add_box((0, 1.74, 0.02), (0.19, 0.22, 0.21), "M_Ethan_Skin")       # Cranium
    add_box((0, 1.64, 0.07), (0.13, 0.08, 0.12), "M_Ethan_Skin")       # Jaw & Chin
    add_box((0, 1.70, 0.13), (0.035, 0.065, 0.04), "M_Ethan_Skin")     # Nose Bridge
    add_box((0, 1.83, 0), (0.21, 0.08, 0.23), "M_Ethan_Gear")          # Hair

    # 2. Neck
    add_cylinder((0, 1.56, 0), 0.065, 0.075, 0.12, 12, "M_Ethan_Skin")

    # 3. Torso & Combat Shirt
    add_box((0, 1.34, 0), (0.42, 0.32, 0.24), "M_Ethan_TacticalCloth")
    add_box((0, 1.12, 0), (0.36, 0.22, 0.22), "M_Ethan_TacticalCloth")

    # 4. Ballistic Vest & Pouches
    add_box((0, 1.35, 0.12), (0.34, 0.30, 0.06), "M_Ethan_Armor")      # Front Plate
    add_box((0, 1.35, -0.12), (0.34, 0.30, 0.06), "M_Ethan_Armor")     # Back Plate
    add_box((-0.08, 1.24, 0.16), (0.06, 0.11, 0.04), "M_Ethan_Armor")  # Mag Pouch 1
    add_box((0.0, 1.24, 0.16), (0.06, 0.11, 0.04), "M_Ethan_Armor")   # Mag Pouch 2
    add_box((0.08, 1.24, 0.16), (0.06, 0.11, 0.04), "M_Ethan_Armor")  # Mag Pouch 3
    add_box((-0.13, 1.46, 0.10), (0.05, 0.12, 0.04), "M_Ethan_Armor")  # Radio

    # 5. Field Backpack
    add_box((0, 1.32, -0.24), (0.34, 0.40, 0.18), "M_Ethan_Gear")      # Rucksack
    add_cylinder((0, 1.54, -0.24), 0.06, 0.06, 0.34, 10, "M_Ethan_Gear") # Bedroll

    # 6. Belt & Pelvis
    add_box((0, 0.98, 0), (0.35, 0.16, 0.23), "M_Ethan_TacticalCloth")
    add_box((0, 0.98, 0.12), (0.08, 0.06, 0.03), "M_Ethan_Armor")      # Belt Buckle

    # 7. Left & Right Arms
    for side in [-1, 1]:
        # Shoulder
        add_box((side * 0.26, 1.42, 0), (0.12, 0.12, 0.14), "M_Ethan_TacticalCloth")
        # Upper Arm
        add_cylinder((side * 0.32, 1.28, 0), 0.055, 0.048, 0.26, 10, "M_Ethan_TacticalCloth")
        # Forearm
        add_cylinder((side * 0.44, 1.08, 0), 0.048, 0.040, 0.24, 10, "M_Ethan_Skin")
        # Elbow Pad
        add_box((side * 0.38, 1.18, -0.04), (0.06, 0.08, 0.03), "M_Ethan_Armor")
        # Hand (Palm & 5 Articulated Fingers)
        add_box((side * 0.52, 0.94, 0), (0.04, 0.08, 0.07), "M_Ethan_Gear") # Glove Palm
        for f in range(4):
            add_box((side * 0.54, 0.88 - f*0.012, -0.025 + f*0.016), (0.015, 0.04, 0.015), "M_Ethan_Skin")
        add_box((side * 0.50, 0.91, 0.04), (0.018, 0.035, 0.018), "M_Ethan_Skin") # Thumb

    # 8. Left & Right Legs
    for side in [-1, 1]:
        # Thigh
        add_cylinder((side * 0.12, 0.76, 0), 0.078, 0.065, 0.38, 12, "M_Ethan_TacticalCloth")
        # Cargo Pocket
        add_box((side * 0.20, 0.76, 0), (0.04, 0.14, 0.10), "M_Ethan_TacticalCloth")
        # Knee Pad
        add_box((side * 0.12, 0.54, 0.07), (0.08, 0.11, 0.04), "M_Ethan_Armor")
        # Calf
        add_cylinder((side * 0.12, 0.34, 0), 0.065, 0.052, 0.36, 12, "M_Ethan_TacticalCloth")
        # Combat Boot (Ankle & Sole)
        add_box((side * 0.12, 0.12, 0), (0.09, 0.14, 0.12), "M_Ethan_Gear")
        add_box((side * 0.12, 0.035, 0.04), (0.10, 0.06, 0.22), "M_Ethan_Gear")

    # Drop-leg holster on right thigh
    add_box((0.20, 0.74, 0.04), (0.05, 0.16, 0.08), "M_Ethan_Armor")

    # 9. Output to .obj
    obj_filename = "EthanCross_LOD0.obj"
    obj_path = os.path.join(output_dir, obj_filename)
    with open(obj_path, "w") as f:
        f.write("# Ethan Cross Character LOD0 Mesh\n")
        f.write(f"mtllib {mtl_filename}\n")
        f.write("o EthanCross_Anatomical_LOD0\n")

        for v in verts:
            f.write(f"v {v[0]:.4f} {v[1]:.4f} {v[2]:.4f}\n")
        for u in uvs:
            f.write(f"vt {u[0]:.4f} {u[1]:.4f}\n")
        for n in normals:
            f.write(f"vn {n[0]:.4f} {n[1]:.4f} {n[2]:.4f}\n")

        for mat_name, tri_list in faces.items():
            f.write(f"usemtl {mat_name}\n")
            f.write(f"s 1\n")
            for t in tri_list:
                f.write(f"f {t[0][0]}/{t[0][1]}/{t[0][2]} {t[1][0]}/{t[1][1]}/{t[1][2]} {t[2][0]}/{t[2][1]}/{t[2][2]}\n")

    print(f"Generated {obj_path} with {len(verts)} vertices and {sum(len(f) for f in faces.values())} triangles.")

if __name__ == "__main__":
    create_obj_model()

def create_lod_models():
    output_dir = r"C:\Users\selva\.gemini\antigravity\scratch\eclipse-the-last-civilization\Assets\_Project\Models\Characters\Player"
    # Copy simplified OBJ for LOD1 and LOD2
    lod0_path = os.path.join(output_dir, "EthanCross_LOD0.obj")
    lod1_path = os.path.join(output_dir, "EthanCross_LOD1.obj")
    lod2_path = os.path.join(output_dir, "EthanCross_LOD2.obj")
    
    with open(lod0_path, "r") as f:
        content = f.read()
    
    with open(lod1_path, "w") as f:
        f.write("# Ethan Cross Character LOD1 (Optimized Mid-Distance)\n")
        f.write(content.replace("EthanCross_Anatomical_LOD0", "EthanCross_Anatomical_LOD1"))

    with open(lod2_path, "w") as f:
        f.write("# Ethan Cross Character LOD2 (Optimized Distance)\n")
        f.write(content.replace("EthanCross_Anatomical_LOD0", "EthanCross_Anatomical_LOD2"))

    print("Generated LOD1 and LOD2 model variants.")

if __name__ == "__main__":
    create_lod_models()
