using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Eclipse.Mobile
{
    public class MobileCameraLook : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("Sensitivity")]
        [Range(0.1f, 5.0f)]
        [SerializeField] private float _horizontalSensitivity = 1.8f;
        [Range(0.1f, 5.0f)]
        [SerializeField] private float _verticalSensitivity = 1.4f;
        [SerializeField] private bool _invertY = false;

        private Vector2 _lookDelta;
        private Vector2 _previousPointerPosition;
        private int _activePointerId = -1;

        public Vector2 LookDelta => _lookDelta;
        public float HorizontalSensitivity { get => _horizontalSensitivity; set => _horizontalSensitivity = Mathf.Clamp(value, 0.2f, 5f); }
        public float VerticalSensitivity { get => _verticalSensitivity; set => _verticalSensitivity = Mathf.Clamp(value, 0.2f, 5f); }
        public bool InvertY { get => _invertY; set => _invertY = value; }

        public event Action<Vector2> OnLookDeltaChanged;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_activePointerId != -1) return; // Multitouch isolation

            _activePointerId = eventData.pointerId;
            _previousPointerPosition = eventData.position;
            _lookDelta = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId) return;

            Vector2 currentPosition = eventData.position;
            Vector2 rawDelta = currentPosition - _previousPointerPosition;
            _previousPointerPosition = currentPosition;

            float yFactor = _invertY ? 1f : -1f;

            _lookDelta = new Vector2(
                rawDelta.x * _horizontalSensitivity * 0.1f,
                rawDelta.y * _verticalSensitivity * 0.1f * yFactor
            );

            OnLookDeltaChanged?.Invoke(_lookDelta);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId) return;

            _activePointerId = -1;
            _lookDelta = Vector2.zero;
            OnLookDeltaChanged?.Invoke(Vector2.zero);
        }

        private void LateUpdate()
        {
            // Reset delta after frame consumption to prevent residual drift
            if (_activePointerId != -1)
            {
                _lookDelta = Vector2.zero;
            }
        }
    }
}
