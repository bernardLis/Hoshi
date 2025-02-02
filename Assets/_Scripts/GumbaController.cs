using System;
using System.Threading;
using DG.Tweening;
using Hoshi.Core;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Hoshi
{
    public class GumbaController : MonoBehaviour
    {
        static readonly int AnimDie = Animator.StringToHash("Die");

        [SerializeField] Sound _deathSound;

        AudioManager _audioManager;
        PlatformerManager _platformerManager;

        Animator _animator;
        Rigidbody2D _rigidbody;
        BoxCollider2D _boxCollider;
        SpriteRenderer _spriteRenderer;
        MMF_Player _feelPlayer;

        Vector3 _startPosition;

        CancellationTokenSource _walkCancellation;
        CancellationTokenSource _checkYPositionCancellation;
        CancellationTokenSource _bounceFromWallsCancellation;

        [SerializeField] float _speed = 10f;
        bool _isDead;

        void Start()
        {
            _audioManager = AudioManager.Instance;

            _platformerManager = PlatformerManager.Instance;
            _platformerManager.OnResetLevel += Reset;

            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _feelPlayer = GetComponent<MMF_Player>();

            _startPosition = transform.position;

            Reset();
        }

        void Reset()
        {
            _spriteRenderer.DOKill();

            _animator.Rebind();
            _animator.Update(0f);

            _isDead = false;
            transform.position = _startPosition;
            _boxCollider.enabled = true;
            _spriteRenderer.color = new(1, 1, 1, 1);

            StartWalking();
            CheckYPosition();
            BounceFromWalls();
        }

        void Die(bool isKilledByPlayer = false)
        {
            if (_isDead) return;
            _isDead = true;

            if (isKilledByPlayer)
            {
                Vector3 position = transform.position;
                position.y -= 0.09f;
                transform.position = position;
                _audioManager.CreateSound().WithSound(_deathSound).WithPosition(transform.position).Play();
                _platformerManager.ChangeScore(100);
                DisplayFloatingText("100", Color.white);
            }

            _boxCollider.enabled = false;

            _walkCancellation?.Cancel();
            _walkCancellation = null;

            _checkYPositionCancellation?.Cancel();
            _checkYPositionCancellation = null;

            _bounceFromWallsCancellation?.Cancel();
            _bounceFromWallsCancellation = null;

            _animator.SetTrigger(AnimDie);
            _spriteRenderer.DOColor(new(1, 1, 1, 0), 4f);
        }

        async void StartWalking()
        {
            try
            {
                _walkCancellation?.Cancel();
                _walkCancellation = new();
                await WalkAwaitable(_walkCancellation.Token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                }
                else
                    throw; // TODO handle exception
            }
        }

        async Awaitable WalkAwaitable(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (this == null) return;
                if (cancellationToken.IsCancellationRequested) break;

                bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down,
                    0.42f, LayerMask.GetMask("Default"));

                // check if is grounded, otherwise fall
                Vector3 move = transform.right * _speed;
                if (transform.position.y > -4.65f) move.y = 1f;

                if (!isGrounded) move.y = 10f;

                _rigidbody.MovePosition(transform.position - move * Time.fixedDeltaTime);

                await Awaitable.FixedUpdateAsync(cancellationToken);
            }
        }

        async void CheckYPosition()
        {
            try
            {
                _checkYPositionCancellation?.Cancel();
                _checkYPositionCancellation = new();
                await YPositionCheckAwaitable(_checkYPositionCancellation.Token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                }
                else
                    throw; // TODO handle exception
            }
        }

        async Awaitable YPositionCheckAwaitable(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (this == null) return;
                if (cancellationToken.IsCancellationRequested) break;
                if (transform.position.y < -10f) Die();

                await Awaitable.WaitForSecondsAsync(1f, cancellationToken);
            }
        }


        async void BounceFromWalls()
        {
            try
            {
                _bounceFromWallsCancellation?.Cancel();
                _bounceFromWallsCancellation = new();
                await WallBounceAwaitable(_bounceFromWallsCancellation.Token);
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                }
                else
                    throw; // TODO handle exception
            }
        }

        async Awaitable WallBounceAwaitable(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (this == null) return;
                if (cancellationToken.IsCancellationRequested) break;

                bool hitWall = Physics2D.Raycast(transform.position, -transform.right,
                    0.5f, LayerMask.GetMask("Default"));
                Debug.DrawRay(transform.position, -transform.right * 0.5f, Color.blue, 0.1f);

                if (hitWall) transform.Rotate(Vector3.up * 180f);

                await Awaitable.WaitForSecondsAsync(0.1f, cancellationToken);
            }
        }

        void KillPlayer(PlayerController playerController)
        {
            if (_isDead) return;
            playerController.PlayerDeath();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController playerController))
                HandlePlayerCollision(playerController);
        }

        void HandlePlayerCollision(PlayerController playerController)
        {
            if (playerController.transform.position.y > transform.position.y)
            {
                playerController.JumpKill();
                Die(true);
            }
            else
                KillPlayer(playerController);
        }

        void DisplayFloatingText(string text, Color color)
        {
            if (_feelPlayer == null) return;
            MMF_FloatingText floatingText = _feelPlayer.GetFeedbackOfType<MMF_FloatingText>();
            floatingText.Value = text;
            floatingText.ForceColor = true;
            floatingText.AnimateColorGradient = Helpers.GetFakeGradient(color);
            Transform t = transform;
            _feelPlayer.PlayFeedbacks(t.position);
        }
    }
}