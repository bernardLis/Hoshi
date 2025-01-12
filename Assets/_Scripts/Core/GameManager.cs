using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Hoshi.Core
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public static bool IsGamePaused;

        public readonly List<FullScreenElement> OpenFullScreens = new();
        public VisualElement Root { get; private set; }

        UnityEngine.InputSystem.PlayerInput _playerInput;
        LevelLoader _levelLoader;

        void Start()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            Helpers.SetUpHelpers(Root);

            _levelLoader = GetComponent<LevelLoader>();
        }


        /* LEVELS */
        public void LoadScene(string level)
        {
            SaveGame();

            foreach (FullScreenElement fse in OpenFullScreens)
                fse.Hide();

            Time.timeScale = 1f;
            _levelLoader.LoadLevel(level);
        }

        /* INPUT */
        void OnEnable()
        {
            _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();

            UnsubscribeInputActions();
            SubscribeInputActions();
        }

        void OnDisable()
        {
            if (_playerInput == null) return;
            UnsubscribeInputActions();
        }

        void OnDestroy()
        {
            if (_playerInput == null) return;
            UnsubscribeInputActions();
        }

        void SubscribeInputActions()
        {
            _playerInput.onControlsChanged += OnControlsChanged;

            _playerInput.actions["OpenMenu"].performed += OpenMenuClicked;
        }

        void UnsubscribeInputActions()
        {
            _playerInput.onControlsChanged -= OnControlsChanged;

            _playerInput.actions["OpenMenu"].performed -= OpenMenuClicked;
        }

        void OpenMenuClicked(InputAction.CallbackContext obj)
        {
            if (OpenFullScreens.Count > 0)
                OpenFullScreens[^1].Hide();
            else
                new MenuScreen();
        }

        void PauseGame()
        {
            IsGamePaused = true;
            Time.timeScale = 0;
        }

        void ResumeGame()
        {
            IsGamePaused = false;
            Time.timeScale = 1;
        }

        public void AddOpenFullScreen(FullScreenElement fse)
        {
            OpenFullScreens.Add(fse);
            if (OpenFullScreens.Count == 1) PauseGame();
            fse.OnHide += OnFullScreenHide;
        }

        void OnFullScreenHide()
        {
            if (OpenFullScreens.Count == 0) ResumeGame();
        }

        void OnControlsChanged(UnityEngine.InputSystem.PlayerInput obj)
        {
            Debug.Log($"Controls changed {obj.currentControlScheme}");
        }

        /* SAVE & LOAD */
        public void SaveGame()
        {
        }
    }

}