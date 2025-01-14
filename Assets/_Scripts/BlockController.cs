using System;
using UnityEngine;

namespace Hoshi
{
    public class BlockController : MonoBehaviour
    {
        protected SpriteRenderer SpriteRenderer;
        protected BoxCollider2D BoxCollider2D;

        protected PlatformerManager PlatformerManager;

        protected virtual void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            BoxCollider2D = GetComponent<BoxCollider2D>();

            PlatformerManager = PlatformerManager.Instance;
            PlatformerManager.OnResetLevel += Reset;
        }

        protected virtual void Reset()
        {
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                CollisionWithPlayer();
        }

        protected virtual void CollisionWithPlayer()
        {
        }
    }
}