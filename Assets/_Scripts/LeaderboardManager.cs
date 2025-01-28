using Dan.Main;
using Hoshi.Core;
using UnityEngine;

namespace Hoshi
{
    public class LeaderboardManager : Singleton<LeaderboardManager>
    {
        [SerializeField] LeaderboardEntryController _leaderboardEntryPrefab;
        [SerializeField] Transform _leaderboardEntriesParent;

        void Start()
        {
            Leaderboards.HoshiLeaderboard.GetEntries(entries =>
            {
                for (int i = 0; i < 3; i++)
                {
                    LeaderboardEntryController entry = Instantiate(_leaderboardEntryPrefab, _leaderboardEntriesParent);
                    entry.SetEntry(entries[i].Username, entries[i].Score);
                }
            });
        }
    }
}