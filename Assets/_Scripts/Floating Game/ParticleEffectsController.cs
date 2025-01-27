using System.Collections.Generic;
using Hoshi.Core;
using UnityEngine;

namespace Hoshi.Floating_Game
{
    public class ParticleEffectsController : MonoBehaviour
    {
        [SerializeField] List<GameObject> _effects;

        List<ParticleSystem> _particleSystems;

        float _threshold = 0.4f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            MusicFrequencyManager.Instance.OnFrequencyBandUpdate += InstanceOnOnFrequencyBandUpdate;
            FloatingGameManager.Instance.OnFloatingGame10SecondsUntilFinished += () => _threshold = 0.7f;


            _particleSystems = new List<ParticleSystem>();
            foreach (var effect in _effects)
            {
                var ps = effect.GetComponent<ParticleSystem>();
                _particleSystems.Add(ps);
            }
        }

        void InstanceOnOnFrequencyBandUpdate(float[] arg1, float arg2, float arg3)
        {
            for (int i = 0; i < arg1.Length; i++)
            {
                if (arg1[i] > _threshold)
                    _particleSystems[i].Play();
            }
        }
    }
}