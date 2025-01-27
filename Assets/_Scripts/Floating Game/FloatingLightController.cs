using System.Collections;
using DG.Tweening;
using Hoshi.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Hoshi.Floating_Game
{
    public class FloatingLightController : MonoBehaviour
    {
        Light2D _light;

        public void Initialize()
        {
            _light = GetComponent<Light2D>();
            MusicFrequencyManager.Instance.OnFrequencyBandUpdate += InstanceOnOnFrequencyBandUpdate;
        }

        void InstanceOnOnFrequencyBandUpdate(float[] arg1, float arg2, float arg3)
        {
            _light.intensity = arg3 * 2;
        }

        public void Run()
        {
            StartCoroutine(MoveCoroutine());
        }

        IEnumerator MoveCoroutine()
        {
            while (true)
            {
                if (this == null) yield break;
                yield return transform.DOMoveY(Random.Range(18f, 20f), Random.Range(4f, 8f))
                    .SetEase(Ease.InOutSine)
                    .WaitForCompletion();
                yield return new WaitForSeconds(Random.Range(1f, 2f));
                Vector3 newPos = new Vector3(transform.position.x, -11, transform.position.z);
                transform.position = newPos;
                // yield return transform.DOMoveY(Random.Range(-6f, -8f), Random.Range(4f, 8f))
                //     .SetEase(Ease.InOutSine)
                //     .WaitForCompletion();
            }
        }


        public void Stop(Vector3 pos)
        {
            transform.DOKill();
            StopAllCoroutines();
            transform.DOMove(pos, 1f);
        }
    }
}