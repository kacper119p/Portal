using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Portal
{
    public class PortalManager : MonoBehaviour
    {
        [SerializeField] private Portal _portal1;
        [SerializeField] private Portal _portal2;

        private List<TeleportableData> _candidates;
        private const int DefaultCandidatesCapacity = 32;

        private UnityAction<Teleportable, Portal> _enterListener;
        private UnityAction<Teleportable, Portal> _exitListener;

        private const float ExitPushAmount = 0.05f;

        private void Awake()
        {
            _candidates = new(DefaultCandidatesCapacity);
            _enterListener = OnPortalEnter;
            _exitListener = OnPortalExit;
        }

        private void OnEnable()
        {
            _portal1.OnPortalEnter.AddListener(_enterListener);
            _portal1.OnPortalExit.AddListener(_exitListener);
            _portal2.OnPortalEnter.AddListener(_enterListener);
            _portal2.OnPortalExit.AddListener(_exitListener);
        }

        private void OnDisable()
        {
            _portal1.OnPortalEnter.RemoveListener(_enterListener);
            _portal1.OnPortalExit.RemoveListener(_exitListener);
            _portal2.OnPortalEnter.RemoveListener(_enterListener);
            _portal2.OnPortalExit.RemoveListener(_exitListener);
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _candidates.Count; i++)
            {
                if (_candidates[i].UpdatePortalSide())
                {
                    TeleportableData data = _candidates[i];
                    Teleport(ref data);
                    _candidates[i] = data;
                }
            }
        }

        private void OnPortalEnter(Teleportable teleportable, Portal portal)
        {
            TeleportableData data = new(teleportable, portal);
            if (!_candidates.Contains(data))
            {
                _candidates.Add(data);
            }
        }

        private void OnPortalExit(Teleportable teleportable, Portal portal)
        {
            if (_candidates.First(x => x.Teleportable == teleportable).Portal != portal)
            {
                return;
            }

            TeleportableData data = new(teleportable, portal);
            _candidates.Remove(data);
        }

        private Portal GetOtherPortal(Portal portal) => portal == _portal1 ? _portal2 : _portal1;

        private void Teleport(ref TeleportableData data)
        {
            Transform teleportableTransform = data.Teleportable.transform;
            Portal entrance = data.Portal;
            Portal exit = GetOtherPortal(entrance);

            data.Portal = exit;

            Matrix4x4 newTransformMatrix
                = exit.transform.localToWorldMatrix
                  * entrance.transform.worldToLocalMatrix
                  * teleportableTransform.localToWorldMatrix;

            teleportableTransform.SetPositionAndRotation
            (
                newTransformMatrix.GetPosition()
                + ExitPushAmount  * entrance.CalculatePortalSide(teleportableTransform.position) * exit.transform.forward,
                newTransformMatrix.rotation
            );

            data.UpdatePortalSide();
        }
    }
}
