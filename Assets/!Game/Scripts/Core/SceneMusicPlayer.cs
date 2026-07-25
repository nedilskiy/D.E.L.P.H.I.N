using Delphin.Services;
using UnityEngine;

namespace Delphin.Core
{
    public class SceneMusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip musicClip;
        [SerializeField] private bool loop = true;
        [SerializeField, Range(0f, 1f)] private float volume = 1f;

        private void Start()
        {
            ServiceLocator.Get<IAudioService>().PlayMusic(musicClip, loop, volume);
        }
    }
}
