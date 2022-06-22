using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP02
{
    public class PlayerCameraMove : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = Vector3.zero;


        float minZoom = 5.0f;
        float maxZoom = 10.0f;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            transform.RotateAround(target.position, Vector3.up, 360.0f * Time.deltaTime * Input.GetAxisRaw("Mouse X"));

            if (Input.GetKey("q"))
            {
                // rotate toward left Yaxis
                transform.RotateAround(target.position, Vector3.up, 90.0f * Time.deltaTime);

                offset = transform.position - target.position;
                offset.Normalize();
            }
            if (Input.GetKey("e"))
            {
                transform.RotateAround(target.position, Vector3.up, -90.0f * Time.deltaTime);

                offset = transform.position - target.position;
                offset.Normalize();
            }
        }

        void LateUpdate()
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }

    }
}