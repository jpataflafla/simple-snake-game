using UnityEngine;

namespace SnakeGame
{
    public interface IAudioSourcePool
    {
        public AudioSource GetFreeAudioSource();
    }
}
