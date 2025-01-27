using System.Collections.Generic;
using UnityEngine;

namespace Hoshi.Floating_Game
{
    public class SpotLightController : MonoBehaviour
    {
        [SerializeField] List<Color> _colors;

        [SerializeField] FloatingLightController _lightPrefab;

        List<FloatingLightController> _lights = new();

        [SerializeField] int _lightCount = 3;

        void Start()
        {
            _lights = new();
            for (int i = -1; i < _lightCount - 1; i++)
            {
                FloatingLightController instance =
                    Instantiate(_lightPrefab, transform).GetComponent<FloatingLightController>();
                instance.Initialize();
                instance.transform.localPosition = new(i * 10, 0, 0);
                _lights.Add(instance);
            }

            FloatingGameManager.Instance.OnFloatingGameStarted += StartLightsMovement;
            FloatingGameManager.Instance.OnFloatingGameFinished += Stop;
        }

        void StartLightsMovement()
        {
            for (int i = 0; i < _lights.Count; i++)
                _lights[i].Run();
        }

        void Stop(Vector3 pos)
        {
            foreach (FloatingLightController t in _lights)
                t.Stop(pos);
        }
    }
}