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


        protected override void Awake()
        {
            base.Awake();

            Addition.Clear();
        }


        public static void Open(string name, LoadSceneMode mode = LoadSceneMode.Single, UIEventParam param = null)
        {
            SmartCoroutine.Create(OpenAsync(name, mode, param));
        }

        public static void Close()
        {
            SmartCoroutine.Create(CloseAsync());
        }
        public static void Close(string name)
        {
            SmartCoroutine.Create(CloseAsync(name));
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

            yield return CloseAdditionAsync();
            Close();

            Current = layouts[name];
            Current.gameObject.SetActive(true);
            yield return Current.OnEnter(param);
        }

        private static IEnumerator CloseAdditionAsync()
        {
            List<SmartCoroutine> release = new List<SmartCoroutine>();
            for (var iter = Addition.Begin; iter != null; ++iter)
            {
                if (iter.Current.Data == null) continue;

                iter.Current.Data.gameObject.SetActive(false);

                release.Add(SmartCoroutine.Create(iter.Current.Data.OnExit));
                release[release.Count - 1].Start();
            }

            while (release.Count > 0)
            {
                for (int i = 0; i < release.Count; ++i)
                {
                    if (release[i].IsRunning) continue;

                    release[i] = release[release.Count - 1];
                    release.RemoveAt(release.Count - 1);
                }

                yield return null;
            }

            Addition.Clear();
        }
        public static IEnumerator CloseAsync()
        {
            var dummyCurrent = Current;
            Current = null;

            yield return CloseAdditionAsync();

            if (dummyCurrent != null)
            {
                dummyCurrent.gameObject.SetActive(false);
                yield return dummyCurrent.OnExit();
            }
        }
        public static IEnumerator CloseAsync(string name)
        {
            Debug.Assert(layouts.ContainsKey(name));

            var findRet = Addition.Find(ui => ui.LayoutName.CompareTo(name) == 0);
            if (findRet == null) yield break; // Throw exception?

            Addition.Remove(findRet);

            findRet.gameObject.SetActive(false);
            yield return findRet.OnExit();
        }


        public override IEnumerator ReleaseAsync()
        {
            yield return CloseAsync();
        }
    }
}