using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP09
{
    public class ScreenAnchor : MonoBehaviour
    {
        public enum PivotType
        {
            LeftBottom,
            CenterBottom,
            RightBottom,
            LeftCenter,
            Center,
            RightCenter,
            LeftTop,
            CenterTop,
            RightTop
        }

        private static Camera targetCamera;

        [SerializeField] private PivotType pivot;
        [SerializeField] private float depth;

        public PivotType Pivot
        {
            get
            {
                return pivot;
            }
            set
            {
                if (pivot == value) return;
                pivot = value;
                UpdatePivot();
            }
        }

        public float Depth
        {
            get
            {
                return depth;
            }
            set
            {
                if (Mathf.Approximately(depth, value)) return;
                depth = value;
                UpdatePivot();
            }
        }


        public void SetPivotAndDepth(PivotType p, float d)
        {
            pivot = p;
            depth = d;
            UpdatePivot();
        }


        public static void UpdateBoundary(Camera cam = null)
        {
            if (cam != null) targetCamera = cam;
            if (targetCamera == null) targetCamera = Camera.main;

            targetCamera.ViewportToWorldPoint(Vector3.zero);
        }


        public void UpdatePivot()
        {
            if (targetCamera == null) targetCamera = Camera.main;

            float clipDepth = depth + targetCamera.nearClipPlane;

            var leftBottom = targetCamera.ViewportToWorldPoint(new Vector3(0F, 0F, clipDepth));
            var rightBottom = targetCamera.ViewportToWorldPoint(new Vector3(1F, 0F, clipDepth));
            var rightTop = targetCamera.ViewportToWorldPoint(new Vector3(1F, 1F, clipDepth));

            float width = (rightBottom - leftBottom).magnitude;
            float height = (rightTop - rightBottom).magnitude;

            transform.position = leftBottom + targetCamera.transform.right * ((int)pivot % 3) * width * 0.5f
                                            + targetCamera.transform.up * ((int)pivot / 3) * height * 0.5f;
        }


        private void Awake()
        {
            UpdatePivot();
        }


#if UNITY_EDITOR
        private void Update()
        {
            UpdatePivot();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
#endif
    }
}