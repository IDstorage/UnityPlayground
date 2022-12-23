namespace UP08
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // SmartCoroutine
    using UP04;

    public abstract class UIController<T> : MonoBehaviour where T : MonoBehaviour, IUILayout
    {
        public static T Current { get; protected set; }
        protected static Dictionary<string, T> layouts = new Dictionary<string, T>();

        protected static UIController<T> instance = null;

        protected virtual void Awake()
        {
            instance = this;

            Current = null;

            layouts.Clear();

            var layoutObjects = GetComponentsInChildren<T>(true);
            for (int i = 0; i < layoutObjects.Length; ++i)
            {
                var obj = layoutObjects[i];
                layouts.Add(obj.LayoutName, obj);

                if (!obj.gameObject.activeSelf) continue;
                Current = obj;
            }

            if (Current == null) return;

            SmartCoroutine.Create(Current.OnEnter());
        }


        public virtual void Release()
        {
            SmartCoroutine.Create(ReleaseAsync());
        }
        public abstract IEnumerator ReleaseAsync();
    }
}