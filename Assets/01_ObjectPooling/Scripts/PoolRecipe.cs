using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP01
{
    [CreateAssetMenu(menuName = "UP01/Pool Recipe", fileName = "PoolRecipe")]
    public class PoolRecipe : ScriptableObject
    {
        private Dictionary<string, PoolObject> pool = new Dictionary<string, PoolObject>();
        private bool isInitialized = false;

        public PoolObject[] objectList;


        public void Initialize()
        {
            pool.Clear();
            for (int i = 0; i < objectList.Length; ++i)
            {
                var obj = objectList[i];
                if (pool.ContainsKey(obj.name))
                {
                    Debug.LogWarning($"Duplicated pool object was detected. System will ignore this (objName: {obj.name})");
                    continue;
                }
                pool.Add(obj.name, obj);
            }

            isInitialized = true;
        }

        public PoolObject Find(string name)
        {
            if (isInitialized)
            {
                return pool.TryGetValue(name, out var obj) ? obj : null;
            }

            for (int i = 0; i < objectList.Length; ++i)
            {
                if (objectList[i].name != name) continue;
                return objectList[i];
            }
            return null;
        }
    }
}