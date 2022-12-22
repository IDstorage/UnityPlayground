namespace UP08
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // PoolObject
    using UP01;

    [RequireComponent(typeof(PoolObject))]
    public abstract class Popup : MonoBehaviour, IUILayout
    {
        // [SerializeField] protected string layoutName;
        public string LayoutName => name;

        [SerializeField] protected Canvas canvas;

        public abstract IEnumerator OnEnter(UIEventParam param = null);
        public abstract IEnumerator OnExit();

        public void Hide()
        {
            PopupController.Hide(this);
        }

        public void Focus()
        {
            PopupController.Focus(this);
        }

        public void SetCanvasOrder(int order)
        {
            canvas.sortingOrder = order;
        }
    }
}