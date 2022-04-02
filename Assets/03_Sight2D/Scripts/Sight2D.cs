using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP03 {
    public class Sight2D : MonoBehaviour
    {
        [SerializeField]
        private Vector2 range = Vector2.up;

        [SerializeField]
        private float sightAngle = 45F;
        [SerializeField]
        private float targetAngle = 0F;


        [SerializeField]
        private List<Transform> targetList = new List<Transform>();


        private void Start()
        {
            var list = GameObject.FindGameObjectsWithTag("UP03_Target");
            for (int i = 0; i < list.Length; ++i) 
            {
                targetList.Add(list[i].transform);
            }
        }


        private void Update() 
        {
            Vector3 forward = Quaternion.AngleAxis(targetAngle, Vector3.up) * transform.forward;
            for (int i = 0; i < targetList.Count; ++i) 
            {
                Vector3 dir = targetList[i].position - transform.position;
                if (range.x * range.x > dir.sqrMagnitude || range.y * range.y < dir.sqrMagnitude) {
                    targetList[i].gameObject.SetActive(false);
                    continue;
                }

                float dot = Vector3.Dot(forward, dir.normalized);
                float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                
                targetList[i].gameObject.SetActive(angle < sightAngle * 0.5f);
            }

            Debug.DrawRay(transform.position, forward, Color.red);
            Quaternion sightQuat = Quaternion.AngleAxis(sightAngle * 0.5f, Vector3.up);
            Vector3 right = sightQuat * forward;
            Debug.DrawRay(transform.position + right * range.x, right * (range.y - range.x), Color.green);
            Vector3 left = Quaternion.Inverse(sightQuat) * forward;
            Debug.DrawRay(transform.position + left * range.x, left * (range.y - range.x), Color.green);

            Debug.DrawLine(transform.position + left * range.x, transform.position + right * range.x, Color.green);
            Debug.DrawLine(transform.position + left * range.y, transform.position + right * range.y, Color.green);
        }
    }
}