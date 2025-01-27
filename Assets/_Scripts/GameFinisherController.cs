using System;
using UnityEngine;

namespace Hoshi
{
    public class GameFinisherController : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            EndGameScreen endGameScreen = new EndGameScreen();
        }
    }
}