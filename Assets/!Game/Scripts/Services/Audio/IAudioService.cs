using Delphin.Core;
using UnityEngine;

namespace Delphin.Services
{
    public interface IAudioService : IGameService
    {
        float MasterVolume { get; }
        float MusicVolume { get; }
        float SfxVolume { get; }
        float DialogueVolume { get; }

        void PlaySfx(AudioClip clip, float volume = 1f);
        void PlaySfxAtPoint(AudioClip clip, Vector3 position, float volume = 1f);
        void PlayMusic(AudioClip clip, bool loop = true, float volume = 1f);
        void StopMusic();
        void PlayDialogue(AudioClip clip, float volume = 1f);
        void StopDialogue();

        void SetMasterVolume(float volume);
        void SetMusicVolume(float volume);
        void SetSfxVolume(float volume);
        void SetDialogueVolume(float volume);
    }
}
