using System.Collections.Generic;
using UnityEngine;

namespace Hoshi
{
    public class StrokeEffectsController : MonoBehaviour
    {
        [SerializeField] List<GameObject> _effects;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            foreach (GameObject e in _effects)
            {
                e.SetActive(false);
            }

            FloatingGameManager.Instance.OnFloatingGameStarted += () =>
            {
                foreach (GameObject e in _effects)
                {
                    e.SetActive(true);
                }
            };
        }
    }
}