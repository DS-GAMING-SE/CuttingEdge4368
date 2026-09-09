using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CuttingEdge
{
    [ExecuteInEditMode]
    public class Plane : MonoBehaviour
    {
        public static Plane enemyPlane;
        public static Plane playerPlane;
        public float width { get; private set; }
        public float height { get; private set; }
        public float distanceFromCamera { get { return (Camera.main.transform.position - transform.position).magnitude; } }
        private Vector3[] corners = new Vector3[4];
        public PlaneType planeType { get { return _planeType; } }
        [SerializeField]
        private PlaneType _planeType;
        private void Awake()
        {
            SetBoundsFromCamera();
        }

        [ContextMenu("Set Bounds From Camera")]
        public void SetBoundsFromCamera()
        {
            if (Camera.main)
            {
                transform.rotation = Camera.main.transform.rotation;
                float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                Camera.main.CalculateFrustumCorners(new Rect(0, 0, 1, 1), distance, Camera.MonoOrStereoscopicEye.Mono, corners);
                height = (corners[1] - corners[0]).magnitude;
                width = (corners[0] - corners[3]).magnitude;
            }
            transform.hasChanged = false;
        }
        private void OnEnable()
        {
            if (planeType == PlaneType.Player && !playerPlane)
            {
                playerPlane = this;
            }
            if (planeType == PlaneType.Enemy && !enemyPlane)
            {
                enemyPlane = this;
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (transform.hasChanged)
            {
                SetBoundsFromCamera();
            }
        }
#endif
        private void OnDisable()
        {
            if (playerPlane == this)
            {
                playerPlane = null;
            }
            if (enemyPlane == this)
            {
                enemyPlane = null;
            }
        }
        public Vector3 GetPositionFromScreenPosition(Vector2 screenPosition)
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, distanceFromCamera));
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Camera.main.transform.position + corners[0], Camera.main.transform.position + corners[1]);
            Gizmos.DrawLine(Camera.main.transform.position + corners[1], Camera.main.transform.position + corners[2]);
            Gizmos.DrawLine(Camera.main.transform.position + corners[2], Camera.main.transform.position + corners[3]);
            Gizmos.DrawLine(Camera.main.transform.position + corners[3], Camera.main.transform.position + corners[0]);
        }

        public enum PlaneType
        {
            None,
            Player,
            Enemy
        }
    }
}
