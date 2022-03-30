using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP01
{
    public class PoolTest : MonoBehaviour
    {
        private Stack<PoolObject> pool = new Stack<PoolObject>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.PageUp)) 
            {
                var obj = PoolManager.Instance.Get("Pool");
                obj.transform.position = Random.insideUnitSphere * 5F;
                pool.Push(obj);
            }
            if (Input.GetKeyDown(KeyCode.PageDown) && pool.Count > 0)
            {
                var obj = pool.Pop();
                obj.Return();
            }
        }
    }
}