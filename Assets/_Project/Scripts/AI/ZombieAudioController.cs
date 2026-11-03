using UnityEngine;

namespace Eclipse.AI
{
    [RequireComponent(typeof(AudioSource))]
    public class ZombieAudioController : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource _audioSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] _idleGroans;
        [SerializeField] private AudioClip[] _alertShrieks;
        [SerializeField] private AudioClip[] _attackSounds;
        [SerializeField] private AudioClip[] _hitFlinches;
        [SerializeField] private AudioClip[] _deathScreams;

        [Header("Idle Groan Interval")]
        [SerializeField] private float _minGroanInterval = 4.0f;
        [SerializeField] private float _maxGroanInterval = 10.0f;

        private float _groanTimer;
        private float _nextGroanDelay;

        private void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            // Configure 3D spatial settings for mobile
            _audioSource.spatialBlend = 1.0f; // 3D sound
            _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            _audioSource.minDistance = 1.5f;
            _audioSource.maxDistance = 25.0f;
            _audioSource.playOnAwake = false;

            ResetGroanTimer();
        }

        private void Update()
        {
            _groanTimer += Time.deltaTime;
            if (_groanTimer >= _nextGroanDelay)
            {
                PlayRandomClip(_idleGroans, 0.4f);
                ResetGroanTimer();
            }
        }

        public void PlayAlert()
        {
            PlayRandomClip(_alertShrieks, 0.8f);
        }

        public void PlayAttack()
        {
            PlayRandomClip(_attackSounds, 0.85f);
        }

        public void PlayHit()
        {
            PlayRandomClip(_hitFlinches, 0.7f);
        }

        public void PlayDeath()
        {
            PlayRandomClip(_deathScreams, 0.9f);
        }

        private void PlayRandomClip(AudioClip[] clips, float volume)
        {
            if (clips == null || clips.Length == 0 || _audioSource == null) return;

            AudioClip clip = clips[Random.Range(0, clips.Length)];
            if (clip != null)
            {
                _audioSource.pitch = Random.Range(0.88f, 1.12f); // Pitch variation
                _audioSource.PlayOneShot(clip, volume);
            }
        }

        private void ResetGroanTimer()
        {
            _groanTimer = 0f;
            _nextGroanDelay = Random.Range(_minGroanInterval, _maxGroanInterval);
        }
    }
}
