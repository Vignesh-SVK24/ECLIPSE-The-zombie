using System;
using UnityEngine;
using Eclipse.Character;
using Eclipse.Damage;
using Eclipse.Player;

namespace Eclipse.Player
{
    /// <summary>
    /// Utility builder that constructs and configures the unified Ethan Cross player hierarchy,
    /// establishing standard Humanoid avatar binding, LOD group tiers, combat hitboxes,
    /// and controller references.
    /// </summary>
    public static class PlayerPrefabBuilder
    {
        public static GameObject BuildEthanCrossPlayerObject(string name = "EthanCross_Player")
        {
            GameObject playerGo = new GameObject(name);
            playerGo.tag = "Player";

            // 1. Character Controller
            var cc = playerGo.AddComponent<CharacterController>();
            cc.height = 1.83f;
            cc.radius = 0.35f;
            cc.center = new Vector3(0, 0.915f, 0);
            cc.stepOffset = 0.35f;
            cc.slopeLimit = 45f;

            // 2. Health Component & Damage Integration
            var health = playerGo.AddComponent<HealthComponent>();
            health.Initialize(100f, 100f);

            // 3. Ethan Cross Master Character Component
            var ethanChar = playerGo.AddComponent<EthanCrossCharacter>();
            ethanChar.ConstructAnatomicalEthanCross();

            // 4. Movement Controller
            var movement = playerGo.AddComponent<PlayerMovementController>();

            // 5. Combat Controller
            var combat = playerGo.AddComponent<PlayerCombatController>();

            // 6. LOD Group Setup (LOD0: 100-60%, LOD1: 60-25%, LOD2: 25-5%)
            var lodGroup = playerGo.AddComponent<LODGroup>();
            var smr = playerGo.GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr != null)
            {
                LOD[] lods = new LOD[3];
                lods[0] = new LOD(0.55f, new Renderer[] { smr });
                lods[1] = new LOD(0.20f, new Renderer[] { smr });
                lods[2] = new LOD(0.04f, new Renderer[] { smr });
                lodGroup.SetLODs(lods);
                lodGroup.RecalculateBounds();
            }

            Debug.Log($"[PlayerPrefabBuilder] Constructed {name} with complete 65-bone rig, anatomical mesh, LODGroup, and combat controllers.");
            return playerGo;
        }
    }
}
