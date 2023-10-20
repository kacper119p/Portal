using JetBrains.Annotations;
using UnityEngine;

namespace Utility
{
    public static class MathHelpers
    {
        [Pure]
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < 0.0f)
            {
                angle = 360.0f + angle;
            }

            if (angle > 180.0f)
            {
                return Mathf.Max(angle, 360 + min);
            }

            return Mathf.Min(angle, max);
        }
    }
}
