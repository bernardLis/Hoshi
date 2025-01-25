using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Hoshi
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] Color[] _colors;
        int _currentColorIndex = 0;

        SpriteRenderer _spriteRenderer;

        IEnumerator _changeColorCoroutine;

        void Start()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            FloatingGameManager.Instance.OnFloatingGameStarted += ChangeColor;
            FloatingGameManager.Instance.OnFloatingGame10SecondsUntilFinished += Stop;
        }

        void ChangeColor()
        {
            _changeColorCoroutine = ChangeColorCoroutine();
            StartCoroutine(_changeColorCoroutine);
        }

        void Stop()
        {
            StopCoroutine(_changeColorCoroutine);
            _spriteRenderer.DOKill();
            _spriteRenderer.DOColor(new(0.4980392f, 0.4392156f, 0.5411764f, 0f), 10f);
        }

        IEnumerator ChangeColorCoroutine()
        {
            while (true)
            {
                if (this == null) yield break;
                _spriteRenderer.DOColor(_colors[_currentColorIndex], Random.Range(20f, 30f));
                _currentColorIndex = (_currentColorIndex + 1) % _colors.Length;
                yield return new WaitForSeconds(Random.Range(4f, 5f));
            }
        }
    }
}