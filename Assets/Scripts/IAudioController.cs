namespace SnakeGame
{
    public interface IAudioController
    {
        void PlayBackgroundLoop(float volume = 1f);
        void StopBackgroundLoop();
        void PlayGameOverSound(float volume = 1f);
        void PlayEdibleItemSound(float volume = 1f);
        void PlayInedibleItemSound(float volume = 1f);
        void PlaySpeedUpItemSound(float volume = 1f);
        void PlaySlowDownItemSound(float volume = 1f);
        void PlayHeadTailSwapItemSound(float volume = 1f);
    }
}
