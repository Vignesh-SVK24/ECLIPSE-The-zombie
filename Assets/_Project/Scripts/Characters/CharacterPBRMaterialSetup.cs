using System;
using UnityEngine;

namespace Eclipse.Character
{
    /// <summary>
    /// Configures high-fidelity cinematic semi-realistic PBR materials for characters in ECLIPSE: THE LAST CIVILIZATION.
    /// Inspired by high-end survival shooters (e.g. Left to Survive art direction).
    /// Uses custom URP PBR shaders with GGX specular distribution, skin subsurface approximation,
    /// weathered canvas and matte ballistic armor properties.
    /// </summary>
    public static class CharacterPBRMaterialSetup
    {
        public static Material[] CreateEthanCrossMaterials()
        {
            Shader pbrShader = Shader.Find("Eclipse/Character/StylizedCharacterPBR");
            if (pbrShader == null)
            {
                pbrShader = Shader.Find("Universal Render Pipeline/Lit");
            }
            if (pbrShader == null)
            {
                pbrShader = Shader.Find("Standard");
            }

            Material[] mats = new Material[4];

            // -------------------------------------------------------------
            // Submesh 0: Ethan Cross Gritty Survivor Skin (Weathered Caucasian, realistic SSS)
            // -------------------------------------------------------------
            mats[0] = new Material(pbrShader) { name = "M_Ethan_RealisticSkin" };
            mats[0].SetColor("_BaseColor", new Color(0.75f, 0.63f, 0.54f, 1.0f)); // Mature weathered Caucasian survivor skin
            mats[0].SetColor("_ShadowColor", new Color(0.24f, 0.18f, 0.16f, 1.0f)); // Natural ambient occlusion shadow
            mats[0].SetColor("_SubsurfaceColor", new Color(0.65f, 0.28f, 0.20f, 1.0f)); // Natural dermal SSS flush
            mats[0].SetColor("_RimColor", new Color(0.85f, 0.60f, 0.35f, 1.0f)); // Atmospheric dusk rim
            mats[0].SetFloat("_Smoothness", 0.42f);
            mats[0].SetFloat("_Metallic", 0.0f);
            mats[0].SetFloat("_RampThreshold", 0.48f);
            mats[0].SetFloat("_RampSmoothness", 0.16f);
            mats[0].SetFloat("_RimPower", 3.0f);
            mats[0].SetFloat("_RimIntensity", 1.2f);
            AssignTextureIfExists(mats[0], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Skin_Albedo.png");
            AssignTextureIfExists(mats[0], "_BumpMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Skin_Normal.png");

            // -------------------------------------------------------------
            // Submesh 1: Weathered Tactical Jacket (Desaturated Crimson Field Canvas)
            // -------------------------------------------------------------
            mats[1] = new Material(pbrShader) { name = "M_Ethan_FieldJacket" };
            mats[1].SetColor("_BaseColor", new Color(0.46f, 0.15f, 0.14f, 1.0f)); // Gritty desaturated field red canvas
            mats[1].SetColor("_ShadowColor", new Color(0.18f, 0.07f, 0.08f, 1.0f)); // Deep shadow fold
            mats[1].SetColor("_SubsurfaceColor", new Color(0.40f, 0.12f, 0.10f, 1.0f));
            mats[1].SetColor("_RimColor", new Color(0.85f, 0.55f, 0.30f, 1.0f));
            mats[1].SetFloat("_Smoothness", 0.25f); // Heavy tactical canvas
            mats[1].SetFloat("_Metallic", 0.02f);
            mats[1].SetFloat("_RampThreshold", 0.44f);
            mats[1].SetFloat("_RampSmoothness", 0.14f);
            mats[1].SetFloat("_RimPower", 3.2f);
            mats[1].SetFloat("_RimIntensity", 1.1f);
            AssignTextureIfExists(mats[1], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Cloth_Albedo.png");
            AssignTextureIfExists(mats[1], "_BumpMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Cloth_Normal.png");

            // -------------------------------------------------------------
            // Submesh 2: Ballistic Armor & Cargo Pants (Charcoal Slate & Tactical Olive)
            // -------------------------------------------------------------
            mats[2] = new Material(pbrShader) { name = "M_Ethan_TacticalArmor" };
            mats[2].SetColor("_BaseColor", new Color(0.16f, 0.18f, 0.19f, 1.0f)); // Dark slate charcoal
            mats[2].SetColor("_ShadowColor", new Color(0.08f, 0.09f, 0.10f, 1.0f));
            mats[2].SetColor("_RimColor", new Color(0.80f, 0.55f, 0.30f, 1.0f));
            mats[2].SetFloat("_Smoothness", 0.40f);
            mats[2].SetFloat("_Metallic", 0.18f); // Reinforced polymer plates
            mats[2].SetFloat("_RampThreshold", 0.46f);
            mats[2].SetFloat("_RampSmoothness", 0.15f);
            mats[2].SetFloat("_RimPower", 3.4f);
            mats[2].SetFloat("_RimIntensity", 1.0f);
            AssignTextureIfExists(mats[2], "_BaseMap", "Assets/ECLIPSE/Textures/Characters/EthanCross/T_EthanCross_Gear_Albedo.png");

            // -------------------------------------------------------------
            // Submesh 3: Leather Gear, Combat Boots & Survival Rucksack
            // -------------------------------------------------------------
            mats[3] = new Material(pbrShader) { name = "M_Ethan_SurvivalGear" };
            mats[3].SetColor("_BaseColor", new Color(0.28f, 0.20f, 0.14f, 1.0f)); // Worn saddle field leather
            mats[3].SetColor("_ShadowColor", new Color(0.12f, 0.09f, 0.06f, 1.0f));
            mats[3].SetColor("_RimColor", new Color(0.82f, 0.58f, 0.28f, 1.0f));
            mats[3].SetFloat("_Smoothness", 0.32f);
            mats[3].SetFloat("_Metallic", 0.10f);
            mats[3].SetFloat("_RampThreshold", 0.45f);
            mats[3].SetFloat("_RampSmoothness", 0.15f);
            mats[3].SetFloat("_RimPower", 3.2f);
            mats[3].SetFloat("_RimIntensity", 1.2f);
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

            // 0: Gritty Mottled Decayed Flesh with Thrombosed Veins
            mats[0] = new Material(infectedShader) { name = "M_Zombie_DecayedSkin" };
            mats[0].SetColor("_BaseColor", new Color(0.31f, 0.35f, 0.30f, 1.0f)); // Desaturated mottled decaying olive-gray
            mats[0].SetColor("_VeinColor", new Color(0.18f, 0.12f, 0.20f, 1.0f)); // Dark thrombosed purple
            mats[0].SetColor("_ShadowColor", new Color(0.11f, 0.13f, 0.10f, 1.0f));
            mats[0].SetColor("_EyeGlowColor", new Color(0.25f, 0.78f, 0.12f, 1.0f)); // Internal mutated ocular glow
            mats[0].SetFloat("_EyeGlowIntensity", 1.8f); // Calibrated to 1.8 (replaces previous 4.5)
            mats[0].SetFloat("_EyePulseRate", 2.2f);
            mats[0].SetFloat("_RimIntensity", 0.9f);

            // 1: Tattered Stained Workwear Fabric
            mats[1] = new Material(infectedShader) { name = "M_Zombie_TatteredCloth" };
            mats[1].SetColor("_BaseColor", new Color(0.18f, 0.20f, 0.22f, 1.0f)); // Grimy stained fabric
            mats[1].SetColor("_ShadowColor", new Color(0.09f, 0.10f, 0.11f, 1.0f));
            mats[1].SetFloat("_EyeGlowIntensity", 0.0f);

            // 2: Mutated Infected Eye (Subtle Internal Glow)
            Shader urpUnlit = Shader.Find("Universal Render Pipeline/Unlit");
            mats[2] = new Material(urpUnlit != null ? urpUnlit : infectedShader) { name = "M_Zombie_InfectedEyes" };
            mats[2].SetColor("_BaseColor", new Color(0.24f, 0.80f, 0.12f, 1.0f));
            mats[2].EnableKeyword("_EMISSION");
            mats[2].SetColor("_EmissionColor", new Color(0.24f, 0.80f, 0.12f, 1.0f) * 1.8f);

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
