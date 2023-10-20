using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

namespace Portal
{
    [RequireComponent(typeof(Camera))]
    public class PortalCamera : MonoBehaviour
    {
        [SerializeField] private GameObject _portal;
        [SerializeField] private GameObject _otherPortal;

        private Camera _camera;

        private static int _textureID = Shader.PropertyToID("RenderTexture");

        private Action<ScriptableRenderContext, Camera> _renderListener;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            SetRenderTexture();
            _renderListener = OnRender;
        }

        private void LateUpdate()
        {
            Matrix4x4 matrix
                = _otherPortal.transform.worldToLocalMatrix
                  * _portal.transform.localToWorldMatrix
                  * Camera.main.transform.localToWorldMatrix;

            transform.SetPositionAndRotation(matrix.GetPosition(), matrix.rotation);
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += _renderListener;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= _renderListener;
        }

        private void OnRender(ScriptableRenderContext context, Camera renderedCamera)
        {
            if (renderedCamera != _camera)
            {
                return;
            }

            Plane plane = new(GetPortalNormal(), _portal.transform.position);
            Vector4 clipPlaneWorldSpace = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
            Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(_camera.cameraToWorldMatrix) * clipPlaneWorldSpace;
            _camera.projectionMatrix = Camera.main.CalculateObliqueMatrix(clipPlaneCameraSpace);
        }

        private Vector3 GetPortalNormal()
        {
            Transform portalTransform = _portal.transform;
            if (Mathf.Sign(Vector3.Dot(transform.forward, portalTransform.forward)) < 0.0f)
            {
                return -portalTransform.forward;
            }

            return portalTransform.forward;
        }

        private void SetRenderTexture()
        {
            VisualEffect visualEffect = _otherPortal.GetComponent<VisualEffect>();
            RenderTexture renderTexture = new(Screen.width, Screen.height, 16, RenderTextureFormat.ARGBFloat);
            _camera.targetTexture = renderTexture;
            visualEffect.SetTexture(_textureID, renderTexture);
        }
    }
}
