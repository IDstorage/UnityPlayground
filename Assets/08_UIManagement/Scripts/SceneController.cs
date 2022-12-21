namespace UP08
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // SmartCoroutine
    using UP04;

    public class SceneController : UIController<Scene>
    {
        public static void Change(string name, UIEventParam param = null)
        {
            if (layoutDictionary.ContainsKey(name))
            {

            }
            Debug.Assert(layoutDictionary.ContainsKey(name));

            SmartCoroutine.Create(CoExecute());

            IEnumerator CoExecute()
            {
                if (Current != null)
                {
                    Current.gameObject.SetActive(false);
                }

                Current = layoutDictionary[name];
                Current.gameObject.SetActive(true);
                yield return Current.OnEnter(param);
            }
        }

        public static IEnumerator Release()
        {
            if (Current == null) yield break;
            yield return Current.OnExit();
        }
    }
}