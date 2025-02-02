using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hoshi.Floating_Game
{
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] GameObject _coinPrefab;

        void Start()
        {
            FloatingGameManager.Instance.OnFloatingGameStarted += Run;
            FloatingGameManager.Instance.OnFloatingGame10SecondsUntilFinished += Stop;
        }

        void Stop()
        {
            StopAllCoroutines();
        }

        void Run()
        {
            StartCoroutine(CoinSpawnerCoroutine());
        }

        IEnumerator CoinSpawnerCoroutine()
        {
            yield return new WaitForSeconds(2f);

            while (true)
            {
                if (this == null) yield break;

                int count = Random.Range(1, 4);
                for (int i = 0; i < count; i++)
                {
                    Vector3 pos = new(Random.Range(207f, 233f), 15, 0);
                    GravityCoinController c = Instantiate(_coinPrefab, pos, Quaternion.identity)
                        .GetComponent<GravityCoinController>();
                    c.Initialize();
                }

                yield return new WaitForSeconds(Random.Range(1f, 4f));
            }
        }
    }
}