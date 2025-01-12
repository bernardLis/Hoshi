using UnityEngine;

namespace Hoshi
{
    public class GumbaController : MonoBehaviour
    {
        Rigidbody2D _rigidbody;
        Awaitable _walkAwaitable;

        [SerializeField] float _speed = 10f;

        async void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _walkAwaitable = WalkAwaitable();
            await _walkAwaitable;
        }

        async Awaitable WalkAwaitable()
        {
            while (true)
            {
                if (this == null) return;

                bool isGrounded = Physics2D.Raycast(transform.position + Vector3.right * 0.5f, Vector2.down,
                    Vector2.down.magnitude,
                    LayerMask.GetMask("Default"));
                Debug.DrawRay(transform.position + Vector3.right * 0.5f, Vector2.down, Color.red, 0.1f);
                // check if is grounded, otherwise fall
                Vector3 move = Vector3.right * _speed;
                if (!isGrounded)
                {
                    move.y = 10f;
                    Debug.Log("fall");
                }

                _rigidbody.MovePosition(transform.position - move * Time.fixedDeltaTime);

                await Awaitable.FixedUpdateAsync();
            }
        }
    }
}