using TarodevController;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinBlockController : MonoBehaviour
{
    CapsuleCollider2D _capsuleCollider;
    SpriteRenderer _spriteRenderer;

    [SerializeField] Sprite _activeBlockSprite;
    [SerializeField] Sprite _spentBlockSprite;

    [SerializeField] int _coins;
    [SerializeField] GameObject _particles;

    int _currentCoins;
    ParticleSystem _particleSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _particleSystem = _particles.GetComponent<ParticleSystem>();

        PlayerController.Instance.OnDeath += Reset;

        Reset();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerController playerController)) return;

        _particleSystem.Play();
        playerController.AddCoin();
        _currentCoins--;
        if (_currentCoins == 0)
            EndCoins();
    }

    void EndCoins()
    {
        _capsuleCollider.enabled = false;
        _spriteRenderer.sprite = _spentBlockSprite;
    }

    void Reset()
    {
        _capsuleCollider.enabled = true;
        _currentCoins = _coins;
        _spriteRenderer.sprite = _activeBlockSprite;
    }
}