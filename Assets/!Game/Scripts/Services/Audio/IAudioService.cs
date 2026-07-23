using Delphin.Core;
using UnityEngine;

namespace Delphin.Services
{
    public interface IAudioService : IGameService
    {
        void PlaySfx(AudioClip clip, float volume = 1f);
        void PlaySfxAtPoint(AudioClip clip, Vector3 position, float volume = 1f);
        void PlayMusic(AudioClip clip, bool loop = true, float volume = 1f);
        void StopMusic();
        void SetMasterVolume(float volume);
    }
}
