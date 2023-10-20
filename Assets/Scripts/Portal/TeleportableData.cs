using System;
using UnityEngine;

namespace Portal
{
    public struct TeleportableData
    {
        private readonly Teleportable _teleportable;
        private Portal _portal;
        private float _direction;

        public Teleportable Teleportable => _teleportable;

        public Portal Portal
        {
            get => _portal;
            set => _portal = value;
        }

        public TeleportableData(Teleportable teleportable, Portal portal)
        {
            _teleportable = teleportable;
            _portal = portal;
            _direction = portal.CalculatePortalSide(teleportable.transform.position);
        }

        public override bool Equals(object obj)
        {
            if (obj is not TeleportableData data)
            {
                return false;
            }

            return Equals(data);
        }

        public bool Equals(TeleportableData data)
        {
            return _teleportable == data._teleportable;
        }

        public static bool operator ==(TeleportableData a, TeleportableData b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TeleportableData a, TeleportableData b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_teleportable);
        }

        /// <summary>
        /// Updates direction to a portal.
        /// </summary>
        /// <returns>True if direction changed</returns>
        public bool UpdatePortalSide()
        {
            float old = _direction;
            _direction = Portal.CalculatePortalSide(_teleportable.transform.position);
            return !Mathf.Approximately(Mathf.Sign(old), Mathf.Sign(_direction));
        }
    }
}
