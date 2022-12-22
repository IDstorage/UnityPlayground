namespace UP08
{
    using System.Collections;
    using UnityEngine;

    public class SubScene : Scene
    {
        public override IEnumerator OnEnter(UIEventParam param = null)
        {
            Debug.Log(">>> SubScene Enter");
            yield break;
        }

        public override IEnumerator OnExit()
        {
            Debug.Log("<<< SubScene Exit");
            yield break;
        }
    }
}