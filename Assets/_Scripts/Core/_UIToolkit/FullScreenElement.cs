using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hoshi.Core
{
    public class FullScreenElement : VisualElement
    {
        protected const string USSCommonTextPrimary = "common__text-primary";
        protected const string USSCommonButton = "common__button";
        protected const string USSCommonHorizontalSpacer = "common__horizontal-spacer";
        const string _ussCommonFullScreenMain = "common__full-screen-main";
        const string _ussCommonFullScreenTitle = "common__full-screen-title";
        const string _ussCommonFullScreenContent = "common__full-screen-content";
        const string _ussCommonFullScreenUtilityContainer = "common__full-screen-utility-container";

        readonly Label _titleLabel;
        protected readonly VisualElement Content;
        protected readonly VisualElement UtilityContainer;

        bool _isNavigationDisabled;

        protected VisualElement Root;
        protected ContinueButton ContinueButton;

        protected GameManager GameManager;

        protected FullScreenElement()
        {
            GameManager = GameManager.Instance;
            Root = GameManager.Root;
            Root.Add(this);

            GameManager.AddOpenFullScreen(this);

            AddToClassList(_ussCommonFullScreenMain);
            AddToClassList(USSCommonTextPrimary);

            _titleLabel = new("");
            _titleLabel.AddToClassList(_ussCommonFullScreenTitle);
            Add(_titleLabel);

            // VisualElement spacer = new();
            // spacer.AddToClassList(USSCommonHorizontalSpacer);
            // Add(spacer);

            Content = new();
            Content.AddToClassList(_ussCommonFullScreenContent);
            Add(Content);

            UtilityContainer = new();
            UtilityContainer.AddToClassList(_ussCommonFullScreenUtilityContainer);
            Add(UtilityContainer);

            focusable = true;
            Focus();

            Time.timeScale = 0;

            style.opacity = 0;
            DOTween.To(x => style.opacity = x, style.opacity.value, 1, 0.5f)
                .SetUpdate(true)
                .OnComplete(EnableNavigation);
        }

        public event Action OnHide;

        protected void SetTitle(string txt)
        {
            _titleLabel.text = txt;
            DOTween.To(x => _titleLabel.style.opacity = x, 0, 1, 0.5f)
                .SetUpdate(true);
        }

        void EnableNavigation()
        {
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<KeyDownEvent>(OnKeyDown); // TODO: full screen management vs menu opening and closing
        }

        protected void DisableNavigation()
        {
            _isNavigationDisabled = true;
            UnregisterCallback<PointerDownEvent>(OnPointerDown);
            UnregisterCallback<KeyDownEvent>(OnKeyDown);
        }

        void OnPointerDown(PointerDownEvent evt)
        {
            if (_isNavigationDisabled) return;
            if (evt.button != 1) return; // only right mouse click

            Hide();
        }

        void OnKeyDown(KeyDownEvent evt)
        {
            if (_isNavigationDisabled) return;
            if (evt.keyCode != KeyCode.Escape) return;

            Hide();
        }

        protected void AddContinueButton()
        {
            ContinueButton = new(callback: Hide);
            UtilityContainer.Add(ContinueButton);
        }

        public void Hide()
        {
            VisualElement tt = Root.Q<VisualElement>("tooltipContainer");
            if (tt != null) tt.style.display = DisplayStyle.None;

            DOTween.To(x => style.opacity = x, style.opacity.value, 0, 0.5f)
                .SetUpdate(true);
            DOTween.To(x => Content.style.opacity = x, 1, 0, 0.5f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    GameManager.OpenFullScreens.Remove(this);
                    if (GameManager.OpenFullScreens.Count > 0) GameManager.OpenFullScreens[^1].Focus();

                    SetEnabled(false);
                    RemoveFromHierarchy();

                    OnHide?.Invoke();
                });
        }
    }
}