using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Hoshi.Core
{
    public class ConsoleManager : MonoBehaviour
    {
        GameManager _gameManager;
        UnityEngine.InputSystem.PlayerInput _playerInput;

        static string _myLog = "";

        bool _isOpen;

        VisualElement _consoleContainer;
        ScrollView _logContainer;

        void Start()
        {
            _gameManager = GetComponent<GameManager>();
            _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _consoleContainer = root.Q<VisualElement>("consoleContainer");
            _logContainer = root.Q<ScrollView>("logContainer");
        }

        /* INPUT */
        void OnEnable()
        {
            if (_gameManager == null)
                _gameManager = GameManager.Instance;
            _playerInput = _gameManager.GetComponent<UnityEngine.InputSystem.PlayerInput>();
            UnsubscribeInputActions();
            SubscribeInputActions();

            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            if (_playerInput == null)
                return;
            UnsubscribeInputActions();

            Application.logMessageReceived -= Log;
        }

        void SubscribeInputActions()
        {
            _playerInput.actions["ToggleConsole"].performed += ToggleCommandLine;
        }

        void UnsubscribeInputActions()
        {
            _playerInput.actions["ToggleConsole"].performed -= ToggleCommandLine;
        }

        void ToggleCommandLine(InputAction.CallbackContext ctx)
        {
            _isOpen = !_isOpen;

            _consoleContainer.style.display = _isOpen ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /* LOG */
        void Log(string logString, string stackTrace, LogType type)
        {
            if (_logContainer == null) return;

            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
            string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            Label n = new(timeText + ": " + logString);
            _logContainer.Add(n);

            _myLog = logString + "\n" + _myLog;
            if (_myLog.Length > 5000)
                _myLog = _myLog.Substring(0, 4000);
        }
    }
}