using Dan.Main;
using UnityEngine;
using Hoshi.Core;
using UnityEngine.UIElements;

namespace Hoshi
{
    public class EndGameScreen : FullScreenElement
    {
        MyButton _submitButton;

        const string _ussCommonTextField = "common__text-field";

        public EndGameScreen() : base()
        {
            SetTitle("Wow! Good game!");

            AddScore();
            AddLeaderboardEntryField();

            AddLeaderBoardButton();
            AddResetButton();
            AddQuitButton();
        }


        void AddScore()
        {
            Label title = new Label("Add your score to the leaderboard:");
            Content.Add(title);

            VisualElement container = new();
            container.style.flexDirection = FlexDirection.Row;

            Label score = new Label($"Score: ");
            ChangingValueElement scoreElement = new();
            scoreElement.Initialize(PlatformerManager.Instance.GetScore(), 34);

            container.Add(score);
            container.Add(scoreElement);
            Content.Add(container);
        }

        void AddLeaderboardEntryField()
        {
            TextField textField = new();
            textField.AddToClassList(_ussCommonTextField);

            textField.label = "Name";
            textField.value = "opos";
            textField.maxLength = 4;

            Label n = textField.Q<Label>();


            Content.Add(textField);

            _submitButton = new("Submit", USSCommonButton, () =>
            {
                _submitButton.SetEnabled(false);
                Leaderboards.HoshiLeaderboard.UploadNewEntry(textField.text, PlatformerManager.Instance.GetScore(),
                    isSuccessful =>
                    {
                        if (isSuccessful) _submitButton.SetText("Submitted");
                        else _submitButton.SetText("Failed to submit");
                    });
            });
            Content.Add(_submitButton);
        }

        void AddLeaderBoardButton()
        {
            MyButton leaderboardButton = new("Leaderboard", USSCommonButton, () => new LeaderboardScreen());
            UtilityContainer.Add(leaderboardButton);
        }

        void AddResetButton()
        {
            MyButton resetButton = new("Play Again!", USSCommonButton, () =>
            {
                Hide();
                OnHide += () => GameManager.LoadScene(Scenes.Game);
            });
            UtilityContainer.Add(resetButton);
        }


        void AddQuitButton()
        {
            MyButton quitButton = new("Quit", USSCommonButton, Application.Quit);
            UtilityContainer.Add(quitButton);
        }
    }
}