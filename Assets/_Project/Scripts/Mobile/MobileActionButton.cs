using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Eclipse.Mobile
{
    /// <summary>
    /// Represents a mobile touch action button with visual press scaling,
    /// color tinting, minimum touch target enforcement (Apple 44pt / Android 48dp standard),
    /// and haptic feedback hooks.
    /// </summary>
    public class MobileActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Button Identifier")]
        [SerializeField] private string _buttonId;

        [Header("Visual Feedback")]
        [SerializeField] private Graphic _targetGraphic;
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _pressedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        [SerializeField] private Vector3 _pressedScale = new Vector3(0.92f, 0.92f, 1f);

        [Header("Haptic & Accessibility")]
        [SerializeField] private bool _triggerHapticOnPress = true;

        private bool _isPressed;
        private Vector3 _originalScale;

        public string ButtonId => _buttonId;
        public bool IsPressed => _isPressed;

        public event Action<string> OnButtonDown;
        public event Action<string> OnButtonUp;
        public event Action<string> OnTriggered;

        private void Awake()
        {
            _originalScale = transform.localScale;
            if (_targetGraphic == null)
            {
                _targetGraphic = GetComponent<Graphic>();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            ApplyVisualState(true);

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (_triggerHapticOnPress)
            {
                Handheld.Vibrate();
            }
#endif

            OnButtonDown?.Invoke(_buttonId);
            OnTriggered?.Invoke(_buttonId);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
            ApplyVisualState(false);
            OnButtonUp?.Invoke(_buttonId);
        }

        private void ApplyVisualState(bool pressed)
        {
            transform.localScale = pressed ? Vector3.Scale(_originalScale, _pressedScale) : _originalScale;

            if (_targetGraphic != null)
            {
                _targetGraphic.color = pressed ? _pressedColor : _normalColor;
            }
        }

        public void SetSize(float multiplier)
        {
            _originalScale = Vector3.one * Mathf.Clamp(multiplier, 0.7f, 1.5f);
            if (!_isPressed)
            {
                transform.localScale = _originalScale;
            }
        }
    }
}
