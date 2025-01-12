using UnityEngine;

namespace Hoshi
{
    public class DestructibleBlockController : MonoBehaviour
    {
        CapsuleCollider2D _capsuleCollider;
        BoxCollider2D _boxCollider;
        SpriteRenderer _spriteRenderer;

        [SerializeField] GameObject _particles;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            _boxCollider = GetComponentInChildren<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            PlayerController.Instance.OnDeath += Reset;
            Reset();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerController playerController)) return;

            _particles.SetActive(true);
            _capsuleCollider.enabled = false;
            _boxCollider.enabled = false;
            _spriteRenderer.enabled = false;
        }

        void Reset()
        {
            _particles.SetActive(false);
            _capsuleCollider.enabled = true;
            _spriteRenderer.enabled = true;
            _boxCollider.enabled = true;
        }
    }
}