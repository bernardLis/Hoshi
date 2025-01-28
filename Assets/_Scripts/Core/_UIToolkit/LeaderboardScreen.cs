using Dan.Main;
using Hoshi.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hoshi
{
    public class LeaderboardScreen : FullScreenElement
    {
        public LeaderboardScreen()
        {
            SetTitle("Leaderboard");

            ScrollView scrollView = new();
            Content.Add(scrollView);

            Leaderboards.HoshiLeaderboard.GetEntries(entries =>
            {
                for (int i = 0; i < 100; i++)
                {
                    if (i > entries.Length - 1) break;

                    VisualElement container = new();
                    container.style.flexDirection = FlexDirection.Row;
                    container.style.alignItems = Align.Center;
                    container.style.justifyContent = Justify.SpaceBetween;

                    Label nameLabel = new();
                    nameLabel.text = entries[i].Username;
                    Label scoreLabel = new();
                    scoreLabel.text = entries[i].Score.ToString();

                    container.Add(nameLabel);
                    container.Add(scoreLabel);

                    scrollView.Add(container);
                }
            });

            AddContinueButton();
        }
    }
}