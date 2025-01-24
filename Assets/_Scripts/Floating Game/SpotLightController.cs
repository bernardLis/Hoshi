using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Hoshi
{
    public class SpotLightController : MonoBehaviour
    {
        [SerializeField] List<Light2D> _lights;

        readonly List<Vector2> _endPositions = new()
            { new(9, 5), new(-9, 5), new(-9, -5), new(9, -5) };

        void Start()
        {
            for (int i = 0; i < _lights.Count; i++)
            {
                _lights[i].transform.localPosition = _endPositions[3 - i];
                _lights[i].gameObject.SetActive(false);

            }

            FloatingGameManager.Instance.OnFloatingGameStarted += StartLightsMovement;

        }

        void StartLightsMovement()
        {

            for (int i = 0; i < _lights.Count; i++)
            {
                _lights[i].transform.DOLocalMoveX(_endPositions[i].x, Random.Range(3f, 5f))
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);

                _lights[i].transform.DOLocalMoveY(_endPositions[i].y, Random.Range(3f, 5f))
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}