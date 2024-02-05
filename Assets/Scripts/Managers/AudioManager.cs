using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DriftTT
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Mixer Parameters")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private float onValue = 0f;
        [SerializeField] private float muteValue = -80.0f;
        [Header("UI Elements")]
        [SerializeField] private Toggle audioToggle;

        private const string AudioVolumeKey = "audioKey";
        private void Awake()
        {
            audioToggle.onValueChanged.AddListener(HandleSoundsToggleChanged);
        }
        private void Start()
        {
            if (!PlayerPrefs.HasKey(AudioVolumeKey))
            {
                PlayerPrefs.SetFloat(AudioVolumeKey, onValue);
            }
            audioMixer.SetFloat(AudioVolumeKey, PlayerPrefs.GetFloat(AudioVolumeKey));
            CheckTogglers();
        }

        private void CheckTogglers()
        {
            audioToggle.isOn = PlayerPrefs.GetFloat(AudioVolumeKey) != muteValue;
        }

        private void HandleSoundsToggleChanged(bool enableAudio)
        {
            float volume;
            if (enableAudio)
            {
                volume = onValue;
                audioMixer.SetFloat(AudioVolumeKey, volume);
            }
            else
            {
                volume = muteValue;
                audioMixer.SetFloat(AudioVolumeKey, volume);
            }
            PlayerPrefs.SetFloat(AudioVolumeKey, volume);
        }
    }
}
