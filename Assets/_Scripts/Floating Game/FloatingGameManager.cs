using System;
using Hoshi.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace Hoshi
{
    public class FloatingGameManager : Singleton<FloatingGameManager>
    {
        BoxCollider2D _boxCollider2D;

        [SerializeField] CinemachineCamera _floatingGameCamera;
        [SerializeField] FloatingPlayerController _floatingPlayerControllerPrefab;
        [SerializeField] GameObject[] _walls;


        PlayerController _playerController;

        bool _isFloatingGameStarted;

        public event Action OnFloatingGameStarted;

        void Start()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                if (playerController.transform.position.x > _boxCollider2D.bounds.max.x)
                {
                    _playerController = playerController;
                    StartFloatingGame();
                }
            }
        }

        void StartFloatingGame()
        {
            if (_isFloatingGameStarted) return;
            _isFloatingGameStarted = true;

            _floatingGameCamera.Priority = 1;
            FloatingPlayerController floatingPlayerController =
                Instantiate(_floatingPlayerControllerPrefab, _playerController.transform.position, Quaternion.identity);
            floatingPlayerController.Initialize(_playerController.GetComponent<Rigidbody2D>().linearVelocity);
            _playerController.gameObject.SetActive(false);

            foreach (GameObject wall in _walls) wall.SetActive(true);

            OnFloatingGameStarted?.Invoke();


        }
    }
}