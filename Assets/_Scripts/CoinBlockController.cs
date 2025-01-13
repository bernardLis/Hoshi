using UnityEngine;

namespace Hoshi
{
    public class BlockControllerCoin : BlockController
    {
        CapsuleCollider2D _capsuleCollider;

        [SerializeField] Sprite _activeBlockSprite;
        [SerializeField] Sprite _spentBlockSprite;

        [SerializeField] int _coins;
        [SerializeField] GameObject _particles;

        int _currentCoins;
        ParticleSystem _particleSystem;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();

            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            _particleSystem = _particles.GetComponent<ParticleSystem>();

            Reset();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerController playerController)) return;

            _particleSystem.Play();
            PlatformerManager.AddCoin();
            _currentCoins--;
            if (_currentCoins == 0)
                EndCoins();
        }

        void EndCoins()
        {
            _capsuleCollider.enabled = false;
            SpriteRenderer.sprite = _spentBlockSprite;
        }

        protected override void Reset()
        {
            base.Reset();
            _capsuleCollider.enabled = true;
            _currentCoins = _coins;
            SpriteRenderer.sprite = _activeBlockSprite;
        }
    }
}