namespace UP08
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // PoolManager
    using UP01;

    // SmartCoroutine
    using UP04;

    public class PopupController : MonoBehaviour
    {
        public static Popup Current { get; protected set; }
        public static List<Popup> PopupStack { get; protected set; } = new List<Popup>();

        private static Dictionary<string, Popup> layouts = new Dictionary<string, Popup>();


        protected virtual void Awake()
        {
            Current = null;

            layouts.Clear();

            var layoutObjects = GetComponentsInChildren<Popup>(true);
            for (int i = 0; i < layoutObjects.Length; ++i)
            {
                var obj = layoutObjects[i];
                layouts.Add(obj.name, obj);

                if (!obj.gameObject.activeSelf) continue;
                Current = obj;
            }

            if (Current == null) return;

            SmartCoroutine.Create(Current.OnEnter());
        }


        public static void Show(string name, UIEventParam param = null)
        {
            Popup popup = null;

            if (!layouts.TryGetValue(name, out popup))
            {
                popup = PoolManager.Instance.Get(name).GetComponent<Popup>();
            }

            if (popup == null) return;

            SmartCoroutine.Create(CoShow(popup));

            IEnumerator CoShow(Popup popup)
            {
                popup.SetCanvasOrder(PopupStack.Count);
                PopupStack.Add(popup);
                yield return popup.OnEnter(param);
            }
        }

        public static void Hide()
        {
            SmartCoroutine.Create(CoHide());

            IEnumerator CoHide()
            {
                var hiddenPopup = PopupStack[PopupStack.Count - 1];
                PopupStack.RemoveAt(PopupStack.Count - 1);
                yield return hiddenPopup.OnExit();
            }
        }

        public static void Hide(string name)
        {
            if (Focus(name)) Hide();
        }
        public static void Hide(Popup popup)
        {
            if (Focus(popup)) Hide();
        }


        public static bool Focus(string name)
        {
            int idx = FindIndex(name);
            if (idx < 0) return false;

            Focus(idx);
            return true;
        }
        public static bool Focus(Popup popup)
        {
            int idx = FindIndex(popup);
            if (idx < 0) return false;

            Focus(idx);
            return true;
        }
        private static void Focus(int idx)
        {
            var temp = PopupStack[idx];
            PopupStack[idx] = PopupStack[PopupStack.Count - 1];
            PopupStack[PopupStack.Count - 1] = temp;

            PopupStack[idx].SetCanvasOrder(idx);
            PopupStack[PopupStack.Count - 1].SetCanvasOrder(PopupStack.Count - 1);
        }

        public static int FindIndex(string name)
        {
            return PopupStack.FindIndex(p => p.Name.CompareTo(name) == 0);
        }
        public static int FindIndex(Popup popup)
        {
            return PopupStack.FindIndex(p => ReferenceEquals(p, popup));
        }

        public static bool IsOpened(string name)
        {
            return FindIndex(name) >= 0;
        }
        public static bool IsOpened(Popup popup)
        {
            return FindIndex(popup) >= 0;
        }
    }
}