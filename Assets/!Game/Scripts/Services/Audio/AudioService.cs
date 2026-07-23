using UnityEngine;

namespace Delphin.Services
{
    public sealed class AudioService : IAudioService
    {
        private const int SfxPoolSize = 8;

        private GameObject root;
        private AudioSource musicSource;
        private AudioSource[] sfxPool;
        private int nextSfxIndex;

        public void Initialize()
        {
            root = new GameObject("[AudioService]");
            Object.DontDestroyOnLoad(root);

            musicSource = root.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;

            sfxPool = new AudioSource[SfxPoolSize];
            for (var i = 0; i < SfxPoolSize; i++)
            {
                var source = root.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 0f;
                sfxPool[i] = source;
            }
        }

        public void PlaySfx(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;

            NextSfxSource().PlayOneShot(clip, volume);
        }

        public void PlaySfxAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip == null)
                return;

            AudioSource.PlayClipAtPoint(clip, position, volume);
        }

        public void PlayMusic(AudioClip clip, bool loop = true, float volume = 1f)
        {
            if (clip == null)
                return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = volume;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void SetMasterVolume(float volume)
        {
            AudioListener.volume = Mathf.Clamp01(volume);
        }

        public void Shutdown()
        {
            if (root != null)
                Object.Destroy(root);
        }

        private AudioSource NextSfxSource()
        {
            var source = sfxPool[nextSfxIndex];
            nextSfxIndex = (nextSfxIndex + 1) % sfxPool.Length;
            return source;
        }
    }
}
