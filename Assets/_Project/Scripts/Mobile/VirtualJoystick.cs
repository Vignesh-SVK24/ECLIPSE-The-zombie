using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Eclipse.Mobile
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("UI RectTransforms")]
        [SerializeField] private RectTransform _backgroundRect;
        [SerializeField] private RectTransform _handleRect;

        [Header("Joystick Settings")]
        [Range(20f, 150f)]
        [SerializeField] private float _movementRange = 75f;
        [Range(0f, 0.5f)]
        [SerializeField] private float _deadZone = 0.1f;
        [SerializeField] private bool _dynamicPosition = false;

        private Vector2 _inputDirection = Vector2.zero;
        private Canvas _parentCanvas;
        private Camera _uiCamera;
        private Vector2 _defaultBackgroundPosition;
        private int _activePointerId = -1;

        public Vector2 InputDirection => _inputDirection;
        public float Horizontal => _inputDirection.x;
        public float Vertical => _inputDirection.y;

        public event Action<Vector2> OnDirectionChanged;

        private void Awake()
        {
            if (_backgroundRect == null) _backgroundRect = GetComponent<RectTransform>();
            _parentCanvas = GetComponentInParent<Canvas>();
            _uiCamera = _parentCanvas != null && _parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay ? _parentCanvas.worldCamera : null;
            _defaultBackgroundPosition = _backgroundRect.anchoredPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_activePointerId != -1) return; // Multitouch lock: Ignore second touch on this joystick
            _activePointerId = eventData.pointerId;

            if (_dynamicPosition)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _parentCanvas.transform as RectTransform,
                    eventData.position,
                    _uiCamera,
                    out var localPoint);
                _backgroundRect.anchoredPosition = localPoint;
            }

            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _backgroundRect,
                eventData.position,
                _uiCamera,
                out var position);

            Vector2 clampedPosition = Vector2.ClampMagnitude(position, _movementRange);

            if (_handleRect != null)
            {
                _handleRect.anchoredPosition = clampedPosition;
            }

            Vector2 rawDirection = clampedPosition / _movementRange;

            if (rawDirection.magnitude > _deadZone)
            {
                // Smooth deadzone normalization
                _inputDirection = rawDirection;
            }
            else
            {
                _inputDirection = Vector2.zero;
            }

            OnDirectionChanged?.Invoke(_inputDirection);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != _activePointerId) return;
            _activePointerId = -1;

            _inputDirection = Vector2.zero;

            if (_handleRect != null)
            {
                _handleRect.anchoredPosition = Vector2.zero;
            }

            if (_dynamicPosition)
            {
                _backgroundRect.anchoredPosition = _defaultBackgroundPosition;
            }

            OnDirectionChanged?.Invoke(Vector2.zero);
        }

        public void SetScale(float scaleMultiplier)
        {
            transform.localScale = Vector3.one * Mathf.Clamp(scaleMultiplier, 0.6f, 1.6f);
        }
    }
}
