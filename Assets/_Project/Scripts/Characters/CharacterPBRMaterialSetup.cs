using System;
using UnityEngine;

namespace Eclipse.Character
{
    /// <summary>
    /// Configures high-fidelity PBR materials for anatomical characters.
    /// Supports URP Lit and custom character shaders with SRP Batcher compatibility.
    /// </summary>
    public static class CharacterPBRMaterialSetup
    {
        public static Material[] CreateEthanCrossMaterials()
        {
            Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
            if (urpLitShader == null)
            {
                urpLitShader = Shader.Find("Standard");
            }

            Material[] mats = new Material[4];

            // 0: Ethan Cross Realistic Skin (Warm Caucasian with melanin tone, micro-roughness)
            mats[0] = new Material(urpLitShader) { name = "M_Ethan_Skin" };
            mats[0].SetColor("_BaseColor", new Color(0.85f, 0.68f, 0.58f, 1.0f)); // Natural skin tone
            mats[0].SetFloat("_Smoothness", 0.42f); // Soft skin scattering roughness
            mats[0].SetFloat("_Metallic", 0.0f);
            AssignTextureIfExists(mats[0], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Skin_Albedo.png");
            AssignTextureIfExists(mats[0], "_BumpMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Skin_Normal.png");

            // 1: Tactical Clothing (Weathered olive-drab combat jacket & charcoal trousers)
            mats[1] = new Material(urpLitShader) { name = "M_Ethan_TacticalCloth" };
            mats[1].SetColor("_BaseColor", new Color(0.22f, 0.28f, 0.24f, 1.0f)); // Forest tactical olive
            mats[1].SetFloat("_Smoothness", 0.25f); // Matte fabric
            mats[1].SetFloat("_Metallic", 0.02f);
            AssignTextureIfExists(mats[1], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Cloth_Albedo.png");
            AssignTextureIfExists(mats[1], "_BumpMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Cloth_Normal.png");

            // 2: Ballistic Vest & Armor Plates (Reinforced matte polymer & nylon webbing)
            mats[2] = new Material(urpLitShader) { name = "M_Ethan_Armor" };
            mats[2].SetColor("_BaseColor", new Color(0.14f, 0.16f, 0.15f, 1.0f)); // Tactical dark grey
            mats[2].SetFloat("_Smoothness", 0.45f); // Subtle polymer sheen
            mats[2].SetFloat("_Metallic", 0.15f);
            AssignTextureIfExists(mats[2], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Gear_Albedo.png");

            // 3: Gear (Leather combat boots, utility holster, backpack)
            mats[3] = new Material(urpLitShader) { name = "M_Ethan_Gear" };
            mats[3].SetColor("_BaseColor", new Color(0.35f, 0.25f, 0.18f, 1.0f)); // Worn field leather
            mats[3].SetFloat("_Smoothness", 0.32f);
            mats[3].SetFloat("_Metallic", 0.10f);
            AssignTextureIfExists(mats[3], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Gear_Albedo.png");

            return mats;
        }

        private static void AssignTextureIfExists(Material mat, string propertyName, string assetPath)
        {
#if UNITY_EDITOR
            var tex = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            if (tex != null)
            {
                mat.SetTexture(propertyName, tex);
            }
#endif
        }
    }
}
