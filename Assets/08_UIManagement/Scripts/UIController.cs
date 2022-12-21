namespace UP08
{
    using System.Collections.Generic;
    using UnityEngine;

    // SmartCoroutine
    using UP04;

    public class UIController<T> : MonoBehaviour where T : MonoBehaviour, IUILayout
    {
        protected static Dictionary<string, T> layoutDictionary = new Dictionary<string, T>();
        public static T Current { get; protected set; }

        [SerializeField] private T firstLayout;

        protected virtual void Awake()
        {
            Current = null;

            var layouts = GetComponentsInChildren<T>(true);
            for (int i = 0; i < layouts.Length; ++i)
            {
                layoutDictionary.Add(layouts[i].name, layouts[i]);
                if (!layouts[i].gameObject.activeSelf) continue;
                Current = layouts[i];
            }

            if (firstLayout != null) Current = firstLayout;

            if (Current == null) return;

            SmartCoroutine.Create(Current.OnEnter());
        }
    }
}