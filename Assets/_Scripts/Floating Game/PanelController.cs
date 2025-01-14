using System.Collections;
using DG.Tweening;
using Hoshi.Core;
using UnityEngine;

namespace Hoshi
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] _spriteRenderers;

        public void Initialize()
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
                    sr.DOColor(Helpers.AllResurrectColors[Random.Range(0, Helpers.AllResurrectColors.Length)],
                        Random.Range(2f, 3f));
                }

                yield return new WaitForSeconds(Random.Range(3f, 5f));
            }
        }
    }
}