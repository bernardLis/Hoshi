using System;
using Hoshi.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hoshi
{
    public class PlatformerManager : Singleton<PlatformerManager>
    {
        int _coins;
        int _score;

        VisualElement _root;

        ChangingValueElement _scoreValueElement;
        Label _coinCountLabel;

        public event Action OnResetLevel;

        void Start()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _coinCountLabel = _root.Q<Label>("coinCountLabel");
            _coinCountLabel.text = _coins.ToString();

            _scoreValueElement = new();
            _scoreValueElement.Initialize(0, 34);
            _root.Q<VisualElement>("scoreContainer").Add(_scoreValueElement);

            ResetLevel();
        }

        public void ResetLevel()
        {
            SetCoin(0);
            SetScore(0);

            OnResetLevel?.Invoke();
        }

        public void ChangeScore(int change)
        {
            SetScore(_score + change);
        }

        void SetScore(int score)
        {
            _score = score;
            _scoreValueElement.ChangeAmount(_score);
        }

        public void ChangeCoin(int change)
        {
            SetCoin(_coins + change);
            ChangeScore(100);
            _coinCountLabel.text = _coins.ToString();
        }

        void SetCoin(int coin)
        {
            _coins = coin;
            _coinCountLabel.text = _coins.ToString();
        }
    }
}