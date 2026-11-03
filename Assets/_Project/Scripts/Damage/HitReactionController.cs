using System.Collections;
using UnityEngine;

namespace Eclipse.Damage
{
    public class HitReactionController : MonoBehaviour
    {
        [Header("Damage Source Binding")]
        [SerializeField] private MonoBehaviour _damageableSource;

        [Header("Flash Feedback")]
        [SerializeField] private Renderer[] _renderers;
        [SerializeField] private Color _flashColor = new Color(1f, 0.2f, 0.2f, 1f);
        [SerializeField] private float _flashDuration = 0.12f;

        [Header("Physics Impulse")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _impulseStrength = 3.5f;

        private IDamageable _damageable;
        private MaterialPropertyBlock _propBlock;
        private Coroutine _flashRoutine;
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            _propBlock = new MaterialPropertyBlock();

            if (_damageableSource is IDamageable explicitDamageable)
            {
                _damageable = explicitDamageable;
            }
            else
            {
                _damageable = GetComponent<IDamageable>();
            }

            if (_renderers == null || _renderers.Length == 0)
            {
                _renderers = GetComponentsInChildren<Renderer>();
            }

            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
        }

        private void OnEnable()
        {
            if (_damageable != null)
            {
                _damageable.OnDamageTaken += HandleDamageTaken;
            }
        }

        private void OnDisable()
        {
            if (_damageable != null)
            {
                _damageable.OnDamageTaken -= HandleDamageTaken;
            }
        }

        private void HandleDamageTaken(DamageInfo info)
        {
            // Trigger visual flash
            if (_renderers != null && _renderers.Length > 0 && gameObject.activeInHierarchy)
            {
                if (_flashRoutine != null)
                {
                    StopCoroutine(_flashRoutine);
                }
                _flashRoutine = StartCoroutine(FlashRoutine());
            }

            // Apply physics impulse if directional
            if (_rigidbody != null && !_rigidbody.isKinematic && info.HitDirection != Vector3.zero)
            {
                _rigidbody.AddForce(info.HitDirection.normalized * _impulseStrength, ForceMode.Impulse);
            }
        }

        private IEnumerator FlashRoutine()
        {
            SetFlashColor(_flashColor);
            yield return new WaitForSeconds(_flashDuration);
            ResetFlashColor();
            _flashRoutine = null;
        }

        private void SetFlashColor(Color color)
        {
            foreach (var r in _renderers)
            {
                if (r == null) continue;
                r.GetPropertyBlock(_propBlock);
                _propBlock.SetColor(EmissionColorId, color);
                r.SetPropertyBlock(_propBlock);
            }
        }

        private void ResetFlashColor()
        {
            foreach (var r in _renderers)
            {
                if (r == null) continue;
                r.GetPropertyBlock(_propBlock);
                _propBlock.SetColor(EmissionColorId, Color.black);
                r.SetPropertyBlock(_propBlock);
            }
        }
    }
}
