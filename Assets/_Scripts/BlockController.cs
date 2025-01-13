using UnityEngine;

namespace Hoshi
{
    public class BlockController : MonoBehaviour
    {
        protected SpriteRenderer SpriteRenderer;

        protected PlatformerManager PlatformerManager;

        protected virtual void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();

            PlatformerManager = PlatformerManager.Instance;
            PlatformerManager.OnResetLevel += Reset;

        }

        protected virtual void Reset()
        {
        }
    }
}