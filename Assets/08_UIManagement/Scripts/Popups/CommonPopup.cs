namespace UP08
{
    using System.Collections;
    using UnityEngine;

    public class CommonPopup : Popup
    {
        [SerializeField] private Color logColor;
        [SerializeField] private UnityEngine.UI.Text titleText;

        public override IEnumerator OnEnter(UIEventParam param = null)
        {
            titleText.text = Random.Range(100, 100000).ToString();
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(logColor)}> >>> {LayoutName} Enter</color>");
            yield break;
        }

        public override IEnumerator OnExit()
        {
            Debug.Log($"<color={ColorUtility.ToHtmlStringRGB(logColor)}> <<< {LayoutName} Exit</color>");
            yield break;
        }

        private void Update()
        {
            if (ReferenceEquals(PopupController.Current, this)) Debug.Log(this.titleText.text);
        }
    }
}