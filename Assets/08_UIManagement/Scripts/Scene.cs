namespace UP08
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Scene : MonoBehaviour, IUILayout
    {
        [SerializeField] protected string layoutName;
        public string LayoutName => layoutName;

        public abstract IEnumerator OnEnter(UIEventParam param = null);
        public abstract IEnumerator OnExit();
    }
}