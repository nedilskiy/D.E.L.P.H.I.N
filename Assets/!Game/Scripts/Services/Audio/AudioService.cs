using UnityEngine;
using UnityEngine.Audio;

namespace Delphin.Services
{
    public sealed class AudioService : IAudioService
    {
        private const int SfxPoolSize = 8;

        private const string MusicVolumeParam = "MusicVolume";
        private const string SfxVolumeParam = "SFXVolume";
        private const string DialogueVolumeParam = "DialogueVolume";

        private const string MasterVolumeKey = "Audio.MasterVolume";
        private const string MusicVolumeKey = "Audio.MusicVolume";
        private const string SfxVolumeKey = "Audio.SfxVolume";
        private const string DialogueVolumeKey = "Audio.DialogueVolume";

        private readonly AudioMixer mixer;
        private readonly AudioMixerGroup musicGroup;
        private readonly AudioMixerGroup sfxGroup;
        private readonly AudioMixerGroup dialogueGroup;

        private GameObject root;
        private AudioSource musicSource;
        private AudioSource dialogueSource;
        private AudioSource[] sfxPool;
        private int nextSfxIndex;

        public float MasterVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float SfxVolume { get; private set; }
        public float DialogueVolume { get; private set; }

        public AudioService(AudioMixer mixer, AudioMixerGroup musicGroup, AudioMixerGroup sfxGroup, AudioMixerGroup dialogueGroup)
        {
            this.mixer = mixer;
            this.musicGroup = musicGroup;
            this.sfxGroup = sfxGroup;
            this.dialogueGroup = dialogueGroup;
        }

        public void Initialize()
        {
            root = new GameObject("[AudioService]");
            Object.DontDestroyOnLoad(root);

            musicSource = root.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
            musicSource.outputAudioMixerGroup = musicGroup;

            dialogueSource = root.AddComponent<AudioSource>();
            dialogueSource.playOnAwake = false;
            dialogueSource.spatialBlend = 0f;
            dialogueSource.outputAudioMixerGroup = dialogueGroup;

            sfxPool = new AudioSource[SfxPoolSize];
            for (var i = 0; i < SfxPoolSize; i++)
            {
                var source = root.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 0f;
                source.outputAudioMixerGroup = sfxGroup;
                sfxPool[i] = source;
            }

            SetMasterVolume(PlayerPrefs.GetFloat(MasterVolumeKey, 1f));
            SetMusicVolume(PlayerPrefs.GetFloat(MusicVolumeKey, 1f));
            SetSfxVolume(PlayerPrefs.GetFloat(SfxVolumeKey, 1f));
            SetDialogueVolume(PlayerPrefs.GetFloat(DialogueVolumeKey, 1f));
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

        public void PlayDialogue(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;

            dialogueSource.clip = clip;
            dialogueSource.volume = volume;
            dialogueSource.Play();
        }

        public void StopDialogue()
        {
            dialogueSource.Stop();
        }

        public void SetMasterVolume(float volume)
        {
            MasterVolume = Mathf.Clamp01(volume);
            AudioListener.volume = MasterVolume;
            PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp01(volume);
            mixer.SetFloat(MusicVolumeParam, LinearToDecibel(MusicVolume));
            PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
        }

        public void SetSfxVolume(float volume)
        {
            SfxVolume = Mathf.Clamp01(volume);
            mixer.SetFloat(SfxVolumeParam, LinearToDecibel(SfxVolume));
            PlayerPrefs.SetFloat(SfxVolumeKey, SfxVolume);
        }

        public void SetDialogueVolume(float volume)
        {
            DialogueVolume = Mathf.Clamp01(volume);
            mixer.SetFloat(DialogueVolumeParam, LinearToDecibel(DialogueVolume));
            PlayerPrefs.SetFloat(DialogueVolumeKey, DialogueVolume);
        }

        public void Shutdown()
        {
            PlayerPrefs.Save();

            if (root != null)
                Object.Destroy(root);
        }

        private AudioSource NextSfxSource()
        {
            var source = sfxPool[nextSfxIndex];
            nextSfxIndex = (nextSfxIndex + 1) % sfxPool.Length;
            return source;
        }

        private static float LinearToDecibel(float linear)
        {
            return linear <= 0.0001f ? -80f : Mathf.Log10(linear) * 20f;
        }
    }
}
