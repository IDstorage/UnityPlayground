namespace UP08
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // SmartCoroutine
    using UP04;

    public class SceneController : UIController<Scene>
    {
        public enum LoadSceneMode
        {
            Single,
            Additive
        }

        public static CustomLinkedList<Scene> Addition { get; protected set; } = new CustomLinkedList<Scene>();


        public static void Open(string name, LoadSceneMode mode = LoadSceneMode.Single, UIEventParam param = null)
        {
            SmartCoroutine.Create(OpenAsync(name, mode, param));
        }

        public static IEnumerator OpenAsync(string name, LoadSceneMode mode = LoadSceneMode.Single, UIEventParam param = null)
        {
            Debug.Assert(layouts.ContainsKey(name), $"No such scene: {name}");

            if (mode == LoadSceneMode.Additive)
            {
                var addedScene = layouts[name];
                addedScene.gameObject.SetActive(true);
                Addition.AddLast(addedScene);
                yield return addedScene.OnEnter(param);
                yield break;
            }

            Close();

            Current = layouts[name];
            Current.gameObject.SetActive(true);
            yield return Current.OnEnter(param);
        }

        public static void Close()
        {
            var dummyCurrent = Current;
            Current = null;
            SmartCoroutine.Create(CoClose(dummyCurrent));
        }

        private static IEnumerator CoClose(Scene current)
        {
            for (var iter = Addition.Begin; iter != null; ++iter)
            {
                if (iter.Current.Data == null) continue;

                iter.Current.Data.gameObject.SetActive(false);
                yield return iter.Current.Data.OnExit();
            }

            Addition.Clear();

            if (current != null)
            {
                current.gameObject.SetActive(false);
                yield return current.OnExit();
            }
        }

        public static void Close(string name)
        {
            Debug.Assert(layouts.ContainsKey(name));

            var findRet = Addition.Find(ui => ui.LayoutName.CompareTo(name) == 0);
            if (findRet == null) return; // Throw exception?

            Addition.Remove(findRet);

            findRet.gameObject.SetActive(false);
            SmartCoroutine.Create(findRet.OnExit());
        }
    }
}