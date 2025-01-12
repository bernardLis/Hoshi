using UnityEngine;

namespace Hoshi
{
    public class ZoneVolumeSetter : MonoBehaviour
    {
        AudioSource _audioSource;
        [SerializeField] float _volume = 0.5f;

        void Start()
        {
            _audioSource = GetComponentInParent<AudioSource>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Enter");
            if (other.CompareTag("Player"))
            {
                Debug.Log("player");

                _audioSource.volume = _volume;

            }
        }
    }
}