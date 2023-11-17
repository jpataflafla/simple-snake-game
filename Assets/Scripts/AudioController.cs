using System.Collections;
using UnityEngine;

namespace SnakeGame
{

    public class AudioController : MonoBehaviour, IAudioController
    {
        [SerializeField] private AudioClip _backgroundClip;
        private AudioSource _backgroundAudioSource;
        [SerializeField] private AudioClip _edibleItemSound;
        [SerializeField] private AudioClip _inedibleItemSound;
        [SerializeField] private AudioClip _speedUpItemSound;
        [SerializeField] private AudioClip _slowDownItemSound;
        [SerializeField] private AudioClip _headTailSwapSound;
        [SerializeField] private AudioClip _gameOverSound;

        private readonly int _audioSourcePoolSize = 5;
        private IAudioSourcePool _audioSourcePool;

        private void Awake()
        {
            _audioSourcePool = new AudioSourcePool(_audioSourcePoolSize, this.transform);
        }

        public void PlayBackgroundLoop(float volume = 1f)
        {
            var audioSource = _audioSourcePool.GetFreeAudioSource();
            audioSource.clip = _backgroundClip;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.Play();
            _backgroundAudioSource = audioSource;
        }

        public void StopBackgroundLoop()
        {
            _backgroundAudioSource.Stop();
            _backgroundAudioSource = null;
        }

        public void PlayEdibleItemSound(float volume = 1f)
        {
            PlayEffect(_edibleItemSound, volume);
        }

        public void PlayGameOverSound(float volume = 1f)
        {
            PlayEffect(_gameOverSound, volume);
        }

        public void PlayInedibleItemSound(float volume = 1)
        {
            PlayEffect(_inedibleItemSound, volume);
        }

        public void PlaySpeedUpItemSound(float volume = 1)
        {
            PlayEffect(_speedUpItemSound, volume);
        }

        public void PlaySlowDownItemSound(float volume = 1)
        {
            PlayEffect(_slowDownItemSound, volume);
        }

        public void PlayHeadTailSwapItemSound(float volume = 1)
        {
            PlayEffect(_headTailSwapSound, volume);
        }

        private void PlayEffect(AudioClip clip, float volume)
        {
            var audioSource = _audioSourcePool.GetFreeAudioSource();
            audioSource.Stop();
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
