using System;
using Hoshi.Core;
using UnityEngine;

namespace Hoshi
{
    public class PlatformerManager : Singleton<PlatformerManager>
    {
        int _coins;

        public event Action OnResetLevel;
        public event Action<int> OnCoinCountChanged;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void ResetLevel()
        {
            _coins = 0;

            OnCoinCountChanged?.Invoke(_coins);
            OnResetLevel?.Invoke();
        }

        public void AddCoin()
        {
            _coins++;
            OnCoinCountChanged?.Invoke(_coins);
        }
    }
}