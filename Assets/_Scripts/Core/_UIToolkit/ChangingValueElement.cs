using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hoshi.Core
{
    public class ChangingValueElement : VisualElement
    {
        IVisualElementScheduledItem _amountChangeScheduler;
        int _currentlyDisplayedAmount;

        Label _text;
        public int Amount;

        public event Action OnAnimationFinished;

        public void Initialize(int amount, int fontSize)
        {
            Amount = amount;
            _currentlyDisplayedAmount = amount;
            _text = new(amount.ToString());
            _text.style.fontSize = fontSize;
            _text.style.marginBottom = 0;
            Add(_text);
        }

        public void ChangeAmount(int newValue)
        {
            if (newValue == Amount)
                return;

            _currentlyDisplayedAmount = Amount;
            Amount = newValue;

            if (_amountChangeScheduler != null)
                _amountChangeScheduler.Pause();

            _amountChangeScheduler = schedule.Execute(NumberAnimation).Every(10);
        }

        void NumberAnimation()
        {
            if (_currentlyDisplayedAmount == Amount)
                FinishAnimation();

            int currentDiff = Mathf.Abs(_currentlyDisplayedAmount - Amount);
            int multiplier = 1 + Mathf.FloorToInt(currentDiff / 100);

            if (_currentlyDisplayedAmount < Amount)
                _currentlyDisplayedAmount += 1 * multiplier;
            if (_currentlyDisplayedAmount > Amount)
                _currentlyDisplayedAmount -= 1 * multiplier;

            _text.text = _currentlyDisplayedAmount.ToString();
        }

        void FinishAnimation()
        {
            if (_amountChangeScheduler != null)
                _amountChangeScheduler.Pause();

            OnAnimationFinished?.Invoke();
        }
    }
}