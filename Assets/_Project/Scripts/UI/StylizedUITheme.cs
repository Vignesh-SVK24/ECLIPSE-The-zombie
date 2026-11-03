using UnityEngine;

namespace Eclipse.UI
{
    [CreateAssetMenu(fileName = "StylizedUITheme", menuName = "ECLIPSE/UI/Theme", order = 100)]
    public class StylizedUITheme : ScriptableObject
    {
        [Header("Brand & State Colors")]
        public Color BrandRed = new Color(0.827f, 0.184f, 0.184f, 1.0f);        // #D32F2F (Critical / Stencil)
        public Color BrandAmber = new Color(1.0f, 0.702f, 0.0f, 1.0f);           // #FFB300 (Stamina / Focus)
        public Color ToxicGreen = new Color(0.0f, 0.902f, 0.463f, 1.0f);         // #00E676 (Biohazard / Eye Glow)
        public Color ShieldBlue = new Color(0.098f, 0.529f, 0.824f, 1.0f);        // #1976D2 (Armor / Ally)

        [Header("Surfaces & Borders")]
        public Color PanelBackground = new Color(0.078f, 0.098f, 0.106f, 0.88f); // #14191B
        public Color CardBackground = new Color(0.118f, 0.145f, 0.157f, 1.0f);   // #1E2528
        public Color BorderBronze = new Color(0.553f, 0.431f, 0.388f, 1.0f);    // #8D6E63
        public Color BorderFocused = new Color(1.0f, 0.753f, 0.2f, 1.0f);        // #FFC033

        [Header("Typography")]
        public Color TextPrimary = new Color(0.925f, 0.937f, 0.945f, 1.0f);     // #ECEFF1
        public Color TextMuted = new Color(0.565f, 0.643f, 0.682f, 1.0f);       // #90A4AE
        public Color TextHighlight = new Color(1.0f, 0.835f, 0.31f, 1.0f);       // #FFD54F

        [Header("Item Rarity Colors")]
        public Color RarityCommon = new Color(0.62f, 0.62f, 0.62f, 1.0f);        // Gray
        public Color RarityUncommon = new Color(0.298f, 0.686f, 0.314f, 1.0f);    // Green
        public Color RarityRare = new Color(0.129f, 0.588f, 0.953f, 1.0f);        // Blue
        public Color RarityEpic = new Color(0.612f, 0.153f, 0.69f, 1.0f);         // Purple
        public Color RarityLegendary = new Color(1.0f, 0.627f, 0.0f, 1.0f);       // Gold

        public Color GetRarityColor(int rarityTier)
        {
            switch (rarityTier)
            {
                case 1: return RarityUncommon;
                case 2: return RarityRare;
                case 3: return RarityEpic;
                case 4: return RarityLegendary;
                default: return RarityCommon;
            }
        }
    }
}
