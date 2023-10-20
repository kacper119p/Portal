using System;
using UnityEngine;
using UnityEngine.Events;

namespace Portal
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Teleportable, Portal> _onPortalEnter;

        public UnityEvent<Teleportable, Portal> OnPortalEnter => _onPortalEnter;

        public UnityEvent<Teleportable, Portal> OnPortalExit => _onPortalExit;

        [SerializeField] private UnityEvent<Teleportable, Portal> _onPortalExit;


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Teleportable teleportable))
            {
                _onPortalEnter.Invoke(teleportable, this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Teleportable teleportable))
            {
                _onPortalExit.Invoke(teleportable, this);
            }
        }

        public float CalculatePortalSide(Vector3 position)
        {
            Transform portalTransform = transform;
            return Mathf.Sign(Vector3.Dot
                (
                    position - portalTransform.position,
                    portalTransform.forward
                )
            );
        }
    }
}
