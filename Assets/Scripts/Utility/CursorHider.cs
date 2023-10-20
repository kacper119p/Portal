using System;
using UnityEngine;

namespace Utility
{
    public class CursorHider : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
