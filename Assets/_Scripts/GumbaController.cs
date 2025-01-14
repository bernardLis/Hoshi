using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Hoshi
{
    public class GumbaController : MonoBehaviour
    {
        static readonly int AnimDie = Animator.StringToHash("Die");

        PlatformerManager _platformerManager;

        Animator _animator;
        Rigidbody2D _rigidbody;
        BoxCollider2D _boxCollider;
        SpriteRenderer _spriteRenderer;

        Vector3 _startPosition;

        CancellationTokenSource _walkCancellation;
        CancellationTokenSource _checkYPositionCancellation;
        CancellationTokenSource _bounceFromWallsCancellation;
        CancellationTokenSource _checkPlayerJumpCancellation;
        CancellationTokenSource _killCheckCancellation;

        [SerializeField] float _speed = 10f;
        bool _isDead;

        void Start()
        {
            _platformerManager = PlatformerManager.Instance;
            _platformerManager.OnResetLevel += Reset;

            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

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

            if (isKilledByPlayer) _platformerManager.ChangeScore(100);

            _walkCancellation?.Cancel();
            _walkCancellation = null;

            _checkYPositionCancellation?.Cancel();
            _checkYPositionCancellation = null;

            _bounceFromWallsCancellation?.Cancel();
            _bounceFromWallsCancellation = null;

            _animator.SetTrigger(AnimDie);
            _spriteRenderer.DOColor(new(1, 1, 1, 0), 4f).OnComplete(
                () => _boxCollider.enabled = false);
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
                if (e is not OperationCanceledException)
                    Debug.LogException(e);
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
                    0.54f, LayerMask.GetMask("Default"));
                Debug.DrawRay(transform.position, Vector2.down * 0.54f, Color.red, 0.1f);

                // check if is grounded, otherwise fall
                Vector3 move = transform.right * _speed;
                if (!isGrounded) move.y = 10f;

                _rigidbody.MovePosition(transform.position - move * Time.fixedDeltaTime);

                await Awaitable.FixedUpdateAsync();
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
                if (e is not TaskCanceledException)
                    Debug.LogException(e);
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

                await Awaitable.WaitForSecondsAsync(1f);
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
                if (e is not TaskCanceledException)
                    Debug.LogException(e);
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

                await Awaitable.WaitForSecondsAsync(0.1f);
            }
        }

        void KillPlayer()
        {
            if (_isDead) return;
            _platformerManager.ResetLevel();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController playerController))
                HandlePlayerCollision(playerController);
        }

        void HandlePlayerCollision(PlayerController playerController)
        {
            if (playerController.transform.position.y > transform.position.y)
                Die(true);
            else
                KillPlayer();
        }
    }
}