using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP01
{
    public class PoolObject : MonoBehaviour
    {
        public string LayoutName { get; set; }

        public void Return()
        {
            PoolManager.Instance.Add(this);
        }
    }
}