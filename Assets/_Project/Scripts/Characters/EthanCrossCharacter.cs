using System;
using UnityEngine;
using Eclipse.Damage;

namespace Eclipse.Character
{
    /// <summary>
    /// Master coordinator for the protagonist hero character Ethan Cross.
    /// Manages anatomical mesh generation, humanoid rig binding, hitbox attachments,
    /// weapon sockets, and animation controllers.
    /// </summary>
    [RequireComponent(typeof(HealthComponent))]
    public class EthanCrossCharacter : MonoBehaviour
    {
        [Header("Character Configuration")]
        [SerializeField] private string _characterName = "Ethan Cross";
        [SerializeField] private bool _autoGenerateAnatomicalMeshOnAwake = true;

        [Header("Rig & Mesh References")]
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private Transform _skeletonRoot;
        [SerializeField] private CharacterAnimationController _animationController;

        [Header("Weapon Sockets")]
        [SerializeField] private Transform _rightHandSocket;
        [SerializeField] private Transform _leftHandSocket;
        [SerializeField] private Transform _holsterSocket;
        [SerializeField] private Transform _backSocket;

        [Header("Damage & Health Integration")]
        [SerializeField] private HealthComponent _healthComponent;

        private HumanoidRigBuilder.RigResult _rigResult;

        public Transform RightHandSocket => _rightHandSocket;
        public Transform LeftHandSocket => _leftHandSocket;
        public Transform HolsterSocket => _holsterSocket;
        public Transform BackSocket => _backSocket;
        public CharacterAnimationController AnimationController => _animationController;
        public HealthComponent Health => _healthComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();

            if (_autoGenerateAnatomicalMeshOnAwake && _skinnedMeshRenderer == null)
            {
                ConstructAnatomicalEthanCross();
            }
        }

        /// <summary>
        /// Assembles the complete anatomical character: 65-bone rig, anatomical mesh,
        /// PBR materials, hitboxes, sockets, and animation controller.
        /// </summary>
        [ContextMenu("Construct Anatomical Ethan Cross")]
        public void ConstructAnatomicalEthanCross()
        {
            // 1. Build Humanoid Rig
            _rigResult = HumanoidRigBuilder.BuildStandardHumanoidRig(gameObject, _characterName.Replace(" ", ""));
            _skeletonRoot = _rigResult.RootTransform;

            _rightHandSocket = _rigResult.Bones[HumanoidBoneType.Socket_Weapon_RightHand];
            _leftHandSocket = _rigResult.Bones[HumanoidBoneType.Socket_Weapon_LeftHand];
            _holsterSocket = _rigResult.Bones[HumanoidBoneType.Socket_Weapon_Holster];
            _backSocket = _rigResult.Bones[HumanoidBoneType.Socket_Weapon_Back];

            // 2. Generate PBR Materials
            Material[] pbrMaterials = CharacterPBRMaterialSetup.CreateEthanCrossMaterials();

            // 3. Generate Anatomical Skinned Mesh
            var generatedMesh = AnatomicalHumanoidGenerator.GenerateEthanCrossMesh(_rigResult, pbrMaterials);

            // 4. Setup SkinnedMeshRenderer
            GameObject smrObject = new GameObject($"{_characterName}_SkinnedMesh");
            smrObject.transform.SetParent(transform, false);
            _skinnedMeshRenderer = smrObject.AddComponent<SkinnedMeshRenderer>();
            _skinnedMeshRenderer.sharedMesh = generatedMesh.Mesh;
            _skinnedMeshRenderer.materials = generatedMesh.Materials;
            _skinnedMeshRenderer.bones = _rigResult.BoneTransforms;
            _skinnedMeshRenderer.rootBone = _rigResult.Bones[HumanoidBoneType.Hips];
            _skinnedMeshRenderer.quality = SkinQuality.Bone4; // Mobile High 4-bone quality
            _skinnedMeshRenderer.updateWhenOffscreen = false;

            // 5. Attach Anatomical Precision Hitboxes (routes to HealthComponent)
            AttachAnatomicalHitboxes();

            // 6. Setup Animation Controller
            _animationController = gameObject.AddComponent<CharacterAnimationController>();
            _animationController.Initialize(
                _rigResult.Bones[HumanoidBoneType.Chest],
                _rigResult.Bones[HumanoidBoneType.Spine],
                _rigResult.Bones[HumanoidBoneType.Head],
                _rigResult.Bones[HumanoidBoneType.RightUpperArm],
                _rigResult.Bones[HumanoidBoneType.LeftUpperArm]
            );

            Debug.Log($"[EthanCrossCharacter] Successfully assembled anatomical model for {_characterName} with {_rigResult.BoneTransforms.Length} bones and 4-submesh PBR materials.");
        }

        private void AttachAnatomicalHitboxes()
        {
            if (_rigResult == null || _healthComponent == null) return;

            // Head Hitbox (2.5x critical damage)
            AttachHitboxToBone(_rigResult.Bones[HumanoidBoneType.Head], HitLocation.Head, new Vector3(0.18f, 0.22f, 0.20f), Vector3.zero);

            // Chest / Torso Hitbox (Standard damage)
            AttachHitboxToBone(_rigResult.Bones[HumanoidBoneType.Chest], HitLocation.Chest, new Vector3(0.38f, 0.32f, 0.25f), Vector3.zero);

            // Stomach / Hips Hitbox
            AttachHitboxToBone(_rigResult.Bones[HumanoidBoneType.Hips], HitLocation.Stomach, new Vector3(0.32f, 0.25f, 0.22f), Vector3.zero);

            // Arms (0.75x damage)
            AttachHitboxToBone(_rigResult.Bones[HumanoidBoneType.LeftUpperArm], HitLocation.ArmLeft, new Vector3(0.12f, 0.28f, 0.12f), Vector3.zero);
            AttachHitboxToBone(_rigResult.Bones[HumanoidBoneType.RightUpperArm], HitLocation.ArmRight, new Vector3(0.12f, 0.28f, 0.12f), Vector3.zero);

            // Legs (0.75x damage)
            AttachHitboxToBone(_rigResult.Bones[HumanoidBoneType.LeftUpperLeg], HitLocation.LegLeft, new Vector3(0.14f, 0.40f, 0.14f), Vector3.zero);
            AttachHitboxToBone(_rigResult.Bones[HumanoidBoneType.RightUpperLeg], HitLocation.LegRight, new Vector3(0.14f, 0.40f, 0.14f), Vector3.zero);
        }

        private void AttachHitboxToBone(Transform bone, HitLocation location, Vector3 size, Vector3 centerOffset)
        {
            if (bone == null) return;

            var col = bone.gameObject.AddComponent<BoxCollider>();
            col.size = size;
            col.center = centerOffset;
            col.isTrigger = false;

            var hitBox = bone.gameObject.AddComponent<HitBox>();
            hitBox.Initialize(_healthComponent, location);
        }
    }
}
