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

            Content.Add(new ContinueButton(callback: Hide));
            AddLeaderboardButton();
            AddSettingsButton();
            AddResetButton();
            AddQuitButton();
        }


        void AddLeaderboardButton()
        {
            MyButton leaderboardButton = new("Leaderboard", USSCommonButton, () => new LeaderboardScreen());
            Content.Add(leaderboardButton);
        }

        void AddSettingsButton()
        {
            MyButton settingsButton = new("Settings", USSCommonButton, () => new SettingsScreen());
            Content.Add(settingsButton);
        }

        void AddResetButton()
        {
            MyButton resetButton = new("Reset", USSCommonButton, () =>
            {
                Hide();
                OnHide += () => GameManager.LoadScene(Scenes.Game);
            });
            Content.Add(resetButton);
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