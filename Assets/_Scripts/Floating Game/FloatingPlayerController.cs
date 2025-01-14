using System;
using UnityEngine;

namespace Hoshi
{
    public class FloatingPlayerController : MonoBehaviour
    {
        PlayerInput _playerInput;
        FrameInput _frameInput;

        Rigidbody2D _rigidbody2D;

        Vector2 _targetVelocity;
        Vector2 _currentVelocity;

        [SerializeField] float _speed = 2f;


        void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            _frameInput = _playerInput.Gather();
        }

        void FixedUpdate()
        {
            _rigidbody2D.linearVelocity =
                Vector2.Lerp(_rigidbody2D.linearVelocity, _targetVelocity, Time.fixedDeltaTime);
            _targetVelocity = _frameInput.Move.normalized * _speed;
        }

        public void Initialize(Vector2 linearVelocity)
        {
            _targetVelocity = linearVelocity;
        }
    }
}