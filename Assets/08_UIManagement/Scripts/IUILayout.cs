namespace UP08
{
    using System.Collections;

    public class UIEventParam { }

    public interface IUILayout
    {
        IEnumerator OnEnter(UIEventParam param = null);
        IEnumerator OnExit();
    }
}