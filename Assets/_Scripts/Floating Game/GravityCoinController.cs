using System.Collections;
using UnityEngine;

namespace Hoshi.Floating_Game
{
    public class GravityCoinController : CoinController
    {
        protected override void Start()
        {
            StartCoroutine(CheckYPosition());
        }

        IEnumerator CheckYPosition()
        {
            while (true)
            {
                if (this == null) yield break;
                if (transform.position.y < -10) Destroy(gameObject);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void Initialize()
        {
            GetComponent<Rigidbody2D>().gravityScale = Random.Range(0.1f, 0.5f);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out FloatingPlayerController playerController)) return;
            PlatformerManager.Instance.ChangeCoin(1);
            Destroy(gameObject);
        }
    }
}