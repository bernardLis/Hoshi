using UnityEngine;

namespace Hoshi
{
    public class DestructibleBlockController : BlockController
    {
        [SerializeField] GameObject _particles;

        protected override void Start()
        {
            base.Start();
            Reset();
        }

        protected override void CollisionWithPlayer()
        {
            _particles.SetActive(true);
            BoxCollider2D.enabled = false;
            SpriteRenderer.enabled = false;
        }

        protected override void Reset()
        {
            _particles.SetActive(false);
            SpriteRenderer.enabled = true;
            BoxCollider2D.enabled = true;
        }
    }
}