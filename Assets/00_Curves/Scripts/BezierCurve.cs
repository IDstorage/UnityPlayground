using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UP00
{
    public class BezierCurve : MonoBehaviour
    {
        [SerializeField, Range(0.01f, 1F)]
        private float deltaThreshold;

        [System.NonSerialized]
        public List<Transform> bezierPoints;

        public void Init()
        {
            bezierPoints = GetComponentsInChildren<Transform>().ToList();
            bezierPoints.RemoveAt(0);
        }

        private Vector3 NextPosition(float t)
        {
            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < bezierPoints.Count; ++i) list.Add(bezierPoints[i].position);
            while (list.Count > 1)
            {
                for (int i = 0; i < list.Count - 1; ++i)
                {
                    list[i] = Vector3.Lerp(list[i], list[i + 1], t);
                }
                list.RemoveAt(list.Count - 1);
            }
            return list[0];
        }

        private void OnDrawGizmos()
        {
            if (bezierPoints == null) return;

            float t = deltaThreshold;
            Vector3 prev = bezierPoints[0].position;
            while (t <= 1F)
            {
                Vector3 curr = NextPosition(t);
                Gizmos.DrawLine(prev, curr);
                prev = curr;
                t += deltaThreshold;
            }
        }
    }
}

#if UNITY_EDITOR
namespace UP00
{
    using UnityEditor;

    [CustomEditor(typeof(BezierCurve))]
    public class BezierCurveEditor : Editor
    {
        private void OnSceneGUI()
        {
            var self = target as BezierCurve;

            if (self.bezierPoints == null) return;

            for (int i = 0; i < self.bezierPoints.Count; ++i)
            {
                Handles.Label(self.bezierPoints[i].position, $"Point{i + 1}");
                self.bezierPoints[i].position = Handles.DoPositionHandle(self.bezierPoints[i].position, self.bezierPoints[i].rotation);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            if (GUILayout.Button("Draw"))
            {
                (target as BezierCurve).Init();
            }
        }
    }
}
#endif
