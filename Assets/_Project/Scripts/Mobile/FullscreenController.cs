using UnityEngine;
using UnityEngine.UI;

namespace Eclipse.Mobile
{
    /// <summary>
    /// Controls cross-platform Fullscreen mode for WebGL, Mobile, and Desktop Standalone.
    /// Provides toggle functionality via UI button and keyboard shortcut (F / F11).
    /// </summary>
    public class FullscreenController : MonoBehaviour
    {
        [Header("UI Reference")]
        [SerializeField] private Button _fullscreenButton;
        [SerializeField] private Text _label;

        private void Start()
        {
            if (_fullscreenButton != null)
            {
                _fullscreenButton.onClick.AddListener(ToggleFullscreen);
            }
            UpdateLabel();
        }

        private void Update()
        {
            // Keyboard shortcut on desktop / laptop
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.F11))
            {
                ToggleFullscreen();
            }
        }

        public void ToggleFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            if (_label != null)
            {
                _label.text = Screen.fullScreen ? "EXIT FS" : "FULLSCREEN";
            }
        }
    }
}
