using DG.Tweening;
using UnityEngine;

namespace Hoshi
{
    public class CoinController : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            PlayerController.Instance.OnDeath += Reset;
            Reset();
        }

        void Reset()
        {
            gameObject.SetActive(true);
            transform.DOKill();
            transform.DOMove(transform.position + Vector3.up * 0.2f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutBounce);
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerController playerController)) return;
            playerController.AddCoin();
            gameObject.SetActive(false);
            transform.DOKill();
        }
    }
}