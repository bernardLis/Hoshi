using UnityEngine;

namespace Hoshi
{
    public class BlockControllerCoin : BlockController
    {
        [SerializeField] Sprite _activeBlockSprite;
        [SerializeField] Sprite _spentBlockSprite;

        [SerializeField] int _coins;
        [SerializeField] GameObject _particles;

        int _currentCoins;
        ParticleSystem _particleSystem;

        protected override void Start()
        {
            base.Start();

            _particleSystem = _particles.GetComponent<ParticleSystem>();

            Reset();
        }

        protected override void CollisionWithPlayer()
        {
            base.CollisionWithPlayer();
            if (_currentCoins == 0) return;

            _particleSystem.Play();
            PlatformerManager.ChangeCoin(1);
            _currentCoins--;
            if (_currentCoins == 0)
                EndCoins();
        }

        void EndCoins()
        {
            SpriteRenderer.sprite = _spentBlockSprite;
        }

        protected override void Reset()
        {
            base.Reset();
            _currentCoins = _coins;
            SpriteRenderer.sprite = _activeBlockSprite;
        }
    }
}