using System;
using UnityEngine;

namespace Eclipse.AI
{
    public class ZombieHearing : MonoBehaviour
    {
        [Header("Hearing Settings")]
        [SerializeField] private float _sensitivity = 1.0f;
        [SerializeField] private bool _canHearThroughWalls = false;
        [SerializeField] private LayerMask _obstructionMask = ~0;

        public event Action<Vector3, SoundCategory> OnSoundHeard;

        public float Sensitivity { get => _sensitivity; set => _sensitivity = Mathf.Max(0.1f, value); }

        private void OnEnable()
        {
            SoundEventBus.OnSoundEmitted += HandleSoundEmitted;
        }

        private void OnDisable()
        {
            SoundEventBus.OnSoundEmitted -= HandleSoundEmitted;
        }

        private void HandleSoundEmitted(SoundEvent sound)
        {
            if (sound.Source == gameObject) return;

            float distance = Vector3.Distance(transform.position, sound.Position);
            float effectiveRadius = sound.Radius * _sensitivity;

            if (distance <= effectiveRadius)
            {
                // If wall dampening applies, check raycast line of sound
                if (!_canHearThroughWalls)
                {
                    if (Physics.Linecast(transform.position + Vector3.up * 1.5f, sound.Position, out var hit, _obstructionMask))
                    {
                        if (hit.collider.gameObject != sound.Source)
                        {
                            // Attenuate sound through walls by 60%
                            effectiveRadius *= 0.4f;
                            if (distance > effectiveRadius) return;
                        }
                    }
                }

                OnSoundHeard?.Invoke(sound.Position, sound.Category);
            }
        }
    }
}
