using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Cogwheel.Save
{
    public class SystemSettings : MonoBehaviour
    {
        public AudioMixer masterMixer;

        public bool fullscreen;
        public int[] resolution;
        public int vsyncCount;
        public int graphicsPreset;
        public float masterVolume;
        public float effectsVolume;

        private void Awake()
        {
            fullscreen = Screen.fullScreen;
            resolution = new int[] { Screen.currentResolution.width, Screen.currentResolution.height };
            vsyncCount = QualitySettings.vSyncCount;
            graphicsPreset = QualitySettings.GetQualityLevel();

            masterMixer.GetFloat("MasterVol", out masterVolume);
            masterMixer.GetFloat("EffectsVol", out effectsVolume);
        }
    }
}