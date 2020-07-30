using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using Cogwheel.Save;

namespace Cogwheel.UI
{
    [RequireComponent(typeof(SystemSettings))]
    public class GraphicsSettings : UIElement
    {
        public AudioMixer masterMixer;
        SystemSettings settings;

        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown qualityDropdown;
        public TMP_Dropdown fullscreenDropdown;
        public TMP_Dropdown vsyncDropdown;

        public Slider masterSlider;
        public Slider musicSlider;
        public Slider effectsSlider;

        Resolution[] resolutions;

        // Start is called before the first frame update
        protected void Start()
        {
            settings = GetComponent<SystemSettings>();

            if (settings && SaveSystemXml.CheckFileExsits())
            {
                SystemSettingsData settingsData = SaveSystemXml.LoadSettings();
                settings.fullscreen = settingsData.Fullscreen;
                settings.resolution = settingsData.Resolution;
                settings.vsyncCount = settingsData.VsyncCount;
                settings.graphicsPreset = settingsData.GraphicsPreset;
                settings.masterVolume = settingsData.MasterVolume;
                settings.effectsVolume = settingsData.EffectsVolume;
            }
            else
            {
                SaveSystemXml.SaveSettings(settings);
            }

            if (resolutionDropdown)
            {
                FindSupportedResolutions();
                Screen.SetResolution(settings.resolution[0], settings.resolution[1], Screen.fullScreen);
            }

            if (qualityDropdown)
            {
                qualityDropdown.value = settings.graphicsPreset;
                qualityDropdown.RefreshShownValue();
                QualitySettings.SetQualityLevel(settings.graphicsPreset);
            }

            if (fullscreenDropdown)
            {
                if (settings.fullscreen == true)
                {
                    fullscreenDropdown.value = 1;
                    fullscreenDropdown.RefreshShownValue();
                }
                else
                {
                    fullscreenDropdown.value = 0;
                    fullscreenDropdown.RefreshShownValue();
                }

                Screen.fullScreen = settings.fullscreen;
            }

            if (vsyncDropdown)
            {
                vsyncDropdown.value = settings.vsyncCount;
                vsyncDropdown.RefreshShownValue();
                QualitySettings.vSyncCount = settings.vsyncCount;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Custom Functions

        public void SetResolution(int ResolutionIndex)
        {
            Resolution resolution = resolutions[ResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            settings.resolution = new int[] {resolution.width, resolution.height};
            SaveSystemSettings();
        }

        public void SetFullscreen(int Fullscreen)
        {
            if (Fullscreen == 1)
            {
                Screen.fullScreen = true;
                settings.fullscreen = true;
            }
            else if (Fullscreen == 0)
            {
                Screen.fullScreen = false;
                settings.fullscreen = false;
            }

            SaveSystemSettings();
        }

        public void SetQualityLevel(int Level)
        {
            QualitySettings.SetQualityLevel(Level);
            settings.graphicsPreset = Level;
            SaveSystemSettings();
        }

        public void SetVsyncMode(int Mode)
        {
            QualitySettings.vSyncCount = Mode;
            settings.vsyncCount = Mode;
            SaveSystemSettings();
        }

        void FindSupportedResolutions()
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int CurrentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == settings.resolution[0] && resolutions[i].height == settings.resolution[1])
                {
                    CurrentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = CurrentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SaveSystemSettings()
        {
            SaveSystemXml.SaveSettings(settings);
        }
        #endregion
    }
}
