using System;
using UnityEngine;

namespace Eclipse.Character
{
    /// <summary>
    /// Configures high-fidelity stylized PBR materials for characters in ECLIPSE: THE LAST CIVILIZATION.
    /// Uses custom URP stylized shaders with half-Lambert light ramps, sunset rim lighting,
    /// and hand-painted surface aesthetics matching the key visual art bible.
    /// </summary>
    public static class CharacterPBRMaterialSetup
    {
        public static Material[] CreateEthanCrossMaterials()
        {
            Shader stylizedShader = Shader.Find("Eclipse/Character/StylizedCharacterPBR");
            if (stylizedShader == null)
            {
                stylizedShader = Shader.Find("Universal Render Pipeline/Lit");
            }
            if (stylizedShader == null)
            {
                stylizedShader = Shader.Find("Standard");
            }

            Material[] mats = new Material[4];

            // -------------------------------------------------------------
            // Submesh 0: Ethan Cross Stylized Hero Skin (Warm peach tone, warm SSS)
            // -------------------------------------------------------------
            mats[0] = new Material(stylizedShader) { name = "M_Ethan_StylizedSkin" };
            mats[0].SetColor("_BaseColor", new Color(0.92f, 0.72f, 0.60f, 1.0f)); // Stylized warm heroic Caucasian skin
            mats[0].SetColor("_ShadowColor", new Color(0.35f, 0.22f, 0.28f, 1.0f)); // Stylized twilight shadow
            mats[0].SetColor("_SubsurfaceColor", new Color(0.95f, 0.38f, 0.20f, 1.0f)); // Warm sunset flush
            mats[0].SetColor("_RimColor", new Color(1.0f, 0.68f, 0.28f, 1.0f)); // Golden hour rim
            mats[0].SetFloat("_Smoothness", 0.45f);
            mats[0].SetFloat("_Metallic", 0.0f);
            mats[0].SetFloat("_RampThreshold", 0.48f);
            mats[0].SetFloat("_RampSmoothness", 0.16f);
            mats[0].SetFloat("_RimPower", 2.8f);
            mats[0].SetFloat("_RimIntensity", 1.8f);
            AssignTextureIfExists(mats[0], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Skin_Albedo.png");
            AssignTextureIfExists(mats[0], "_BumpMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Skin_Normal.png");

            // -------------------------------------------------------------
            // Submesh 1: Heroic Tactical Jacket (Crimson Red Hooded Combat Jacket)
            // Matches reference key art: bold, recognizable hero silhouette
            // -------------------------------------------------------------
            mats[1] = new Material(stylizedShader) { name = "M_Ethan_CrimsonJacket" };
            mats[1].SetColor("_BaseColor", new Color(0.72f, 0.14f, 0.14f, 1.0f)); // Signature Crimson Red
            mats[1].SetColor("_ShadowColor", new Color(0.28f, 0.08f, 0.10f, 1.0f)); // Deep burgundy shadow
            mats[1].SetColor("_SubsurfaceColor", new Color(0.85f, 0.20f, 0.12f, 1.0f));
            mats[1].SetColor("_RimColor", new Color(1.0f, 0.70f, 0.30f, 1.0f)); // Warm sunset edge
            mats[1].SetFloat("_Smoothness", 0.28f); // High-grade tactical canvas
            mats[1].SetFloat("_Metallic", 0.04f);
            mats[1].SetFloat("_RampThreshold", 0.44f);
            mats[1].SetFloat("_RampSmoothness", 0.14f);
            mats[1].SetFloat("_RimPower", 3.0f);
            mats[1].SetFloat("_RimIntensity", 1.5f);
            AssignTextureIfExists(mats[1], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Cloth_Albedo.png");
            AssignTextureIfExists(mats[1], "_BumpMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Cloth_Normal.png");

            // -------------------------------------------------------------
            // Submesh 2: Ballistic Armor & Cargo Pants (Charcoal Slate & Matte Polymer)
            // -------------------------------------------------------------
            mats[2] = new Material(stylizedShader) { name = "M_Ethan_TacticalArmor" };
            mats[2].SetColor("_BaseColor", new Color(0.18f, 0.20f, 0.22f, 1.0f)); // Dark slate charcoal
            mats[2].SetColor("_ShadowColor", new Color(0.10f, 0.11f, 0.14f, 1.0f));
            mats[2].SetColor("_RimColor", new Color(0.95f, 0.65f, 0.35f, 1.0f));
            mats[2].SetFloat("_Smoothness", 0.42f);
            mats[2].SetFloat("_Metallic", 0.18f); // Reinforced polymer plates
            mats[2].SetFloat("_RampThreshold", 0.46f);
            mats[2].SetFloat("_RampSmoothness", 0.15f);
            mats[2].SetFloat("_RimPower", 3.2f);
            mats[2].SetFloat("_RimIntensity", 1.4f);
            AssignTextureIfExists(mats[2], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Gear_Albedo.png");

            // -------------------------------------------------------------
            // Submesh 3: Leather Gear, Combat Boots & Survival Rucksack
            // -------------------------------------------------------------
            mats[3] = new Material(stylizedShader) { name = "M_Ethan_SurvivalGear" };
            mats[3].SetColor("_BaseColor", new Color(0.38f, 0.26f, 0.18f, 1.0f)); // Worn saddle field leather
            mats[3].SetColor("_ShadowColor", new Color(0.18f, 0.12f, 0.08f, 1.0f));
            mats[3].SetColor("_RimColor", new Color(1.0f, 0.72f, 0.32f, 1.0f));
            mats[3].SetFloat("_Smoothness", 0.35f);
            mats[3].SetFloat("_Metallic", 0.12f);
            mats[3].SetFloat("_RampThreshold", 0.45f);
            mats[3].SetFloat("_RampSmoothness", 0.15f);
            mats[3].SetFloat("_RimPower", 3.0f);
            mats[3].SetFloat("_RimIntensity", 1.6f);
            AssignTextureIfExists(mats[3], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Gear_Albedo.png");

            return mats;
        }

        public static Material[] CreateStylizedZombieMaterials()
        {
            Shader infectedShader = Shader.Find("Eclipse/Character/StylizedInfectedPBR");
            if (infectedShader == null)
            {
                infectedShader = Shader.Find("Universal Render Pipeline/Lit");
            }
            if (infectedShader == null)
            {
                infectedShader = Shader.Find("Standard");
            }

            Material[] mats = new Material[3];

            // 0: Sickly Olive Decayed Flesh
            mats[0] = new Material(infectedShader) { name = "M_Zombie_DecayedSkin" };
            mats[0].SetColor("_BaseColor", new Color(0.34f, 0.44f, 0.30f, 1.0f)); // Desaturated sickly olive
            mats[0].SetColor("_VeinColor", new Color(0.26f, 0.16f, 0.30f, 1.0f)); // Bruised purple
            mats[0].SetColor("_ShadowColor", new Color(0.14f, 0.18f, 0.12f, 1.0f));
            mats[0].SetColor("_EyeGlowColor", new Color(0.22f, 1.0f, 0.08f, 1.0f)); // Intense neon green
            mats[0].SetFloat("_EyeGlowIntensity", 4.5f);
            mats[0].SetFloat("_EyePulseRate", 3.5f);
            mats[0].SetFloat("_RimIntensity", 1.4f);

            // 1: Tattered Civilian Clothing
            mats[1] = new Material(infectedShader) { name = "M_Zombie_TatteredCloth" };
            mats[1].SetColor("_BaseColor", new Color(0.25f, 0.28f, 0.32f, 1.0f)); // Grimy stained fabric
            mats[1].SetColor("_ShadowColor", new Color(0.12f, 0.14f, 0.16f, 1.0f));
            mats[1].SetFloat("_EyeGlowIntensity", 0.0f);

            // 2: Neon Green Glowing Eyes (Hyper-emissive)
            Shader urpUnlit = Shader.Find("Universal Render Pipeline/Unlit");
            mats[2] = new Material(urpUnlit != null ? urpUnlit : infectedShader) { name = "M_Zombie_NeonEyes" };
            mats[2].SetColor("_BaseColor", new Color(0.22f, 1.0f, 0.08f, 1.0f));
            mats[2].EnableKeyword("_EMISSION");
            mats[2].SetColor("_EmissionColor", new Color(0.22f, 1.0f, 0.08f, 1.0f) * 3.5f);

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
