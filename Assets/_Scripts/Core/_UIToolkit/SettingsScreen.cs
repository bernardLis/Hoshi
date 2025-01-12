using System.Collections.Generic;
using BountyBalance.Core;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace BountyBalance
{
    public class SettingsScreen : FullScreenElement
    {
        const string _ussCommonEvenBackground = "common__even-background";
        const string _ussCommonOddBackground = "common__odd-background";

        const string _ussClassName = "settings-screen__";
        const string _ussMain = _ussClassName + "main";
        const string _ussUiContainer = _ussClassName + "ui-container";
        const string _ussTitle = _ussClassName + "title";
        const string _ussVolumeSlider = _ussClassName + "volume-slider";

        readonly AudioManager _audioManger;

        readonly VisualElement _container;

        Toggle _fullScreenToggle;

        public SettingsScreen()
        {
            _audioManger = AudioManager.Instance;

            _container = new();
            _container.AddToClassList(_ussMain);
            Content.Add(_container);

            SetTitle("Settings");

            AddSoundOptions();
            AddGraphicsOptions();
            AddFpsCounterToggle();

            AddContinueButton();
        }

        void AddFpsCounterToggle()
        {
            Toggle fpsCounterToggle = new();
            fpsCounterToggle.label = "FPS Counter";
            fpsCounterToggle.value = PlayerPrefs.GetInt("fpsCounter", 0) != 0;
            fpsCounterToggle.RegisterValueChangedCallback(FpsCounterToggleClick);
            Content.Add(fpsCounterToggle);
        }

        void FpsCounterToggleClick(ChangeEvent<bool> evt)
        {
            PlayerPrefs.SetInt("fpsCounter", evt.newValue ? 1 : 0);
            GameManager.GetComponent<FpsDisplayer>().UpdateVisibility();
        }

        void AddSoundOptions()
        {
            VisualElement soundContainer = new();
            soundContainer.AddToClassList(_ussCommonEvenBackground);
            soundContainer.AddToClassList(_ussUiContainer);
            Label sound = new("Sound");
            sound.AddToClassList(_ussTitle);

            // sound.AddToClassList(_ussCommonTextPrimary);
            soundContainer.Add(sound);
            _container.Add(soundContainer);
            AddVolumeSliders(soundContainer);
        }

        void AddGraphicsOptions()
        {
            VisualElement graphicsContainer = new();
            graphicsContainer.AddToClassList(_ussCommonOddBackground);
            graphicsContainer.AddToClassList(_ussUiContainer);
            Label graphics = new("Graphics");
            graphics.AddToClassList(_ussTitle);

            // graphics.AddToClassList(_ussCommonTextPrimary);
            graphicsContainer.Add(graphics);
            _container.Add(graphicsContainer);

            AddFullScreenToggle(graphicsContainer);
            AddRadioResolutionGroup(graphicsContainer);
        }

        void AddVolumeSliders(VisualElement p)
        {
            Slider master = AddVolumeSlider("Master", p);
            master.AddToClassList(_ussVolumeSlider);
            master.value = PlayerPrefs.GetFloat("MasterVolume", 1);
            master.RegisterValueChangedCallback(MasterVolumeChange);

            Slider music = AddVolumeSlider("Music", p);
            music.AddToClassList(_ussVolumeSlider);
            music.value = PlayerPrefs.GetFloat("MusicVolume", 1);
            music.RegisterValueChangedCallback(MusicVolumeChange);

            Slider sfx = AddVolumeSlider("SFX", p);
            sfx.AddToClassList(_ussVolumeSlider);
            sfx.value = PlayerPrefs.GetFloat("SFXVolume", 1);
            sfx.RegisterValueChangedCallback(SfxVolumeChange);

            Slider ui = AddVolumeSlider("UI", p);
            ui.AddToClassList(_ussVolumeSlider);
            ui.value = PlayerPrefs.GetFloat("UIVolume", 1);
            ui.RegisterValueChangedCallback(UIVolumeChange);
        }

        Slider AddVolumeSlider(string n, VisualElement p)
        {
            //https://forum.unity.com/threads/changing-audio-mixer-group-volume-with-ui-slider.297884/
            VisualElement container = CreateContainer(n);
            Slider volumeSlider = new()
            {
                lowValue = 0.001f,
                highValue = 1f
            };
            volumeSlider.style.width = 200;
            volumeSlider.value = PlayerPrefs.GetFloat(n, 1);

            container.Add(volumeSlider);
            p.Add(container);

            return volumeSlider;
        }

        void MasterVolumeChange(ChangeEvent<float> evt)
        {
            PlayerPrefs.SetFloat("MasterVolume", evt.newValue);
            PlayerPrefs.Save();
            _audioManger.SetMasterVolume(evt.newValue);
        }

        void MusicVolumeChange(ChangeEvent<float> evt)
        {
            PlayerPrefs.SetFloat("MusicVolume", evt.newValue);
            PlayerPrefs.Save();
            _audioManger.SetMusicVolume(evt.newValue);
        }

        void SfxVolumeChange(ChangeEvent<float> evt)
        {
            PlayerPrefs.SetFloat("SFXVolume", evt.newValue);
            PlayerPrefs.Save();
            _audioManger.SetSfxVolume(evt.newValue);
        }

        void UIVolumeChange(ChangeEvent<float> evt)
        {
            PlayerPrefs.SetFloat("SFXVolume", evt.newValue);
            PlayerPrefs.Save();
            _audioManger.SetUIVolume(evt.newValue);
        }

        void AddFullScreenToggle(VisualElement p)
        {
            VisualElement container = CreateContainer("Full Screen");
            p.Add(container);

            _fullScreenToggle = new();
            container.Add(_fullScreenToggle);
            SetFullScreen(PlayerPrefs.GetInt("fullScreen", 1) != 0);
            _fullScreenToggle.RegisterValueChangedCallback(FullScreenToggleClick);
        }

        void FullScreenToggleClick(ChangeEvent<bool> evt)
        {
            PlayerPrefs.SetInt("fullScreen", evt.newValue ? 1 : 0);
            SetFullScreen(evt.newValue);
        }

        void SetFullScreen(bool isFullScreen)
        {
            _fullScreenToggle.value = isFullScreen;
            if (isFullScreen)
                Screen.fullScreen = true;
            else
                Screen.fullScreen = false;
        }

        void AddRadioResolutionGroup(VisualElement p)
        {
            List<string> supportedResolutions = new();
            foreach (Resolution res in Screen.resolutions)
                supportedResolutions.Add(res.ToString());

            VisualElement container = CreateContainer("Resolution");
            p.Add(container);

            DropdownField dropdown = new();
            container.Add(dropdown);

            dropdown.value = Screen.currentResolution.ToString();
            dropdown.choices.AddRange(supportedResolutions);
            dropdown.RegisterValueChangedCallback(SetResolution);
        }

        void SetResolution(ChangeEvent<string> evt)
        {
            string[] split = evt.newValue.Split(" x ");
            int width = int.Parse(split[0]);
            string[] split1 = split[1].Split(" @ ");
            int height = int.Parse(split1[0]);
            int hz = int.Parse(split1[1].Split(".")[0]);
            FullScreenMode fullScreenMode = PlayerPrefs.GetInt("fullScreen", 1) != 0
                ? FullScreenMode.ExclusiveFullScreen
                : FullScreenMode.Windowed;
            RefreshRate rr = new() { numerator = (uint)hz, denominator = 1 };
            Screen.SetResolution(width, height, fullScreenMode, rr);
        }

        VisualElement CreateContainer(string labelText)
        {
            VisualElement container = new();
            container.style.flexDirection = FlexDirection.Row;
            Label label = new(labelText);
            label.AddToClassList(USSCommonTextPrimary);
            container.Add(label);
            return container;
        }
    }
}