using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Hoshi.Floating_Game
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] _spriteRenderers;

        List<Color> _colors = new();

        public void Initialize(ColorList colorList)
        {
            _colors = new(colorList.Colors);
        }

        public void Run()
        {
            StartCoroutine(ChangeColorCoroutine());
        }

        IEnumerator ChangeColorCoroutine()
        {
            while (true)
            {
                if (this == null) yield break;

                foreach (SpriteRenderer sr in _spriteRenderers)
                {
                    sr.DOColor(_colors[Random.Range(0, _colors.Count)],
                        Random.Range(2f, 3f));
                }

                yield return new WaitForSeconds(Random.Range(3f, 5f));
            }
        }
    }
}