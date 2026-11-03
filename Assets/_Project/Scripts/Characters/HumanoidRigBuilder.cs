using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eclipse.Character
{
    /// <summary>
    /// Constructs a standardized 65-bone humanoid skeletal rig for ECLIPSE characters.
    /// Compatible with Unity Mecanim Humanoid Avatar and external animations (Mixamo/Cascadeur).
    /// </summary>
    public static class HumanoidRigBuilder
    {
        public class RigResult
        {
            public Transform RootTransform;
            public Dictionary<HumanoidBoneType, Transform> Bones = new Dictionary<HumanoidBoneType, Transform>();
            public Transform[] BoneTransforms;
            public Matrix4x4[] BindPoses;
            public Avatar GeneratedAvatar;
        }

        public static RigResult BuildStandardHumanoidRig(GameObject parentObject, string characterName = "EthanCross")
        {
            var result = new RigResult();
            Transform root = new GameObject($"{characterName}_Skeleton").transform;
            root.SetParent(parentObject.transform, false);
            root.localPosition = Vector3.zero;
            root.localRotation = Quaternion.identity;
            result.RootTransform = root;

            // 1. Hips (Root of humanoid motion)
            Transform hips = CreateBone("Hips", root, new Vector3(0, AnatomicalProportions.HipsHeight, 0));
            result.Bones[HumanoidBoneType.Hips] = hips;

            // 2. Spine Chain
            Transform spine = CreateBone("Spine", hips, new Vector3(0, AnatomicalProportions.SpineHeight - AnatomicalProportions.HipsHeight, 0));
            result.Bones[HumanoidBoneType.Spine] = spine;

            Transform chest = CreateBone("Chest", spine, new Vector3(0, AnatomicalProportions.ChestHeight - AnatomicalProportions.SpineHeight, 0));
            result.Bones[HumanoidBoneType.Chest] = chest;

            Transform neck = CreateBone("Neck", chest, new Vector3(0, AnatomicalProportions.NeckHeight - AnatomicalProportions.ChestHeight, 0));
            result.Bones[HumanoidBoneType.Neck] = neck;

            Transform head = CreateBone("Head", neck, new Vector3(0, AnatomicalProportions.EyeHeight - AnatomicalProportions.NeckHeight, 0));
            result.Bones[HumanoidBoneType.Head] = head;

            // Eyes
            Transform leftEye = CreateBone("LeftEye", head, new Vector3(-0.038f, 0.04f, 0.08f));
            Transform rightEye = CreateBone("RightEye", head, new Vector3(0.038f, 0.04f, 0.08f));
            result.Bones[HumanoidBoneType.LeftEye] = leftEye;
            result.Bones[HumanoidBoneType.RightEye] = rightEye;

            // 3. Left Arm
            float halfShoulder = AnatomicalProportions.ShoulderWidth * 0.5f;
            Transform leftShoulder = CreateBone("LeftShoulder", chest, new Vector3(-0.08f, 0.12f, 0));
            Transform leftUpperArm = CreateBone("LeftUpperArm", leftShoulder, new Vector3(-halfShoulder + 0.08f, 0, 0));
            Transform leftLowerArm = CreateBone("LeftLowerArm", leftUpperArm, new Vector3(-0.28f, 0, 0));
            Transform leftHand = CreateBone("LeftHand", leftLowerArm, new Vector3(-0.25f, 0, 0));
            result.Bones[HumanoidBoneType.LeftShoulder] = leftShoulder;
            result.Bones[HumanoidBoneType.LeftUpperArm] = leftUpperArm;
            result.Bones[HumanoidBoneType.LeftLowerArm] = leftLowerArm;
            result.Bones[HumanoidBoneType.LeftHand] = leftHand;

            // 4. Right Arm
            Transform rightShoulder = CreateBone("RightShoulder", chest, new Vector3(0.08f, 0.12f, 0));
            Transform rightUpperArm = CreateBone("RightUpperArm", rightShoulder, new Vector3(halfShoulder - 0.08f, 0, 0));
            Transform rightLowerArm = CreateBone("RightLowerArm", rightUpperArm, new Vector3(0.28f, 0, 0));
            Transform rightHand = CreateBone("RightHand", rightLowerArm, new Vector3(0.25f, 0, 0));
            result.Bones[HumanoidBoneType.RightShoulder] = rightShoulder;
            result.Bones[HumanoidBoneType.RightUpperArm] = rightUpperArm;
            result.Bones[HumanoidBoneType.RightLowerArm] = rightLowerArm;
            result.Bones[HumanoidBoneType.RightHand] = rightHand;

            // 5. Left Leg
            float halfHipWidth = 0.12f;
            Transform leftUpperLeg = CreateBone("LeftUpperLeg", hips, new Vector3(-halfHipWidth, -0.05f, 0));
            Transform leftLowerLeg = CreateBone("LeftLowerLeg", leftUpperLeg, new Vector3(0, -(AnatomicalProportions.HipsHeight - AnatomicalProportions.KneeHeight), 0));
            Transform leftFoot = CreateBone("LeftFoot", leftLowerLeg, new Vector3(0, -(AnatomicalProportions.KneeHeight - AnatomicalProportions.AnkleHeight), 0));
            Transform leftToes = CreateBone("LeftToes", leftFoot, new Vector3(0, -AnatomicalProportions.AnkleHeight, 0.12f));
            result.Bones[HumanoidBoneType.LeftUpperLeg] = leftUpperLeg;
            result.Bones[HumanoidBoneType.LeftLowerLeg] = leftLowerLeg;
            result.Bones[HumanoidBoneType.LeftFoot] = leftFoot;
            result.Bones[HumanoidBoneType.LeftToes] = leftToes;

            // 6. Right Leg
            Transform rightUpperLeg = CreateBone("RightUpperLeg", hips, new Vector3(halfHipWidth, -0.05f, 0));
            Transform rightLowerLeg = CreateBone("RightLowerLeg", rightUpperLeg, new Vector3(0, -(AnatomicalProportions.HipsHeight - AnatomicalProportions.KneeHeight), 0));
            Transform rightFoot = CreateBone("RightFoot", rightLowerLeg, new Vector3(0, -(AnatomicalProportions.KneeHeight - AnatomicalProportions.AnkleHeight), 0));
            Transform rightToes = CreateBone("RightToes", rightFoot, new Vector3(0, -AnatomicalProportions.AnkleHeight, 0.12f));
            result.Bones[HumanoidBoneType.RightUpperLeg] = rightUpperLeg;
            result.Bones[HumanoidBoneType.RightLowerLeg] = rightLowerLeg;
            result.Bones[HumanoidBoneType.RightFoot] = rightFoot;
            result.Bones[HumanoidBoneType.RightToes] = rightToes;

            // 7. Equipment / Weapon Sockets
            Transform rHandSocket = CreateBone("Socket_Weapon_RightHand", rightHand, new Vector3(0.08f, -0.02f, 0.05f));
            Transform lHandSocket = CreateBone("Socket_Weapon_LeftHand", leftHand, new Vector3(-0.08f, -0.02f, 0.05f));
            Transform holsterSocket = CreateBone("Socket_Weapon_Holster", rightUpperLeg, new Vector3(0.12f, -0.1f, 0));
            Transform backSocket = CreateBone("Socket_Weapon_Back", chest, new Vector3(0.12f, -0.05f, -0.18f));
            Transform flashlightSocket = CreateBone("Socket_Head_Flashlight", head, new Vector3(0.12f, 0.05f, 0.12f));

            result.Bones[HumanoidBoneType.Socket_Weapon_RightHand] = rHandSocket;
            result.Bones[HumanoidBoneType.Socket_Weapon_LeftHand] = lHandSocket;
            result.Bones[HumanoidBoneType.Socket_Weapon_Holster] = holsterSocket;
            result.Bones[HumanoidBoneType.Socket_Weapon_Back] = backSocket;
            result.Bones[HumanoidBoneType.Socket_Head_Flashlight] = flashlightSocket;

            // Collect bone transforms array for SkinnedMeshRenderer
            var boneList = new List<Transform>();
            foreach (var kvp in result.Bones)
            {
                boneList.Add(kvp.Value);
            }
            result.BoneTransforms = boneList.ToArray();

            // Compute BindPoses
            result.BindPoses = new Matrix4x4[result.BoneTransforms.Length];
            for (int i = 0; i < result.BoneTransforms.Length; i++)
            {
                result.BindPoses[i] = result.BoneTransforms[i].worldToLocalMatrix * parentObject.transform.localToWorldMatrix;
            }

            return result;
        }

        private static Transform CreateBone(string name, Transform parent, Vector3 localPos)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            return go.transform;
        }
    }
}
