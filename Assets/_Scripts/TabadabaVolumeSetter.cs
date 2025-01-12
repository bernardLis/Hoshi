using System.Collections;
using Hoshi.Core;
using UnityEngine;

namespace Hoshi
{
    public class TabadabaVolumeSetter : MonoBehaviour
    {
        [SerializeField] Transform _player;
        [SerializeField] float _maxX;

        AudioSource _audioSource;

        IEnumerator _volumeCoroutine;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _volumeCoroutine = VolumeSettingCoroutine();
            StartCoroutine(_volumeCoroutine);
        }

        IEnumerator VolumeSettingCoroutine()
        {
            while (true)
            {
                if (this == null) yield break;

                _audioSource.volume = _player.transform.position.x.Remap(-10f, _maxX, 0, 1);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}