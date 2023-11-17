using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class AudioSourcePool : IAudioSourcePool
    {
        private List<AudioSource> _audioSourcePool = new List<AudioSource>();
        private Transform _parentObject;

        public AudioSourcePool(int audioSourcePoolSize, Transform parentObject) 
        {
            _parentObject = parentObject;
            InitializeAudioSourcePool(audioSourcePoolSize);
        }

        public AudioSource GetFreeAudioSource()
        {
            foreach (var audioSourceTransform in _audioSourcePool)
            {
                var audioSource = audioSourceTransform.GetComponent<AudioSource>();
                if (!audioSource.isPlaying)
                {
                    return audioSource;
                }
            }

            return AddAudioSourceToPool();
        }

        private void InitializeAudioSourcePool(int audioSourcePoolSize)
        {
            for (int i = 0; i < audioSourcePoolSize; i++)
            {
                AddAudioSourceToPool();
            }
        }

        private AudioSource AddAudioSourceToPool()
        {
            var index = _audioSourcePool.Count;
            var newObject = new GameObject($"AudioSource{index}");
            newObject.transform.parent = _parentObject.transform;
            var audioSource = newObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            _audioSourcePool.Add(audioSource);
            return audioSource;
        }
    }
}
