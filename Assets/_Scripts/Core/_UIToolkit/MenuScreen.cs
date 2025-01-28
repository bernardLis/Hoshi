using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Hoshi.Core
{
    public class MenuScreen : FullScreenElement
    {
        public MenuScreen()
        {
            SetTitle("Menu");

            AddEndMarketButton();

            Content.Add(new ContinueButton(callback: Hide));
            AddLeaderboardButton();
            AddSettingsButton();
            AddQuitButton();
        }

        void AddLeaderboardButton()
        {
            MyButton leaderboardButton = new("Leaderboard", USSCommonButton, () => new LeaderboardScreen());
            Content.Add(leaderboardButton);
        }

        void AddEndMarketButton()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name != Scenes.Market)
                return;

            VisualElement spacer = new();
            spacer.AddToClassList(USSCommonHorizontalSpacer);
            Content.Add(spacer);
        }

        void AddSettingsButton()
        {
            MyButton settingsButton = new("Settings", USSCommonButton, () => new SettingsScreen());
            Content.Add(settingsButton);
        }

        void AddMainMenuButton()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == Scenes.MainMenu)
                return;

            MyButton mainMenuButton = new("Main Menu", USSCommonButton, () =>
            {
                Hide();
                OnHide += () => GameManager.LoadScene(Scenes.MainMenu);
            });
            Content.Add(mainMenuButton);
        }

        void AddQuitButton()
        {
            MyButton quitButton = new("Quit", USSCommonButton, Application.Quit);
            Content.Add(quitButton);
        }
    }
}