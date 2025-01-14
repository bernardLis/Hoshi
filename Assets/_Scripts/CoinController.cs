using DG.Tweening;
using UnityEngine;

namespace Hoshi
{
    public class CoinController : MonoBehaviour
    {
        Vector3 _startPosition;
        PlatformerManager _platformerManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _startPosition = transform.position;
            _platformerManager = PlatformerManager.Instance;
            _platformerManager.OnResetLevel += Reset;
            Reset();
        }

        void Reset()
        {
            transform.DOKill();
            transform.position = _startPosition;

            gameObject.SetActive(true);
            transform.DOMove(transform.position + transform.up * 0.2f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutBounce);
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerController playerController)) return;
            _platformerManager.ChangeCoin(1);
            gameObject.SetActive(false);
            transform.DOKill();
        }
    }
}