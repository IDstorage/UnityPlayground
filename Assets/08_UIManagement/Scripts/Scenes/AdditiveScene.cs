namespace UP08
{
    using System.Collections;
    using UnityEngine;

    public class AdditiveScene : Scene
    {
        public override IEnumerator OnEnter(UIEventParam param = null)
        {
            Debug.Log($">>>>> {Name} Enter");
            yield break;
        }

        public override IEnumerator OnExit()
        {
            Debug.Log($"<<<<< {Name} Exit");
            yield break;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}