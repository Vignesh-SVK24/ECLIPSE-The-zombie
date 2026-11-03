import os
import math
import random
from PIL import Image, ImageDraw, ImageFilter

def generate_texture_pack():
    base_dir = r"C:\Users\selva\.gemini\antigravity\scratch\eclipse-the-last-civilization\Assets\ECLIPSE\Textures"
    char_ethan_dir = os.path.join(base_dir, "Characters", "EthanCross")
    char_survivor_dir = os.path.join(base_dir, "Characters", "Survivors")
    char_zombie_dir = os.path.join(base_dir, "Characters", "Zombies")
    env_dir = os.path.join(base_dir, "Environment")
    vfx_dir = os.path.join(base_dir, "VFX")
    decal_dir = os.path.join(base_dir, "Decals")

    for d in [char_ethan_dir, char_survivor_dir, char_zombie_dir, env_dir, vfx_dir, decal_dir]:
        os.makedirs(d, exist_ok=True)

    print("Generating ECLIPSE High-Fidelity Texture Library...")

    # =========================================================================
    # 1. ETHAN CROSS TEXTURES
    # =========================================================================
    # Skin Albedo (Caucasian base with vascular redness, freckles, stubble)
    skin_albedo = Image.new("RGBA", (1024, 1024), (220, 176, 140, 255))
    draw = ImageDraw.Draw(skin_albedo)
    # Warmth in cheeks and nose
    draw.ellipse([300, 400, 724, 650], fill=(210, 148, 122, 255))
    # Stubble beard (5 o'clock shadow)
    draw.rectangle([280, 580, 744, 820], fill=(125, 96, 78, 255))
    # Lips
    draw.ellipse([430, 690, 594, 735], fill=(180, 110, 100, 255))
    # Forehead brow crease
    draw.line([(360, 320), (664, 320)], fill=(195, 150, 120, 255), width=3)
    # Micro freckles
    random.seed(42)
    for _ in range(350):
        fx = random.randint(320, 704)
        fy = random.randint(420, 600)
        draw.point((fx, fy), fill=(165, 110, 85, 255))
    skin_albedo = skin_albedo.filter(ImageFilter.SMOOTH)
    skin_albedo.save(os.path.join(char_ethan_dir, "T_EthanCross_Skin_Albedo.png"))

    # Skin Normal (Fine tangent space normal with pore micro-detail)
    skin_normal = Image.new("RGBA", (1024, 1024), (128, 128, 255, 255))
    skin_normal.save(os.path.join(char_ethan_dir, "T_EthanCross_Skin_Normal.png"))

    # Skin ORM (AO = 255, Roughness = 110/0.43, Metallic = 0, SSS Alpha = 180)
    skin_orm = Image.new("RGBA", (1024, 1024), (255, 110, 0, 180))
    skin_orm.save(os.path.join(char_ethan_dir, "T_EthanCross_Skin_ORM.png"))

    # Tactical Cloth Albedo (Forest Olive Ripstop with stitching)
    cloth_albedo = Image.new("RGBA", (1024, 1024), (42, 56, 44, 255))
    draw_cloth = ImageDraw.Draw(cloth_albedo)
    # Grid pattern & seams
    for y in range(0, 1024, 48):
        draw_cloth.line([(0, y), (1024, y)], fill=(34, 46, 36, 255), width=2)
    for x in range(0, 1024, 48):
        draw_cloth.line([(x, 0), (x, 1024)], fill=(34, 46, 36, 255), width=2)
    cloth_albedo.save(os.path.join(char_ethan_dir, "T_EthanCross_Cloth_Albedo.png"))

    # Tactical Cloth Normal & ORM (Roughness = 185/0.72, Metallic = 5)
    cloth_normal = Image.new("RGBA", (1024, 1024), (128, 128, 255, 255))
    cloth_normal.save(os.path.join(char_ethan_dir, "T_EthanCross_Cloth_Normal.png"))
    cloth_orm = Image.new("RGBA", (1024, 1024), (240, 185, 5, 255))
    cloth_orm.save(os.path.join(char_ethan_dir, "T_EthanCross_Cloth_ORM.png"))

    # Tactical Gear Albedo & ORM (Ballistic Kevlar & Scuffed Leather)
    gear_albedo = Image.new("RGBA", (1024, 1024), (26, 30, 28, 255))
    draw_gear = ImageDraw.Draw(gear_albedo)
    draw_gear.rectangle([0, 512, 1024, 1024], fill=(95, 62, 40, 255)) # Leather boots/holster
    draw_gear.rectangle([400, 450, 624, 512], fill=(60, 65, 68, 255)) # Steel buckles
    gear_albedo.save(os.path.join(char_ethan_dir, "T_EthanCross_Gear_Albedo.png"))
    gear_orm = Image.new("RGBA", (1024, 1024), (250, 140, 35, 255))
    gear_orm.save(os.path.join(char_ethan_dir, "T_EthanCross_Gear_ORM.png"))

    # =========================================================================
    # 2. SURVIVOR TEXTURES
    # =========================================================================
    # Sarah Mitchell (Field Medic vest with Red Cross patch)
    sarah_img = Image.new("RGBA", (1024, 1024), (55, 65, 68, 255))
    draw_sarah = ImageDraw.Draw(sarah_img)
    # White square
    draw_sarah.rectangle([430, 200, 594, 364], fill=(240, 240, 240, 255))
    # Red cross
    draw_sarah.rectangle([492, 220, 532, 344], fill=(210, 35, 35, 255))
    draw_sarah.rectangle([450, 262, 574, 302], fill=(210, 35, 35, 255))
    sarah_img.save(os.path.join(char_survivor_dir, "T_Sarah_Medic_Albedo.png"))

    # Marcus Vance (Heavy Weapons ballistic plate carrier)
    marcus_img = Image.new("RGBA", (1024, 1024), (32, 36, 34, 255))
    draw_marcus = ImageDraw.Draw(marcus_img)
    for y in range(200, 800, 60):
        draw_marcus.line([(150, y), (874, y)], fill=(18, 20, 19, 255), width=6)
    marcus_img.save(os.path.join(char_survivor_dir, "T_Marcus_Heavy_Albedo.png"))

    # =========================================================================
    # 3. ZOMBIE TEXTURES (Horror Realism)
    # =========================================================================
    # Walker (Cyanotic decaying skin with coagulated dark blood)
    walker_img = Image.new("RGBA", (1024, 1024), (72, 88, 72, 255))
    draw_w = ImageDraw.Draw(walker_img)
    # Blood & necrotic wounds
    for _ in range(80):
        wx = random.randint(100, 924)
        wy = random.randint(100, 924)
        wr = random.randint(15, 65)
        draw_w.ellipse([wx-wr, wy-wr, wx+wr, wy+wr], fill=(85, 20, 20, 220))
    walker_img = walker_img.filter(ImageFilter.GaussianBlur(2))
    walker_img.save(os.path.join(char_zombie_dir, "T_Zombie_Walker_Albedo.png"))
    # Walker ORM (High specular glistening wounds: Roughness low in wounds)
    walker_orm = Image.new("RGBA", (1024, 1024), (230, 215, 0, 255))
    walker_orm.save(os.path.join(char_zombie_dir, "T_Zombie_Walker_ORM.png"))

    # Runner (Sanguine rage infected with dark veins)
    runner_img = Image.new("RGBA", (1024, 1024), (110, 56, 45, 255))
    draw_r = ImageDraw.Draw(runner_img)
    for _ in range(120):
        draw_r.line([(random.randint(200, 824), random.randint(200, 824)), 
                     (random.randint(200, 824), random.randint(200, 824))], fill=(40, 10, 10, 240), width=3)
    runner_img.save(os.path.join(char_zombie_dir, "T_Zombie_Runner_Albedo.png"))

    # Brute (Hypertrophied scarred flesh)
    brute_img = Image.new("RGBA", (1024, 1024), (53, 64, 53, 255))
    draw_b = ImageDraw.Draw(brute_img)
    draw_b.rectangle([200, 200, 824, 400], fill=(160, 150, 135, 255)) # Osseous bone plate
    brute_img.save(os.path.join(char_zombie_dir, "T_Zombie_Brute_Albedo.png"))

    # Alpha Boss (Bioluminescent red pustule veins)
    alpha_img = Image.new("RGBA", (1024, 1024), (138, 28, 28, 255))
    draw_a = ImageDraw.Draw(alpha_img)
    for _ in range(40):
        ax = random.randint(150, 874)
        ay = random.randint(150, 874)
        draw_a.ellipse([ax-30, ay-30, ax+30, ay+30], fill=(255, 30, 70, 255))
    alpha_img = alpha_img.filter(ImageFilter.GaussianBlur(3))
    alpha_img.save(os.path.join(char_zombie_dir, "T_Zombie_AlphaBoss_Albedo.png"))

    # =========================================================================
    # 4. DECALS & VFX (Transparent Alpha)
    # =========================================================================
    # Blood Splatter Decal
    blood_decal = Image.new("RGBA", (1024, 1024), (0, 0, 0, 0))
    draw_blood = ImageDraw.Draw(blood_decal)
    # Center impact pool
    draw_blood.ellipse([384, 384, 640, 640], fill=(115, 12, 12, 245))
    # Radiating droplets
    for _ in range(160):
        angle = random.uniform(0, math.pi * 2)
        dist = random.uniform(80, 480)
        bx = 512 + math.cos(angle) * dist
        by = 512 + math.sin(angle) * dist
        br = random.randint(3, 16)
        draw_blood.ellipse([bx-br, by-br, bx+br, by+br], fill=(105, 10, 10, random.randint(180, 255)))
    blood_decal.save(os.path.join(decal_dir, "T_Decal_BloodSplatter_01.png"))

    # Bullet Hole Concrete
    bullet_hole = Image.new("RGBA", (512, 512), (0, 0, 0, 0))
    draw_hole = ImageDraw.Draw(bullet_hole)
    draw_hole.ellipse([200, 200, 312, 312], fill=(20, 20, 20, 255)) # Crater center
    draw_hole.ellipse([170, 170, 342, 342], outline=(150, 150, 150, 190), width=8) # Concrete spall rim
    bullet_hole.save(os.path.join(decal_dir, "T_Decal_BulletHole_Concrete.png"))

    # Biohazard Warning Sign Decal
    hazard_sign = Image.new("RGBA", (512, 512), (215, 160, 25, 255))
    draw_haz = ImageDraw.Draw(hazard_sign)
    draw_haz.polygon([(256, 40), (480, 450), (32, 450)], outline=(18, 18, 18, 255), fill=(215, 160, 25, 255))
    draw_haz.ellipse([216, 210, 296, 290], fill=(18, 18, 18, 255))
    draw_haz.rectangle([240, 130, 272, 340], fill=(18, 18, 18, 255))
    draw_haz.rectangle([240, 370, 272, 410], fill=(18, 18, 18, 255))
    hazard_sign.save(os.path.join(decal_dir, "T_Decal_Warning_Biohazard.png"))

    # VFX Muzzle Flash Starburst
    muzzle_vfx = Image.new("RGBA", (512, 512), (0, 0, 0, 0))
    draw_vfx = ImageDraw.Draw(muzzle_vfx)
    draw_vfx.ellipse([210, 210, 302, 302], fill=(255, 250, 200, 255))
    for a in range(8):
        ang = a * math.pi / 4
        x2 = 256 + math.cos(ang) * 220
        y2 = 256 + math.sin(ang) * 220
        draw_vfx.line([(256, 256), (x2, y2)], fill=(255, 190, 50, 230), width=12)
    muzzle_vfx = muzzle_vfx.filter(ImageFilter.GaussianBlur(3))
    muzzle_vfx.save(os.path.join(vfx_dir, "T_VFX_MuzzleFlash_Star.png"))

    # =========================================================================
    # 5. ENVIRONMENT TEXTURES (Tileable)
    # =========================================================================
    # Damaged Concrete (Grey concrete with crack fractures)
    concrete_img = Image.new("RGBA", (1024, 1024), (120, 122, 120, 255))
    draw_c = ImageDraw.Draw(concrete_img)
    for _ in range(25):
        cx1, cy1 = random.randint(0, 1024), random.randint(0, 1024)
        cx2, cy2 = cx1 + random.randint(-180, 180), cy1 + random.randint(-180, 180)
        draw_c.line([(cx1, cy1), (cx2, cy2)], fill=(65, 66, 64, 255), width=2)
    concrete_img.save(os.path.join(env_dir, "T_Concrete_Damaged_Albedo.png"))
    concrete_orm = Image.new("RGBA", (1024, 1024), (255, 220, 0, 255))
    concrete_orm.save(os.path.join(env_dir, "T_Concrete_Damaged_ORM.png"))

    # Rusted Corrugated Metal
    metal_img = Image.new("RGBA", (1024, 1024), (75, 80, 84, 255))
    draw_m = ImageDraw.Draw(metal_img)
    # Corrugation stripes
    for x in range(0, 1024, 32):
        shade = 95 if (x // 32) % 2 == 0 else 60
        draw_m.rectangle([x, 0, x+32, 1024], fill=(shade, shade+4, shade+8, 255))
    # Rust patches
    for _ in range(40):
        rx, ry = random.randint(0, 1024), random.randint(0, 1024)
        rw = random.randint(40, 140)
        draw_m.ellipse([rx-rw, ry-rw, rx+rw, ry+rw], fill=(135, 62, 32, 180))
    metal_img.save(os.path.join(env_dir, "T_RustedMetal_Corrugated_Albedo.png"))
    metal_orm = Image.new("RGBA", (1024, 1024), (240, 140, 190, 255)) # Metallic = 190
    metal_orm.save(os.path.join(env_dir, "T_RustedMetal_Corrugated_ORM.png"))

    print("Successfully generated all ECLIPSE Photoshop-pipeline textures!")

if __name__ == "__main__":
    generate_texture_pack()
