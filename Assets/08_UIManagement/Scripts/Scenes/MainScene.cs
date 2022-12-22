namespace UP08
{
    using System.Collections;
    using UnityEngine;

    public class MainScene : Scene
    {
        public override IEnumerator OnEnter(UIEventParam param = null)
        {
            Debug.Log(">>> MainScene Enter");
            yield break;
        }

        public override IEnumerator OnExit()
        {
            Debug.Log("<<< MainScene Exit");
            yield break;
        }
    }
}