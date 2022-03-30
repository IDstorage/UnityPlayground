using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UP01
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager instance = null;
        public static PoolManager Instance => instance ?? (instance = GameObject.FindObjectOfType<PoolManager>());

        [SerializeField]
        private PoolRecipe recipe;


        private Dictionary<string, Queue<PoolObject>> pool = new Dictionary<string, Queue<PoolObject>>();


        private void Awake()
        {
            if (!ReferenceEquals(instance, null))
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            instance = this;
        }



        public void WarmUp(string name, int count)
        {
            var target = recipe.Find(name);
            if (target == null) return;

            if (!pool.ContainsKey(name))
            {
                pool.Add(name, new Queue<PoolObject>());
            }

            for (int i = 0; i < count; ++i)
            {
                var obj = Instantiate(target.gameObject).GetComponent<PoolObject>();
                obj.Name = name;
                obj.Return();
            }
        }


        public PoolObject Get(string name)
        {
            if (!pool.ContainsKey(name))
            {
                WarmUp(name, 5);
            }

            if (pool[name].Count == 0 )
            {
                WarmUp(name, 1);
            }

            var obj = pool[name].Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Add(PoolObject obj)
        {
            obj.gameObject.SetActive(false);
            pool[obj.Name].Enqueue(obj);
        }
    }
}