using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5.0f;

        private Vector3 _direction;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _rigidbody.velocity = _rigidbody.rotation * _direction;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (context.valueType != typeof(Vector2))
            {
                Debug.LogError($"Invalid type. Should be {typeof(Vector2)} but is {context.valueType}");
                return;
            }
#endif
            Vector2 direction = context.ReadValue<Vector2>();
            _direction = new Vector3(direction.x, 0.0f, direction.y) * _speed;
        }
    }
}
