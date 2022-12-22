namespace UP08
{
    using UnityEngine;
    using System.Collections;

    public class UIEventParam { }

    public interface IUILayout
    {
        string LayoutName { get; }

        IEnumerator OnEnter(UIEventParam param = null);
        IEnumerator OnExit();
    }
}