using TMPro;
using UnityEngine;

namespace Hoshi
{
    public class LeaderboardEntryController : MonoBehaviour
    {
        [SerializeField] TMP_Text _nameText;
        [SerializeField] TMP_Text _scoreText;

        public void SetEntry(string n, int score)
        {
            _nameText.text = n;
            _scoreText.text = score.ToString();
        }
    }
}