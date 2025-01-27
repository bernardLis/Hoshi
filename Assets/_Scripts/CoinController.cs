using DG.Tweening;
using Hoshi.Core;
using UnityEngine;

namespace Hoshi
{
    public class CoinController : MonoBehaviour
    {
        [SerializeField] Sound _collectSound;

        AudioManager _audioManager;
        PlatformerManager _platformerManager;

        Vector3 _startPosition;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            _startPosition = transform.position;

            _audioManager = AudioManager.Instance;
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
            _audioManager.CreateSound().WithSound(_collectSound).WithPosition(transform.position).Play();
            _platformerManager.ChangeCoin(1);
            gameObject.SetActive(false);
            transform.DOKill();
        }
    }
}