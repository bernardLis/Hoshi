using UnityEngine;
using UnityEngine.UIElements;

namespace BountyBalance.Core
{
    public class FpsDisplayer : MonoBehaviour
    {
        bool _isFpsCounterEnabled;
        Label _fpsLabel;
        float _deltaTime;

        void Start()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _fpsLabel = new();
            _fpsLabel.style.fontSize = 14;
            _fpsLabel.style.color = Color.green;
            _fpsLabel.style.position = Position.Absolute;
            _fpsLabel.style.left = 0;
            _fpsLabel.style.top = 0;
            root.Add(_fpsLabel);

            UpdateVisibility();
        }

        public void UpdateVisibility()
        {
            _isFpsCounterEnabled = PlayerPrefs.GetInt("fpsCounter", 0) != 0;
            _fpsLabel.visible = _isFpsCounterEnabled;
        }

        void Update()
        {
            if (!_isFpsCounterEnabled) return;
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            _fpsLabel.text = $"{Mathf.Ceil(fps)}";
        }
    }
}