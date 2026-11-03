using System;
using UnityEngine;

namespace Eclipse.Character
{
    /// <summary>
    /// Enumeration of all standard humanoid bones utilized in ECLIPSE rigs.
    /// Maps 1:1 with Unity Humanoid Avatar bone specifications.
    /// </summary>
    public enum HumanoidBoneType
    {
        Hips,
        Spine,
        Chest,
        Neck,
        Head,
        LeftEye,
        RightEye,
        
        // Left Arm
        LeftShoulder,
        LeftUpperArm,
        LeftLowerArm,
        LeftHand,
        LeftThumbProximal,
        LeftThumbIntermediate,
        LeftThumbDistal,
        LeftIndexProximal,
        LeftIndexIntermediate,
        LeftIndexDistal,
        LeftMiddleProximal,
        LeftMiddleIntermediate,
        LeftMiddleDistal,

        // Right Arm
        RightShoulder,
        RightUpperArm,
        RightLowerArm,
        RightHand,
        RightThumbProximal,
        RightThumbIntermediate,
        RightThumbDistal,
        RightIndexProximal,
        RightIndexIntermediate,
        RightIndexDistal,
        RightMiddleProximal,
        RightMiddleIntermediate,
        RightMiddleDistal,

        // Left Leg
        LeftUpperLeg,
        LeftLowerLeg,
        LeftFoot,
        LeftToes,

        // Right Leg
        RightUpperLeg,
        RightLowerLeg,
        RightFoot,
        RightToes,

        // Sockets (Non-humanoid attachment points)
        Socket_Weapon_RightHand,
        Socket_Weapon_LeftHand,
        Socket_Weapon_Holster,
        Socket_Weapon_Back,
        Socket_Head_Flashlight
    }

    /// <summary>
    /// Metadata descriptor for standard anatomical proportions (adult male 1.83m).
    /// </summary>
    public static class AnatomicalProportions
    {
        public const float CharacterHeight = 1.83f;
        public const float HeadHeight = 0.244f; // 1:7.5 head proportion
        public const float EyeHeight = 1.71f;
        public const float NeckHeight = 1.56f;
        public const float ShoulderHeight = 1.48f;
        public const float ShoulderWidth = 0.48f;
        public const float ChestHeight = 1.35f;
        public const float SpineHeight = 1.15f;
        public const float HipsHeight = 0.98f;
        public const float KneeHeight = 0.52f;
        public const float AnkleHeight = 0.12f;
        public const float ArmLength = 0.65f;
    }
}
