using System;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Controls
{
    [RequireComponent(typeof(Rigidbody))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _sensitivity = 1.0f;
        [SerializeField] private float _maxVerticalAngle = 60.0f;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (context.valueType != typeof(Vector2))
            {
                Debug.LogError($"Invalid type. Should be {typeof(Vector2)} but is {context.valueType}");
                return;
            }
#endif
            Vector2 direction = context.ReadValue<Vector2>();

            _rigidbody.Move
            (
                _rigidbody.position,
                Quaternion.Euler(0.0f, direction.x * _sensitivity, 0.0f) * _rigidbody.rotation
            );

            Quaternion oldRotation = _camera.transform.localRotation;
            Quaternion newRotation = oldRotation * Quaternion.Euler(direction.y * _sensitivity, 0.0f, 0.0f);
            Vector3 eulerAngles = newRotation.eulerAngles;

            float x = MathHelpers.ClampAngle(eulerAngles.x, -_maxVerticalAngle, _maxVerticalAngle);

            newRotation = Quaternion.Euler
            (
                x,
                0.0f,
                0.0f
            );

            _camera.transform.localRotation = newRotation;
        }
    }
}
