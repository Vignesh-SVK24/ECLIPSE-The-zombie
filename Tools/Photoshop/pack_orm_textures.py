import os
import sys
from PIL import Image

def pack_orm(ao_path, roughness_path, metallic_path, output_path, detail_path=None):
    """
    Packs separate Ambient Occlusion, Roughness, and Metallic grayscale images
    into a single Unity URP Mask Map (ORM):
    - Red Channel   = Ambient Occlusion (AO)
    - Green Channel = Roughness
    - Blue Channel  = Metallic
    - Alpha Channel = Detail / Subsurface Scattering (Optional)
    """
    if not os.path.exists(ao_path) or not os.path.exists(roughness_path) or not os.path.exists(metallic_path):
        print(f"Error: Missing input maps: {ao_path}, {roughness_path}, or {metallic_path}")
        return False

    ao_img = Image.open(ao_path).convert("L")
    rough_img = Image.open(roughness_path).convert("L")
    metal_img = Image.open(metallic_path).convert("L")

    # Match dimensions
    size = ao_img.size
    rough_img = rough_img.resize(size, Image.Resampling.BILINEAR)
    metal_img = metal_img.resize(size, Image.Resampling.BILINEAR)

    if detail_path and os.path.exists(detail_path):
        detail_img = Image.open(detail_path).convert("L").resize(size, Image.Resampling.BILINEAR)
        packed = Image.merge("RGBA", (ao_img, rough_img, metal_img, detail_img))
    else:
        # Default alpha to 255 (Full white)
        alpha_img = Image.new("L", size, 255)
        packed = Image.merge("RGBA", (ao_img, rough_img, metal_img, alpha_img))

    os.makedirs(os.path.dirname(output_path), exist_ok=True)
    packed.save(output_path, "PNG", optimize=True)
    print(f"Successfully packed ORM texture: {output_path} ({size[0]}x{size[1]})")
    return True

if __name__ == "__main__":
    if len(sys.argv) >= 5:
        detail = sys.argv[5] if len(sys.argv) > 5 else None
        pack_orm(sys.argv[1], sys.argv[2], sys.argv[3], sys.argv[4], detail)
    else:
        print("Usage: python pack_orm_textures.py <ao.png> <roughness.png> <metallic.png> <output_orm.png> [optional_detail.png]")
