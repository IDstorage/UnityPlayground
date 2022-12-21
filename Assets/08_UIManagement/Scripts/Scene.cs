namespace UP08
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Scene : MonoBehaviour, IUILayout
    {
        public abstract IEnumerator OnEnter(UIEventParam param = null);
        public abstract IEnumerator OnExit();
    }
}