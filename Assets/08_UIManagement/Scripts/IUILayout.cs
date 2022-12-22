namespace UP08
{
    using UnityEngine;
    using System.Collections;

    public class UIEventParam { }

    public abstract class UILayout : UnityEngine.MonoBehaviour
    {
        [SerializeField] protected string layoutName;
        public string Name => layoutName;

        public abstract IEnumerator OnEnter(UIEventParam param = null);
        public abstract IEnumerator OnExit();
    }
}