using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP09
{
    public class ScreenAnchorCreator : MonoBehaviour
    {
        [SerializeField] private ScreenAnchor.PivotType pivot = ScreenAnchor.PivotType.Center;
        [SerializeField, Range(1, 30)] private int count = 5;
        [SerializeField] private float gap = 1F;

        private List<ScreenAnchor> anchors = new List<ScreenAnchor>();

        private void Update()
        {
            // Sync count
            while (anchors.Count < count)
            {
                anchors.Add(new GameObject().AddComponent<ScreenAnchor>());
                anchors[anchors.Count - 1].transform.parent = transform;
                anchors[anchors.Count - 1].Pivot = pivot;
            }
            while (anchors.Count > count)
            {
                Destroy(anchors[anchors.Count - 1].gameObject);
                anchors.RemoveAt(anchors.Count - 1);
            }


            for (int i = 0; i < count; ++i)
            {
                anchors[i].SetPivotAndDepth(pivot, gap * i);
            }
        }
    }
}