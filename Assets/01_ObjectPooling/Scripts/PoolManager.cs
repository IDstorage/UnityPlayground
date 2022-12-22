namespace UP01
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    // Smart Coroutine
    using UP04;

    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; } = null;

        [SerializeField] private AssetLabelReference[] recipeLabels;
        private List<PoolRecipe> recipes = new List<PoolRecipe>();

        private Dictionary<string, Queue<PoolObject>> pool = new Dictionary<string, Queue<PoolObject>>();


        private bool isInitialized = false;


        private void Awake()
        {
            if (!ReferenceEquals(Instance, null))
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;

            SmartCoroutine.Create(CoInitialize());

            IEnumerator CoInitialize()
            {
                isInitialized = false;

                for (int i = 0; i < recipeLabels.Length; ++i)
                {
                    var op = Addressables.LoadAssetAsync<PoolRecipe>(recipeLabels[i]);
                    yield return op;

                    if (op.Result == null) continue;

                    var cloneRecipe = op.Result.Clone();
                    cloneRecipe.Initialize();
                    recipes.Add(cloneRecipe);
                }

                isInitialized = true;
            }
        }


        private PoolObject Find(string name)
        {
            for (int i = 0; i < recipes.Count; ++i)
            {
                var target = recipes[i].Find(name);
                if (target != null) return target;
            }
            return null;
        }


        public bool WarmUp(string name, int count)
        {
            var target = Find(name);
            if (target == null) return false;

            if (!pool.ContainsKey(name))
            {
                pool.Add(name, new Queue<PoolObject>());
            }

            for (int i = 0; i < count; ++i)
            {
                var obj = Instantiate(target.gameObject).GetComponent<PoolObject>();
                obj.LayoutName = name;
                obj.Return();
            }

            return true;
        }


        public PoolObject Get(string name)
        {
            if (!pool.ContainsKey(name))
            {
                bool ret = WarmUp(name, 10);
                if (!ret) return null;
            }

            if (pool[name].Count == 0)
            {
                bool ret = WarmUp(name, 1);
                if (!ret) return null;
            }

            var obj = pool[name].Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Add(PoolObject obj)
        {
            obj.gameObject.SetActive(false);
            pool[obj.LayoutName].Enqueue(obj);
        }
    }
}