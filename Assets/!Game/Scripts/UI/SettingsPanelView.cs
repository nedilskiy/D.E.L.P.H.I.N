using Delphin.Core;
using Delphin.Services;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Delphin.UI
{
    public class SettingsPanelView : MonoBehaviour
    {
        [FormerlySerializedAs("volumeSlider")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider dialogueVolumeSlider;

        private IAudioService audio;

        private void Awake()
        {
            audio = ServiceLocator.Get<IAudioService>();

            masterVolumeSlider.onValueChanged.AddListener(audio.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(audio.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(audio.SetSfxVolume);
            dialogueVolumeSlider.onValueChanged.AddListener(audio.SetDialogueVolume);
        }

        private void OnEnable()
        {
            masterVolumeSlider.SetValueWithoutNotify(audio.MasterVolume);
            musicVolumeSlider.SetValueWithoutNotify(audio.MusicVolume);
            sfxVolumeSlider.SetValueWithoutNotify(audio.SfxVolume);
            dialogueVolumeSlider.SetValueWithoutNotify(audio.DialogueVolume);
        }
    }
}
