namespace UP08
{
    using System.Collections.Generic;
    using UnityEngine;

    // SmartCoroutine
    using UP04;

    public class UIController<T> : MonoBehaviour where T : UILayout
    {
        protected static Dictionary<string, LinkedList<T>> layoutDictionary = new Dictionary<string, LinkedList<T>>();
        public static T Current { get; protected set; }

        [SerializeField] private T firstLayout;

        protected virtual void Awake()
        {
            Current = null;

            layoutDictionary.Clear();

            var layouts = GetComponentsInChildren<T>(true);
            for (int i = 0; i < layouts.Length; ++i)
            {
                var list = new LinkedList<T>();
                list.AddLast(layouts[i]);

                layoutDictionary.Add(layouts[i].name, list);

                if (!layouts[i].gameObject.activeSelf) continue;
                Current = layouts[i];
            }

            if (firstLayout != null) Current = firstLayout;

            if (Current == null) return;

            SmartCoroutine.Create(Current.OnEnter());
        }
    }
}