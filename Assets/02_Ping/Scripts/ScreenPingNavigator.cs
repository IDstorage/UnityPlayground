using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UP02
{
    public class ScreenPingNavigator : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private float radius = 0F;

        private Stack<UP01.PoolObject> marks = new Stack<UP01.PoolObject>();

        private void Update()
        {
            while (marks.Count > Ping.pingList.Count)
            {
                marks.Pop().Return();
            }
            
            while (marks.Count < Ping.pingList.Count)
            {
                marks.Push(UP01.PoolManager.Instance.Get("PingMark"));
                marks.Peek().transform.parent = canvas.transform;
            }

            int rangeX = (int)(Screen.width * 0.5f) - 40;
            int rangeY = (int)(Screen.height * 0.5f) - 40;

            int index = 0;

            Quaternion inverseRot = Quaternion.Inverse(transform.rotation);

            foreach (var mark in marks)            
            {
                mark.gameObject.SetActive(!Ping.pingList[index].IsVisible);
                float rad = ((inverseRot * Quaternion.LookRotation(Ping.pingList[index].transform.position - transform.position, transform.up)).eulerAngles.y - 90F) * -Mathf.Deg2Rad;
                Vector2 angle = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                mark.transform.localEulerAngles = Vector3.forward * rad * Mathf.Rad2Deg;
                (mark.transform as RectTransform).anchoredPosition = new Vector2(Mathf.Clamp(angle.x * radius, -rangeX, rangeX), Mathf.Clamp(angle.y * radius, -rangeY, rangeY));
                index++;
            }
        }
    }
}