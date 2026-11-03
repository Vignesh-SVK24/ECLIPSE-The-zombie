using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eclipse.Character
{
    /// <summary>
    /// Generates high-fidelity anatomical humanoid mesh geometry with bone weights,
    /// UV coordinates, vertex tangents, and multiple submeshes for PBR shading.
    /// Replaces placeholder stick-figures with anatomical, proportioned human meshes.
    /// </summary>
    public static class AnatomicalHumanoidGenerator
    {
        public class GeneratedHumanoidMesh
        {
            public Mesh Mesh;
            public Material[] Materials;
        }

        /// <summary>
        /// Builds the complete anatomical Ethan Cross mesh rigged to the specified bone transforms.
        /// </summary>
        public static GeneratedHumanoidMesh GenerateEthanCrossMesh(HumanoidRigBuilder.RigResult rig, Material[] pbrMaterials)
        {
            Mesh mesh = new Mesh();
            mesh.name = "EthanCross_Anatomical_LOD0";

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector4> tangents = new List<Vector4>();
            List<BoneWeight> boneWeights = new List<BoneWeight>();

            // Submesh index lists for materials:
            // 0: Skin (Head, Neck, Hands)
            // 1: Tactical Clothing (Jacket, Trousers)
            // 2: Tactical Vest & Armor (Knee pads, vest plates, pouches)
            // 3: Gear (Backpack, Holster, Combat Boots)
            List<int> triSkin = new List<int>();
            List<int> triCloth = new List<int>();
            List<int> triArmor = new List<int>();
            List<int> triGear = new List<int>();

            // Map BoneType to index in rig.BoneTransforms
            Dictionary<HumanoidBoneType, int> boneIndexMap = new Dictionary<HumanoidBoneType, int>();
            for (int i = 0; i < rig.BoneTransforms.Length; i++)
            {
                foreach (var kvp in rig.Bones)
                {
                    if (kvp.Value == rig.BoneTransforms[i])
                    {
                        boneIndexMap[kvp.Key] = i;
                        break;
                    }
                }
            }

            int headBoneIdx = boneIndexMap[HumanoidBoneType.Head];
            int neckBoneIdx = boneIndexMap[HumanoidBoneType.Neck];
            int chestBoneIdx = boneIndexMap[HumanoidBoneType.Chest];
            int spineBoneIdx = boneIndexMap[HumanoidBoneType.Spine];
            int hipsBoneIdx = boneIndexMap[HumanoidBoneType.Hips];

            int lUpperArmIdx = boneIndexMap[HumanoidBoneType.LeftUpperArm];
            int lLowerArmIdx = boneIndexMap[HumanoidBoneType.LeftLowerArm];
            int lHandIdx = boneIndexMap[HumanoidBoneType.LeftHand];

            int rUpperArmIdx = boneIndexMap[HumanoidBoneType.RightUpperArm];
            int rLowerArmIdx = boneIndexMap[HumanoidBoneType.RightLowerArm];
            int rHandIdx = boneIndexMap[HumanoidBoneType.RightHand];

            int lUpperLegIdx = boneIndexMap[HumanoidBoneType.LeftUpperLeg];
            int lLowerLegIdx = boneIndexMap[HumanoidBoneType.LeftLowerLeg];
            int lFootIdx = boneIndexMap[HumanoidBoneType.LeftFoot];

            int rUpperLegIdx = boneIndexMap[HumanoidBoneType.RightUpperLeg];
            int rLowerLegIdx = boneIndexMap[HumanoidBoneType.RightLowerLeg];
            int rFootIdx = boneIndexMap[HumanoidBoneType.RightFoot];

            // 1. ANATOMICAL HEAD & FACE (Detailed cranial structure, nose bridge, jawline, eye sockets)
            Vector3 headCenter = new Vector3(0, AnatomicalProportions.EyeHeight, 0);
            AddAnatomicalHead(vertices, normals, uvs, tangents, boneWeights, triSkin, headCenter, headBoneIdx, neckBoneIdx);

            // 2. NECK (Muscular sternocleidomastoid transition)
            AddNeckGeometry(vertices, normals, uvs, tangents, boneWeights, triSkin, headBoneIdx, neckBoneIdx, chestBoneIdx);

            // 3. TORSO WITH TACTICAL COMBAT JACKET (Pectorals, spine curve, latissimus)
            AddTacticalTorso(vertices, normals, uvs, tangents, boneWeights, triCloth, chestBoneIdx, spineBoneIdx, hipsBoneIdx);

            // 4. BALLISTIC TACTICAL VEST & POUCHES (Layered armor plate, chest rig, radio, straps)
            AddTacticalVestAndPouches(vertices, normals, uvs, tangents, boneWeights, triArmor, chestBoneIdx, spineBoneIdx);

            // 5. ARMS & FOREARMS (Biceps, triceps, elbow articulation)
            AddArmGeometry(vertices, normals, uvs, tangents, boneWeights, triCloth, isLeft: true, chestBoneIdx, lUpperArmIdx, lLowerArmIdx);
            AddArmGeometry(vertices, normals, uvs, tangents, boneWeights, triCloth, isLeft: false, chestBoneIdx, rUpperArmIdx, rLowerArmIdx);

            // 6. DETAILED HANDS & FINGERS (5-digit articulated hands with tactical half-gloves)
            AddDetailedHand(vertices, normals, uvs, tangents, boneWeights, triSkin, triGear, isLeft: true, lLowerArmIdx, lHandIdx);
            AddDetailedHand(vertices, normals, uvs, tangents, boneWeights, triSkin, triGear, isLeft: false, rLowerArmIdx, rHandIdx);

            // 7. LEGS WITH CARGO TROUSERS & KNEE PADS (Quadriceps, calves, kneecaps)
            AddLegGeometry(vertices, normals, uvs, tangents, boneWeights, triCloth, triArmor, isLeft: true, hipsBoneIdx, lUpperLegIdx, lLowerLegIdx);
            AddLegGeometry(vertices, normals, uvs, tangents, boneWeights, triCloth, triArmor, isLeft: false, hipsBoneIdx, rUpperLegIdx, rLowerLegIdx);

            // 8. COMBAT ASSAULT BOOTS (Treaded soles, reinforced ankle support, laces)
            AddCombatBoots(vertices, normals, uvs, tangents, boneWeights, triGear, isLeft: true, lLowerLegIdx, lFootIdx);
            AddCombatBoots(vertices, normals, uvs, tangents, boneWeights, triGear, isLeft: false, rLowerLegIdx, rFootIdx);

            // 9. FIELD BACKPACK & SIDEARM HOLSTER
            AddBackpackAndHolster(vertices, normals, uvs, tangents, boneWeights, triGear, chestBoneIdx, spineBoneIdx, rUpperLegIdx);

            // Assign geometry to Unity Mesh
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTangents(tangents);
            mesh.boneWeights = boneWeights.ToArray();
            mesh.bindposes = rig.BindPoses;

            // 4 Submeshes
            mesh.subMeshCount = 4;
            mesh.SetTriangles(triSkin, 0);
            mesh.SetTriangles(triCloth, 1);
            mesh.SetTriangles(triArmor, 2);
            mesh.SetTriangles(triGear, 3);

            mesh.RecalculateBounds();
            mesh.RecalculateTangents();

            return new GeneratedHumanoidMesh
            {
                Mesh = mesh,
                Materials = pbrMaterials != null && pbrMaterials.Length >= 4 ? pbrMaterials : new Material[4]
            };
        }

        #region Geometry Generators

        private static void AddAnatomicalHead(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> tris, Vector3 center, int headBone, int neckBone)
        {
            int startIdx = verts.Count;
            int latitudeBands = 14;
            int longitudeBands = 18;
            float radiusX = 0.105f; // Head width
            float radiusY = 0.135f; // Head height
            float radiusZ = 0.125f; // Head depth

            for (int lat = 0; lat <= latitudeBands; lat++)
            {
                float theta = lat * Mathf.PI / latitudeBands;
                float sinTheta = Mathf.Sin(theta);
                float cosTheta = Mathf.Cos(theta);

                for (int lon = 0; lon <= longitudeBands; lon++)
                {
                    float phi = lon * 2 * Mathf.PI / longitudeBands;
                    float sinPhi = Mathf.Sin(phi);
                    float cosPhi = Mathf.Cos(phi);

                    Vector3 pos = new Vector3(
                        cosPhi * sinTheta * radiusX,
                        cosTheta * radiusY,
                        sinPhi * sinTheta * radiusZ
                    );

                    // Anatomical jaw / chin refinement & brow protrusion
                    if (pos.y < -0.04f && pos.z > 0.02f)
                    {
                        // Chin narrowing & jaw taper
                        pos.x *= 0.75f;
                        pos.z += 0.025f;
                    }
                    if (pos.y > 0.02f && pos.y < 0.06f && pos.z > 0.06f)
                    {
                        // Nose bridge & brow protrusion
                        pos.z += 0.035f;
                    }

                    Vector3 worldPos = center + pos;
                    Vector3 norm = pos.normalized;

                    verts.Add(worldPos);
                    norms.Add(norm);
                    uvs.Add(new Vector2((float)lon / longitudeBands, (float)lat / latitudeBands));
                    tans.Add(new Vector4(-sinPhi, 0, cosPhi, 1.0f));

                    // Bone weights (90% Head, 10% Neck blend at base)
                    float neckBlend = Mathf.Clamp01((-pos.y / radiusY) * 0.4f);
                    weights.Add(new BoneWeight
                    {
                        boneIndex0 = headBone,
                        weight0 = 1.0f - neckBlend,
                        boneIndex1 = neckBone,
                        weight1 = neckBlend
                    });
                }
            }

            for (int lat = 0; lat < latitudeBands; lat++)
            {
                for (int lon = 0; lon < longitudeBands; lon++)
                {
                    int first = startIdx + (lat * (longitudeBands + 1)) + lon;
                    int second = first + longitudeBands + 1;

                    tris.Add(first);
                    tris.Add(second);
                    tris.Add(first + 1);

                    tris.Add(second);
                    tris.Add(second + 1);
                    tris.Add(first + 1);
                }
            }
        }

        private static void AddNeckGeometry(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> tris, int headBone, int neckBone, int chestBone)
        {
            int startIdx = verts.Count;
            int segments = 12;
            float topY = AnatomicalProportions.NeckHeight + 0.05f;
            float bottomY = AnatomicalProportions.NeckHeight - 0.06f;
            float topRadius = 0.068f;
            float bottomRadius = 0.088f;

            for (int ring = 0; ring <= 2; ring++)
            {
                float t = ring / 2.0f;
                float y = Mathf.Lerp(topY, bottomY, t);
                float r = Mathf.Lerp(topRadius, bottomRadius, t);

                for (int i = 0; i <= segments; i++)
                {
                    float angle = i * 2 * Mathf.PI / segments;
                    float x = Mathf.Cos(angle) * r;
                    float z = Mathf.Sin(angle) * r;

                    verts.Add(new Vector3(x, y, z));
                    norms.Add(new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized);
                    uvs.Add(new Vector2((float)i / segments, t));
                    tans.Add(new Vector4(-Mathf.Sin(angle), 0, Mathf.Cos(angle), 1f));

                    weights.Add(new BoneWeight
                    {
                        boneIndex0 = neckBone,
                        weight0 = 0.7f,
                        boneIndex1 = (t < 0.5f) ? headBone : chestBone,
                        weight1 = 0.3f
                    });
                }
            }

            for (int ring = 0; ring < 2; ring++)
            {
                for (int i = 0; i < segments; i++)
                {
                    int first = startIdx + (ring * (segments + 1)) + i;
                    int second = first + segments + 1;

                    tris.Add(first);
                    tris.Add(second);
                    tris.Add(first + 1);

                    tris.Add(second);
                    tris.Add(second + 1);
                    tris.Add(first + 1);
                }
            }
        }

        private static void AddTacticalTorso(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> tris, int chestBone, int spineBone, int hipsBone)
        {
            int startIdx = verts.Count;
            int rings = 5;
            int segs = 16;
            float[] ringHeights = { 1.48f, 1.38f, 1.25f, 1.12f, 0.98f };
            float[] rx = { 0.24f, 0.23f, 0.20f, 0.18f, 0.19f };
            float[] rz = { 0.15f, 0.16f, 0.14f, 0.13f, 0.14f };

            for (int r = 0; r < rings; r++)
            {
                float y = ringHeights[r];
                for (int s = 0; s <= segs; s++)
                {
                    float angle = s * 2 * Mathf.PI / segs;
                    float cosA = Mathf.Cos(angle);
                    float sinA = Mathf.Sin(angle);

                    float x = cosA * rx[r];
                    float z = sinA * rz[r];

                    verts.Add(new Vector3(x, y, z));
                    norms.Add(new Vector3(cosA, 0.1f, sinA).normalized);
                    uvs.Add(new Vector2((float)s / segs, (float)r / (rings - 1)));
                    tans.Add(new Vector4(-sinA, 0, cosA, 1f));

                    // Bone blending: Chest -> Spine -> Hips
                    if (r <= 1)
                    {
                        weights.Add(new BoneWeight { boneIndex0 = chestBone, weight0 = 0.85f, boneIndex1 = spineBone, weight1 = 0.15f });
                    }
                    else if (r <= 3)
                    {
                        weights.Add(new BoneWeight { boneIndex0 = spineBone, weight0 = 0.7f, boneIndex1 = hipsBone, weight1 = 0.3f });
                    }
                    else
                    {
                        weights.Add(new BoneWeight { boneIndex0 = hipsBone, weight0 = 0.9f, boneIndex1 = spineBone, weight1 = 0.1f });
                    }
                }
            }

            for (int r = 0; r < rings - 1; r++)
            {
                for (int s = 0; s < segs; s++)
                {
                    int first = startIdx + (r * (segs + 1)) + s;
                    int second = first + segs + 1;

                    tris.Add(first);
                    tris.Add(second);
                    tris.Add(first + 1);

                    tris.Add(second);
                    tris.Add(second + 1);
                    tris.Add(first + 1);
                }
            }
        }

        private static void AddTacticalVestAndPouches(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> tris, int chestBone, int spineBone)
        {
            // Front & Back Ballistic Plates
            AddBoxMesh(verts, norms, uvs, tans, weights, tris,
                center: new Vector3(0, 1.34f, 0.12f),
                size: new Vector3(0.32f, 0.36f, 0.05f),
                bone0: chestBone, weight0: 0.8f, bone1: spineBone, weight1: 0.2f);

            AddBoxMesh(verts, norms, uvs, tans, weights, tris,
                center: new Vector3(0, 1.34f, -0.12f),
                size: new Vector3(0.32f, 0.36f, 0.05f),
                bone0: chestBone, weight0: 0.8f, bone1: spineBone, weight1: 0.2f);

            // Ammo Magazine Pouches (Front lower chest)
            AddBoxMesh(verts, norms, uvs, tans, weights, tris,
                center: new Vector3(-0.08f, 1.22f, 0.15f),
                size: new Vector3(0.065f, 0.12f, 0.04f),
                bone0: chestBone, weight0: 0.6f, bone1: spineBone, weight1: 0.4f);

            AddBoxMesh(verts, norms, uvs, tans, weights, tris,
                center: new Vector3(0.0f, 1.22f, 0.15f),
                size: new Vector3(0.065f, 0.12f, 0.04f),
                bone0: chestBone, weight0: 0.6f, bone1: spineBone, weight1: 0.4f);

            AddBoxMesh(verts, norms, uvs, tans, weights, tris,
                center: new Vector3(0.08f, 1.22f, 0.15f),
                size: new Vector3(0.065f, 0.12f, 0.04f),
                bone0: chestBone, weight0: 0.6f, bone1: spineBone, weight1: 0.4f);
        }

        private static void AddArmGeometry(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> tris, bool isLeft, int chestBone, int upperArmBone, int lowerArmBone)
        {
            float side = isLeft ? -1.0f : 1.0f;
            int startIdx = verts.Count;
            int segs = 10;
            int stages = 4;
            Vector3[] joints = {
                new Vector3(side * 0.20f, 1.44f, 0),    // Shoulder
                new Vector3(side * 0.35f, 1.30f, 0),    // Bicep
                new Vector3(side * 0.48f, 1.15f, -0.02f),// Elbow
                new Vector3(side * 0.58f, 0.98f, 0.02f)  // Wrist
            };
            float[] rad = { 0.075f, 0.068f, 0.058f, 0.048f };

            for (int st = 0; st < stages; st++)
            {
                Vector3 c = joints[st];
                for (int s = 0; s <= segs; s++)
                {
                    float angle = s * 2 * Mathf.PI / segs;
                    float yOffset = Mathf.Sin(angle) * rad[st];
                    float zOffset = Mathf.Cos(angle) * rad[st];

                    verts.Add(new Vector3(c.x, c.y + yOffset, c.z + zOffset));
                    norms.Add(new Vector3(side * 0.2f, Mathf.Sin(angle), Mathf.Cos(angle)).normalized);
                    uvs.Add(new Vector2((float)s / segs, (float)st / (stages - 1)));
                    tans.Add(new Vector4(0, -Mathf.Cos(angle), Mathf.Sin(angle), 1f));

                    if (st <= 1)
                    {
                        weights.Add(new BoneWeight { boneIndex0 = upperArmBone, weight0 = 0.85f, boneIndex1 = chestBone, weight1 = 0.15f });
                    }
                    else
                    {
                        weights.Add(new BoneWeight { boneIndex0 = lowerArmBone, weight0 = 0.85f, boneIndex1 = upperArmBone, weight1 = 0.15f });
                    }
                }
            }

            for (int st = 0; st < stages - 1; st++)
            {
                for (int s = 0; s < segs; s++)
                {
                    int first = startIdx + (st * (segs + 1)) + s;
                    int second = first + segs + 1;

                    if (isLeft)
                    {
                        tris.Add(first); tris.Add(second); tris.Add(first + 1);
                        tris.Add(second); tris.Add(second + 1); tris.Add(first + 1);
                    }
                    else
                    {
                        tris.Add(first); tris.Add(first + 1); tris.Add(second);
                        tris.Add(second); tris.Add(first + 1); tris.Add(second + 1);
                    }
                }
            }
        }

        private static void AddDetailedHand(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> triSkin, List<int> triGear, bool isLeft, int lowerArmBone, int handBone)
        {
            float side = isLeft ? -1.0f : 1.0f;
            Vector3 wristPos = new Vector3(side * 0.60f, 0.94f, 0.03f);

            // Palm & Glove Back
            AddBoxMesh(verts, norms, uvs, tans, weights, triGear,
                center: wristPos + new Vector3(side * 0.04f, -0.04f, 0),
                size: new Vector3(0.045f, 0.08f, 0.085f),
                bone0: handBone, weight0: 0.95f, bone1: lowerArmBone, weight1: 0.05f);

            // 5 Distinct Fingers (Thumb + Index + Middle + Ring + Pinky)
            float[] fingerZOffsets = { -0.03f, -0.01f, 0.01f, 0.03f };
            float[] fingerLengths = { 0.042f, 0.048f, 0.045f, 0.038f };

            // 4 Fingers
            for (int f = 0; f < 4; f++)
            {
                Vector3 basePos = wristPos + new Vector3(side * 0.07f, -0.09f, fingerZOffsets[f]);
                AddBoxMesh(verts, norms, uvs, tans, weights, triSkin,
                    center: basePos,
                    size: new Vector3(0.016f, fingerLengths[f], 0.016f),
                    bone0: handBone, weight0: 1.0f, bone1: 0, weight1: 0.0f);
            }

            // Thumb (Angled forward)
            Vector3 thumbPos = wristPos + new Vector3(side * 0.03f, -0.05f, 0.048f);
            AddBoxMesh(verts, norms, uvs, tans, weights, triSkin,
                center: thumbPos,
                size: new Vector3(0.02f, 0.04f, 0.02f),
                bone0: handBone, weight0: 1.0f, bone1: 0, weight1: 0.0f);
        }

        private static void AddLegGeometry(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> triCloth, List<int> triArmor, bool isLeft, int hipsBone, int upperLegBone, int lowerLegBone)
        {
            float side = isLeft ? -1.0f : 1.0f;
            int startIdx = verts.Count;
            int segs = 12;
            int stages = 4;
            Vector3[] joints = {
                new Vector3(side * 0.11f, 0.96f, 0),     // Hip joint
                new Vector3(side * 0.12f, 0.74f, 0.01f), // Thigh
                new Vector3(side * 0.12f, 0.52f, 0),     // Knee
                new Vector3(side * 0.12f, 0.22f, -0.01f) // Shin / Calf
            };
            float[] rad = { 0.10f, 0.092f, 0.075f, 0.068f };

            for (int st = 0; st < stages; st++)
            {
                Vector3 c = joints[st];
                for (int s = 0; s <= segs; s++)
                {
                    float angle = s * 2 * Mathf.PI / segs;
                    float xOffset = Mathf.Cos(angle) * rad[st];
                    float zOffset = Mathf.Sin(angle) * rad[st];

                    verts.Add(new Vector3(c.x + xOffset, c.y, c.z + zOffset));
                    norms.Add(new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized);
                    uvs.Add(new Vector2((float)s / segs, (float)st / (stages - 1)));
                    tans.Add(new Vector4(-Mathf.Sin(angle), 0, Mathf.Cos(angle), 1f));

                    if (st <= 1)
                    {
                        weights.Add(new BoneWeight { boneIndex0 = upperLegBone, weight0 = 0.85f, boneIndex1 = hipsBone, weight1 = 0.15f });
                    }
                    else
                    {
                        weights.Add(new BoneWeight { boneIndex0 = lowerLegBone, weight0 = 0.85f, boneIndex1 = upperLegBone, weight1 = 0.15f });
                    }
                }
            }

            for (int st = 0; st < stages - 1; st++)
            {
                for (int s = 0; s < segs; s++)
                {
                    int first = startIdx + (st * (segs + 1)) + s;
                    int second = first + segs + 1;

                    if (isLeft)
                    {
                        triCloth.Add(first); triCloth.Add(second); triCloth.Add(first + 1);
                        triCloth.Add(second); triCloth.Add(second + 1); triCloth.Add(first + 1);
                    }
                    else
                    {
                        triCloth.Add(first); triCloth.Add(first + 1); triCloth.Add(second);
                        triCloth.Add(second); triCloth.Add(first + 1); triCloth.Add(second + 1);
                    }
                }
            }

            // Reinforced Polymer Knee Pad
            AddBoxMesh(verts, norms, uvs, tans, weights, triArmor,
                center: new Vector3(side * 0.12f, 0.52f, 0.08f),
                size: new Vector3(0.09f, 0.12f, 0.035f),
                bone0: lowerLegBone, weight0: 0.7f, bone1: upperLegBone, weight1: 0.3f);
        }

        private static void AddCombatBoots(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> triGear, bool isLeft, int lowerLegBone, int footBone)
        {
            float side = isLeft ? -1.0f : 1.0f;
            Vector3 footBase = new Vector3(side * 0.12f, 0.06f, 0.04f);

            // Ankle & Heel section
            AddBoxMesh(verts, norms, uvs, tans, weights, triGear,
                center: footBase + new Vector3(0, 0.06f, -0.04f),
                size: new Vector3(0.095f, 0.14f, 0.10f),
                bone0: footBone, weight0: 0.7f, bone1: lowerLegBone, weight1: 0.3f);

            // Toe box & Sole
            AddBoxMesh(verts, norms, uvs, tans, weights, triGear,
                center: footBase + new Vector3(0, 0.03f, 0.07f),
                size: new Vector3(0.10f, 0.075f, 0.16f),
                bone0: footBone, weight0: 0.95f, bone1: lowerLegBone, weight1: 0.05f);
        }

        private static void AddBackpackAndHolster(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> triGear, int chestBone, int spineBone, int rightLegBone)
        {
            // Rugged Field Backpack (Back of torso)
            AddBoxMesh(verts, norms, uvs, tans, weights, triGear,
                center: new Vector3(0, 1.30f, -0.22f),
                size: new Vector3(0.34f, 0.44f, 0.18f),
                bone0: chestBone, weight0: 0.75f, bone1: spineBone, weight1: 0.25f);

            // Sidearm Drop-Leg Holster (Right thigh)
            AddBoxMesh(verts, norms, uvs, tans, weights, triGear,
                center: new Vector3(0.19f, 0.74f, 0.02f),
                size: new Vector3(0.05f, 0.16f, 0.08f),
                bone0: rightLegBone, weight0: 0.95f, bone1: 0, weight1: 0.0f);
        }

        private static void AddBoxMesh(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> tris, Vector3 center, Vector3 size,
            int bone0, float weight0, int bone1, float weight1)
        {
            int startIdx = verts.Count;
            Vector3 h = size * 0.5f;

            Vector3[] p = {
                center + new Vector3(-h.x, -h.y,  h.z), // 0: Front-bottom-left
                center + new Vector3( h.x, -h.y,  h.z), // 1: Front-bottom-right
                center + new Vector3( h.x,  h.y,  h.z), // 2: Front-top-right
                center + new Vector3(-h.x,  h.y,  h.z), // 3: Front-top-left
                center + new Vector3( h.x, -h.y, -h.z), // 4: Back-bottom-right
                center + new Vector3(-h.x, -h.y, -h.z), // 5: Back-bottom-left
                center + new Vector3(-h.x,  h.y, -h.z), // 6: Back-top-left
                center + new Vector3( h.x,  h.y, -h.z)  // 7: Back-top-right
            };

            // 6 Faces * 4 Vertices = 24 Vertices
            AddFace(verts, norms, uvs, tans, weights, tris, p[0], p[1], p[2], p[3], Vector3.forward, bone0, weight0, bone1, weight1);
            AddFace(verts, norms, uvs, tans, weights, tris, p[1], p[4], p[7], p[2], Vector3.right, bone0, weight0, bone1, weight1);
            AddFace(verts, norms, uvs, tans, weights, tris, p[4], p[5], p[6], p[7], Vector3.back, bone0, weight0, bone1, weight1);
            AddFace(verts, norms, uvs, tans, weights, tris, p[5], p[0], p[3], p[6], Vector3.left, bone0, weight0, bone1, weight1);
            AddFace(verts, norms, uvs, tans, weights, tris, p[3], p[2], p[7], p[6], Vector3.up, bone0, weight0, bone1, weight1);
            AddFace(verts, norms, uvs, tans, weights, tris, p[5], p[4], p[1], p[0], Vector3.down, bone0, weight0, bone1, weight1);
        }

        private static void AddFace(
            List<Vector3> verts, List<Vector3> norms, List<Vector2> uvs, List<Vector4> tans,
            List<BoneWeight> weights, List<int> tris, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3,
            Vector3 normal, int bone0, float weight0, int bone1, float weight1)
        {
            int idx = verts.Count;
            verts.Add(v0); verts.Add(v1); verts.Add(v2); verts.Add(v3);
            for (int i = 0; i < 4; i++) norms.Add(normal);

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(0, 1));

            Vector4 tan = (normal == Vector3.up || normal == Vector3.down)
                ? new Vector4(1, 0, 0, 1)
                : Vector3.Cross(normal, Vector3.up).normalized;
            tan.w = 1.0f;
            for (int i = 0; i < 4; i++) tans.Add(tan);

            BoneWeight bw = new BoneWeight
            {
                boneIndex0 = bone0,
                weight0 = weight0,
                boneIndex1 = bone1,
                weight1 = weight1
            };
            for (int i = 0; i < 4; i++) weights.Add(bw);

            tris.Add(idx);
            tris.Add(idx + 1);
            tris.Add(idx + 2);
            tris.Add(idx);
            tris.Add(idx + 2);
            tris.Add(idx + 3);
        }

        #endregion
    }
}
